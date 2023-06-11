// GamePiece is a MonoBehaviour class representing game pieces on the displayed board.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGamePiece : MonoBehaviour
{
	// The color of the piece ('B' or 'W').
	public char color;

	// The graphical colors used to represent the state of a piece.
	public Color normalColor;
	public Color selectableColor;
	public Color selectedColor;

	// Reference to the game object.
	private AGame game;
	// Reference to the displayed board.
	private ABoardDisplay boardDisplay;
	// The location of the piece on the board.
	private Vector location;
	// Boolean flag indicating whether or not this piece completes a selection.
	private bool selectable;
	// The direction of the piece from the anchor, if it's selectable.
	private string selectableDirection;
	
	void Awake()
	{
		// Grab references to the game and the displayed board.
		boardDisplay = GameObject.Find("BoardDisplay").GetComponent<ABoardDisplay>();
		game = GameObject.Find("Game").GetComponent<AGame>();

		// Grab the location of the piece from the parent space.
		location = transform.parent.GetComponent<ASpace>().Location;
	}

	// Anything that happens when a piece is clicked is handled here.
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
