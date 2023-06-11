
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NGame : MonoBehaviour
{

	public Board Board { get; private set; }

	public NBoardDisplay boardDisplay;

	public char CurrentPlayer { get; private set; }

	public bool GameOver { get; private set; }
	
	public string blackTurnMessage;
	
	public string whiteTurnMessage;
	
	public string blackWinsMessage;
	
	public string whiteWinsMessage;

	public Button undoButton;
	
	public Button redoButton;

	public Text gameStatus;

	private Dictionary<char, int> scores;

	private Dictionary<char, Text> displayedScores;

	private LinkedList<Move> moveHistory;

	private LinkedListNode<Move> moveHistoryPointer;

	void Awake()
	{
		Board = new Board();
		
		CurrentPlayer = 'B';
		
		GameOver = false;
		
		scores = new Dictionary<char, int> { {'B', 0}, {'W', 0} };
		
		moveHistory = new LinkedList<Move>();
		
		moveHistoryPointer = moveHistory.Last;
		
		boardDisplay = GameObject.Find("BoardDisplay").GetComponent<NBoardDisplay>();

		var blackScoreText = GameObject.Find("BlackScore").GetComponent<Text>();
		
		var whiteScoreText = GameObject.Find("WhiteScore").GetComponent<Text>();
		
		displayedScores = new Dictionary<char, Text> { {'B', blackScoreText}, {'W', whiteScoreText} };
	}

	public void ProcessMove(Move move, bool newMove = true, bool undoMove = false)
	{
		// check whether it is undo move
		int scoreDelta = (!undoMove) ? Board.Move(move) : Board.UndoMove(move);
		
		NextTurn(scoreDelta);
		
		// If this is a new move add it to the history
		while (moveHistory.Last != moveHistoryPointer) moveHistory.RemoveLast();
		
		moveHistory.AddLast(move);
		
		moveHistoryPointer = moveHistory.Last;
		
		//undo button display enable use
		undoButton.interactable = true; redoButton.interactable = false;

		if (boardDisplay.showingSelectables)
		{ 
			boardDisplay.ClearSelectables();
		}
		else if (boardDisplay.showingSelected){ 
			boardDisplay.ClearSelected();
		}
		
		boardDisplay.UpdateView();
		
		boardDisplay.DisableMoveButtons();
		
		
	}

	// Update game state based on scoreDelta
	private void NextTurn(int scoreDelta)
	{
		// Update scores.
		
		scores[CurrentPlayer] += scoreDelta;
		
		displayedScores[CurrentPlayer].text = ScoreMessage(CurrentPlayer);
		// if any player score= 6 then game over
		if (scores[CurrentPlayer] == 6)
		{
			GameOver = true;
			// display victory message to winner
			gameStatus.text = (CurrentPlayer == 'B') ? blackWinsMessage : whiteWinsMessage;
		}
		//special case for undo a winning status
		else
		{
			if (GameOver) GameOver = false;
			gameStatus.text = (CurrentPlayer == 'B') ? whiteTurnMessage : blackTurnMessage;
		}
		CurrentPlayer = (CurrentPlayer == 'B') ? 'W' : 'B';
	}

	// Restart game.
	public void Restart()
	{
		// Create new board
		Board = new Board();
		CurrentPlayer = 'B';
		gameStatus.text = blackTurnMessage;
		foreach (var player in "BW")
		{
			scores[player] = 0;
			displayedScores[player].text = ScoreMessage(player);
		}
		
		// Reset move history.
		moveHistory = new LinkedList<Move>();
		moveHistoryPointer = moveHistory.Last;
		undoButton.interactable = redoButton.interactable = false;
		
		// Update display.
		boardDisplay.Restart();
	}

	// Undo a move.
	public void Undo()
	{
		ProcessMove(moveHistoryPointer.Value, newMove: false, undoMove: true);
		moveHistoryPointer = moveHistoryPointer.Previous;
		if (moveHistoryPointer == null) undoButton.interactable = false;
		redoButton.interactable = true;
	}

	// Redo a move.
	public void Redo()
	{
		ProcessMove(moveHistoryPointer.Next.Value, newMove: false);
		moveHistoryPointer = moveHistoryPointer.Next;
		if (moveHistoryPointer == moveHistory.Last) redoButton.interactable = false;
		undoButton.interactable = true;
	}

	// helper function for creating player score message string.
	private string ScoreMessage(char player)
	{
		string prefix = (player == 'B') ? "Black: " : "White: ";
		return $"{prefix} {scores[player].ToString()}";
	}
}
