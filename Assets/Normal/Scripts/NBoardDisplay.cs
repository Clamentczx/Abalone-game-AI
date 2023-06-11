

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NBoardDisplay : MonoBehaviour
{

	public Transform spacePrefab;


	public float paddingFactor = 1.1f;


	public bool FlipEveryTurn { get; private set; }


	private List<NMoveButton> moveButtons;


	private NGame game;


	private List<List<NSpace>> boardDisplay;


	private Vector anchorLocation;


	private List<Vector> selectables;
	
	private List<Vector> selected;

 
	public bool showingSelectables;
	
	public bool showingSelected;

	void Awake()
	{

		game = GameObject.Find("Game").GetComponent<NGame>();

		moveButtons = new List<NMoveButton>();
		
		foreach (var direction in Board.Directions)
		{
			var moveButton = GameObject.Find("Move" + direction).GetComponent<NMoveButton>();
			moveButtons.Add(moveButton);
		}

		showingSelectables = showingSelected = FlipEveryTurn = false;
		
		boardDisplay = new List<List<NSpace>>();
		
		var spaceDiameter = spacePrefab.localScale.x * transform.localScale.x * paddingFactor;
		
		var spaceRadius = spaceDiameter / 2;
		
		var boardRadius = Board.height / 2;
		
		var xOffset = boardRadius * spaceDiameter;
		
		for (int i = 0; i < Board.height; i++)
		{
			
			var length = Board.rowLengths[i];
			
			var row = new List<NSpace>();
			
			var x = (Board.height - length) * spaceRadius  - xOffset;
			
			var y = (boardRadius - i) * spaceDiameter;
			
			for (int j = 0; j < length; j++)
			{
				
				var position = new Vector3(x, y, 0);
				var space = Instantiate(spacePrefab, position, Quaternion.identity, transform).GetComponent<NSpace>();
				
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

		UpdateView();
		DisableMoveButtons();
		
		if ((int)transform.eulerAngles.z % 360 == 270) Flip();
	}
	
	
	public void DisableMoveButtons()
	{
		foreach (var button in moveButtons)
		{
			button.Disable();
		}
	}

	
	public void Anchor(Vector anchorLocation)
	{	
		
		if (showingSelectables) ClearSelectables();
		else if (showingSelected) ClearSelected();
		this.anchorLocation = anchorLocation;
		selectables = new List<Vector>();
		

		foreach (var pair in game.Board.GetSelectables(anchorLocation))
		{
			var location = pair.Item1; var direction = pair.Item2;
			if (location == anchorLocation) MarkAnchor(location);
			else MarkSelectable(location, direction);
			
			selectables.Add(location);
		}
		showingSelectables = true;
	}

	//after complete selection check which direction is valid
	public void CompleteSelection(Vector targetLocation, string direction)
	{
		
		ClearSelectables();
		
		selected = Board.GetColumn(anchorLocation, targetLocation, direction);
		
		foreach (var location in selected)
		{
			Select(location);
		}
		showingSelected = true;

		
		if (selected.Count == 1) direction = "";
		
		var selection = new Selection(selected, direction, game.Board.GetSpace(selected[0]));
		
		int i = 0;
		foreach (var move in game.Board.GetMoves(selection))
		{
			
			if (move != null) moveButtons[i].Enable(move);
			i++;
		}
	}


	public void Flip()
	{
		
		StartCoroutine(FlipBoardCoroutine());
	}

	
	public void ClearSelected()
	{
		showingSelected = false;	
		ClearPieces(selected);
	}

	
	private void MarkAnchor(Vector location)
	{
		GetSpace(location).piece.MarkAnchor();
	}

	
	private void MarkSelectable(Vector location, string direction)
	{
		GetSpace(location).piece.MarkSelectable(direction);
	}

	
	private void Select(Vector location)
	{
		GetSpace(location).piece.Select();
	}
	
	
	private NSpace GetSpace(Vector location)
	{
		return boardDisplay[location.x][location.y];
	}

	
	public void ClearSelectables()
	{
		showingSelectables = false;
		ClearPieces(selectables);
	}

	
	private void ClearPieces(IEnumerable<Vector> locations)
	{
		foreach (var location in locations)
		{
			GetSpace(location).piece.Clear();
		}
	}

	private IEnumerator FlipBoardCoroutine(int steps = 60)
	{
		
		var angle = 180.0f / steps;
		for (int count = 0; count < steps; count++)
		{
			transform.Rotate(0, 0, angle);
			yield return null;
		}
	}

	
}
