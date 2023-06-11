

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NGamePiece : MonoBehaviour
{
	// The color of the piece ('B' or 'W')
	public char color;

	// three different status colour
	public Color normalColor;
	public Color selectableColor;
	public Color selectedColor;

	private NGame game;

	private NBoardDisplay boardDisplay;
	// The location of the piece on the board.
	private Vector location;

	private bool selectable;
	// The direction of the piece from the anchor, if it's selectable.
	private string selectableDirection;
	
	void Awake()
	{
	
		boardDisplay = GameObject.Find("BoardDisplay").GetComponent<NBoardDisplay>();
		
		game = GameObject.Find("Game").GetComponent<NGame>();

		location = transform.parent.GetComponent<NSpace>().Location;
	}

	// when a piece is clicked by mouse
	void OnMouseDown()
	{
		//make sure matching colour player operate matching colour marble 
		if (color != game.CurrentPlayer || game.GameOver) return;
		//if selection is completed 
		if (selectable)
		{
			boardDisplay.CompleteSelection(location, selectableDirection);
		}
		//just click once make this marble as anchor
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

	// Mark this piece as selectable
	public void MarkSelectable(string direction)
	{
		selectable = true;
		selectableDirection = direction;
		GetComponent<SpriteRenderer>().color = selectableColor;
	}

	// Mark this piece as selected
	public void Select()
	{
		GetComponent<SpriteRenderer>().color = selectedColor;
	}

	// clear the piece colour 
	public void Clear()
	{
		print("clearing a piece");
		selectable = false;
		GetComponent<SpriteRenderer>().color = normalColor;
	}
}
