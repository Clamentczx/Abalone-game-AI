// MoveButton is a MonoBehaviour class representing the buttons used by the human player(s) to make moves.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveButton : MonoBehaviour
{

	public float disabledAlpha;

	// The move to be made when the button is clicked. If null the button is off.
	private Move move;


	private Game game;	
	private BoardDisplay boardDisplay;

	void Awake()
	{
		game = GameObject.Find("Game").GetComponent<Game>();

	}


	void OnMouseDown()
	{
		// If button is enabled, then move
		if (move != null)
		{
			game.ProcessMove(move);
		}
	}

	// Enable the button to be pressed.
	public void Enable(Move move)
	{
		this.move = move;
		ChangeAlpha(1);
	}

	// Disable the button.
	public void Disable()
	{
		move = null;
		ChangeAlpha(disabledAlpha);
	}

	private void ChangeAlpha(float alpha)
	{
		var sprite = GetComponent<SpriteRenderer>();
		var color = sprite.color;
		color.a = alpha;
		sprite.color = color;
	}
}
