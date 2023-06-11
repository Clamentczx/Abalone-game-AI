

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NMoveButton : MonoBehaviour
{

	public float disabledAlpha;

	private Move move;

	private NGame game;	
	
	private NBoardDisplay boardDisplay;

	void Awake()
	{
		game = GameObject.Find("Game").GetComponent<NGame>();
	}

	
	void OnMouseDown()
	{
		// If the button is on then take the move.
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

	// change alpha to change the compare colour between enable and disable 
	private void ChangeAlpha(float alpha)
	{
		var sprite = GetComponent<SpriteRenderer>();
		var color = sprite.color;
		color.a = alpha;
		sprite.color = color;
	}
}
