// GamePiece is a MonoBehaviour class representing game pieces on the displayed board.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{

	public char color;


	public Color normalColor;
	public Color selectableColor;
	public Color selectedColor;


	private Game game;

	private BoardDisplay boardDisplay;

	private Vector location;

	private bool selectable;

	private string selectableDirection;
	
	void Awake()
	{

		boardDisplay = GameObject.Find("BoardDisplay").GetComponent<BoardDisplay>();
		game = GameObject.Find("Game").GetComponent<Game>();


		location = transform.parent.GetComponent<Space>().Location;
	}


	void OnMouseDown()
	{
		// If this piece does not belong to the current player, do nothing.
		if (color != game.CurrentPlayer || game.GameOver) return;

		// Otherwise, if this piece completes a selection, then do so.
		if (selectable)
		{
			boardDisplay.CompleteSelection(location, selectableDirection);
		}
		// If not, then we want to anchor using this piece.
		else
		{
			boardDisplay.Anchor(location);
		}
	}

	// Mark this piece as an achor.
	public void MarkAnchor()
	{
		selectable = true;
		GetComponent<SpriteRenderer>().color = selectedColor;
	}

	// Mark this piece as selectable. (This means it can be used to complete a selection.)
	public void MarkSelectable(string direction)
	{
		selectable = true;
		selectableDirection = direction;
		GetComponent<SpriteRenderer>().color = selectableColor;
	}

	// Mark this piece as selected.
	public void Select()
	{
		GetComponent<SpriteRenderer>().color = selectedColor;
	}

	public void Clear()
	{
		print("clearing a piece");
		selectable = false;
		GetComponent<SpriteRenderer>().color = normalColor;
	}
}
