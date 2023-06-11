// This MonoBehaviour class handles displaying the game board UI to the user.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardDisplay : MonoBehaviour
{
	// Prefab used for spaces.
	public Transform spacePrefab;

	// The amount of padding between spaces on the board.
	public float paddingFactor = 1.1f;
	
	public bool FlipEveryTurn { get; private set; }

	// List of references to the move buttons.
	private List<MoveButton> moveButtons;

	// A reference to the game object.
	private Game game;

	// A matrix that keeps track of the Space objects on the displayed board.
	private List<List<Space>> boardDisplay;

	// The location of the piece being used as an anchor for selection.
	private Vector anchorLocation;

	// The locations of the selectables / selected.
	private List<Vector> selectables;
	private List<Vector> selected;

	// Boolean flags indicating whether or not selectables / selected are being shown. 
	public bool showingSelectables;
	public bool showingSelected;

	void Awake()
	{
		// Grab reference to game object.
		game = GameObject.Find("Game").GetComponent<Game>();
		// Then grab the movement buttons.
		moveButtons = new List<MoveButton>();
		foreach (var direction in Board.Directions)
		{
			var moveButton = GameObject.Find("Move" + direction).GetComponent<MoveButton>();
			moveButtons.Add(moveButton);
		}
		
		showingSelectables = showingSelected = FlipEveryTurn = false;

		boardDisplay = new List<List<Space>>();
		
		var spaceDiameter = spacePrefab.localScale.x * transform.localScale.x * paddingFactor;
		var spaceRadius = spaceDiameter / 2;

		var boardRadius = Board.height / 2;

		var xOffset = boardRadius * spaceDiameter;

		for (int i = 0; i < Board.height; i++)
		{
			
			var length = Board.rowLengths[i];
			var row = new List<Space>();
			var x = (Board.height - length) * spaceRadius  - xOffset;
			var y = (boardRadius - i) * spaceDiameter;
			for (int j = 0; j < length; j++)
			{

				var position = new Vector3(x, y, 0);
				var space = Instantiate(spacePrefab, position, Quaternion.identity, transform).GetComponent<Space>();
				
				space.Location = new Vector(i, j);
				row.Add(space);

				x += spaceDiameter;
			}
			boardDisplay.Add(row);
		}
	}

	void Start()
	{
		
		UpdateView();
	}

	
	public void UpdateView()
	{
		int i = 0;
		foreach (var row in game.Board.View())
		{
			int j = 0;
			foreach (var slot in row)
			{
				if (slot == 'O')
				{
					boardDisplay[i][j].Clear();
				}
				else
				{
					boardDisplay[i][j].SetPiece(slot);
				}
				j++;
			}
			i++;
		}
	}




	public void Restart()
	{
		// First update the pieces and disable the movement buttons.
		UpdateView();
		DisableMoveButtons();
		//Debug.Log("updated view");
		//Debug.Log(transform.eulerAngles.z);

	}
	
	// DisableMoveButtons disables all movement buttons.
	public void DisableMoveButtons()
	{
		foreach (var button in moveButtons)
		{
			button.Disable();
		}
	}

	// Anchor marks a piece on the board as the anchor and indicates the selectable pieces.
	// Selectable pieces are those that can be used to make a selection by clicking on them.
	// The corresponding selection consists of consecutive game pieces starting with the anchor and ending with the selectable.
	public void Anchor(Vector anchorLocation)
	{	
		// Before we anchor, we must clear any selectables or selection currently active on the board.
		if (showingSelectables) ClearSelectables();
		else if (showingSelected) ClearSelected();
		this.anchorLocation = anchorLocation;
		selectables = new List<Vector>();
		// Iterate over the selectables gotten from the logic board, marking them as appropriate.
		foreach (var pair in game.Board.GetSelectables(anchorLocation))
		{
			var location = pair.Item1; var direction = pair.Item2;
			if (location == anchorLocation) MarkAnchor(location);
			else MarkSelectable(location, direction);
			// We also need to keep track of the selectables for clearing them later.
			selectables.Add(location);
		}
		showingSelectables = true;
	}

	// CompleteSelection selects pieces on the board and indicate allowed moves on the movement buttons.
	public void CompleteSelection(Vector targetLocation, string direction)
	{
		// Ensure we unmark selectable pieces, since we will no longer need / want them.
		ClearSelectables();
		// Grab the column of pieces to be selected from the logic board.
		// We keep track of these so we can clear them later.
		selected = Board.GetColumn(anchorLocation, targetLocation, direction);
		// Indicate each piece selected.
		foreach (var location in selected)
		{
			Select(location);
		}
		showingSelected = true;

		// In case of singleton direction, selection is directionless.
		if (selected.Count == 1) direction = "";
		// Create a Selection object to pass to the logical board.
		var selection = new Selection(selected, direction, game.Board.GetSpace(selected[0]));
		// Now we grab the allowed moves and indicate these as well.
		// Need an index to access MoveButton objects.
		int i = 0;
		foreach (var move in game.Board.GetMoves(selection))
		{
			// If move is not null, it is valid.
			if (move != null) moveButtons[i].Enable(move);
			i++;
		}
	}
	
	public void ChangeFlipSetting()
	{
		FlipEveryTurn = !FlipEveryTurn;
		if (game.CurrentPlayer == 'W') Flip();
	}

	// Flip the board.
	public void Flip()
	{
		// We use a coroutine to provide a smooth rotation.
		StartCoroutine(FlipBoardCoroutine());
	}


	// Helper method for clearing selected pieces.
	public void ClearSelected()
	{
		showingSelected = false;	
		ClearPieces(selected);
	}

	// Helper method for marking the anchor.
	private void MarkAnchor(Vector location)
	{
		GetSpace(location).piece.MarkAnchor();
	}

	// Helper method for marking a selectable piece.
	private void MarkSelectable(Vector location, string direction)
	{
		GetSpace(location).piece.MarkSelectable(direction);
	}

	// Helper method for selecting a piece.
	private void Select(Vector location)
	{
		GetSpace(location).piece.Select();
	}
	
	// Helper method for getting Space object corresponding to a certain location on the board.
	private Space GetSpace(Vector location)
	{
		return boardDisplay[location.x][location.y];
	}

	// Helper method for clearing the selectable pieces.
	public void ClearSelectables()
	{
		showingSelectables = false;
		ClearPieces(selectables);
	}

	// Helper method for clearing (resetting) pieces.
	private void ClearPieces(IEnumerable<Vector> locations)
	{
		foreach (var location in locations)
		{
			GetSpace(location).piece.Clear();
		}
	}
	
	private IEnumerator FlipBoardCoroutine(int steps = 50)
	{
		// steps indicates how many steps it should take to complete the rotation.
		// angle will store the amount of degrees to be rotated each step.
		var angle = 180.0f / steps;
		for (int count = 0; count < steps; count++)
		{
			transform.Rotate(0, 0, angle);
			yield return null;
		}
	}

}
