
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class Board
{
	
	public static int height;

	public static List<int> rowLengths;

	public static IList<string> Directions { get; private set; }
	
	private static Dictionary<string, Vector> directionVectors;

	private List<List<char>> board;

	static Board()
	{
		height = 9;
		rowLengths = new List<int> {5, 6, 7, 8, 9, 8, 7, 6, 5};
		Directions = new List<string> {"NW", "NE", "E", "SE", "SW", "W"}.AsReadOnly();

		directionVectors = new Dictionary<string, Vector>
		{
			{"NW", new Vector(-1, -1)},
			{"NE", new Vector(-1,  0)},
			{"E",  new Vector(0,   1)},
			{"SE", new Vector(1,   1)},
			{"SW", new Vector(1,   0)},
			{"W",  new Vector(0,  -1)}
		};
	}

	public Board()
	{
		board = new List<List<char>>();
		for (int i = 0; i < height; i++)
		{
			var row = Enumerable.Repeat('O', rowLengths[i]).ToList();
			board.Add(row);
		}

		foreach (int i in new List<int> {0, 1, 2, 6, 7, 8})
		{
		
			var piece = (i < 3) ? 'W' : 'B';
			for (int j = 0; j < board[i].Count; j++)
			{
				if ((i == 2 || i == 6) && (j < 2 || j > 4))
				{
					continue;
				}
				board[i][j] = piece;
			}
		}
	}


	public IEnumerable<IEnumerable<char>> View()
	{
		return
			from row in board select
			from space in row select space;
	}

	
	public static List<Vector> GetColumn(Vector start, Vector end, string direction)
	{
		var column = new List<Vector>();
		var current = start;
		while (current != end)
		{
			column.Add(current);
			//the next marble position by this direction
			current = GetNeighborLocation(current, direction);
		}
		column.Add(current);
		return column;
	}


	public IEnumerable<Tuple<Vector, string>> GetSelectables(Vector anchorLocation)
	{

		var anchor = GetSpace(anchorLocation);

		yield return Tuple.Create(anchorLocation, "");

		foreach (var direction in Directions)
		{
			var current = anchorLocation;


			for (var distance = 1; distance < 3; distance++)
			{
				current = GetNeighborLocation(current, direction);

				if (!ValidLocation(current)) break;

				if (GetSpace(current) != anchor) break;

				yield return Tuple.Create(current, direction);
			}
		}
	}


	public IEnumerable<Move> GetMoves(Selection selection)
	{
		foreach (var direction in Directions)
		{
			var move = CheckMove(selection, direction);
			yield return move;
		}
	}


	public int Move(Move move)
	{
		if (move.EnemyColumn == null)
		{
			return MoveSideStep(move);
		}
		else
		{
			return MoveInLine(move);
		}
	}
	
	public int UndoMove(Move move)
	{
		if (move.EnemyColumn == null)
		{
			return UndoMoveSideStep(move);
		}
		else
		{
			return UndoMoveInLine(move);
		}
	}


	public char GetSpace(Vector loc)
	{
		return board[loc.x][loc.y];
	}


	private Move CheckMove(Selection selection, string direction)
	{

		if (SameAxis(selection.Direction, direction))
		{

			var enemyColor = (selection.Color == 'B') ? 'W' : 'B';
			var edgeIndex = selection.Column.Count - 1;
			var selectionEdge = (selection.Direction == direction) ? selection.Column[edgeIndex] : selection.Column[0];
			var enemyColumnStart = GetNeighborLocation(selectionEdge, direction);
			var enemyLimit = edgeIndex;
			var enemyColumn = new List<Vector>();
			if (Sumito(enemyColumnStart, direction, enemyColor, enemyLimit, enemyColumn))
			{
				return new Move(selection, enemyColumn, direction);
			}
		}


		foreach (var location in selection.Column)
		{
			var neighborLocation = GetNeighborLocation(location, direction);
			if (!ValidLocation(neighborLocation) || GetSpace(neighborLocation) != 'O') return null;
		}

		return new Move(selection, null, direction);
	}


	private bool Sumito(Vector start, string direction, char color, int bound, List<Vector> column)
	{
	
		var current = start;
	
		while (column.Count < bound)
		{
			
			if (!ValidLocation(current)) return column.Count > 0;
			var space = GetSpace(current);
			
			if (space == 'O') return true;
			
			if (space != color) return false;
			
			column.Add(current);
			current = GetNeighborLocation(current, direction);
		}
		
		return !ValidLocation(current) || GetSpace(current) == 'O';
	}

	
	private static bool SameAxis(string direction1, string direction2)
	{
		if (direction1 == direction2) return true;

		// Sort the strings to simplify the logic.
		if (string.Compare(direction1, direction2) > 0)
		{
			var temp = direction1; direction1 = direction2; direction2 = temp;
		}
		if (direction1 == "E" && direction2 == "W") return true;
		if (direction1 == "NW" && direction2 == "SE") return true;
		if (direction1 == "NE" && direction2 == "SW") return true;
		return false;
	}

	
	private int MoveSideStep(Move move)
	{
		foreach (var location in move.Selection.Column)
		{
			SetSpace(GetNeighborLocation(location, move.Direction), move.Selection.Color);
			SetSpace(location, 'O');
		}

		return 0;
	}



	
	private int MoveInLine(Move move)
	{

		var emptyIndex = (move.Direction == move.Selection.Direction) ? 0 : move.Selection.Column.Count - 1;
		SetSpace(move.Selection.Column[emptyIndex], 'O');

		int leadingIndex; Vector leadingTargetLocation;
		if (move.EnemyColumn.Count == 0)
		{
			leadingIndex = (emptyIndex == 0) ? move.Selection.Column.Count - 1 : 0;
			leadingTargetLocation = GetNeighborLocation(move.Selection.Column[leadingIndex], move.Direction);
			SetSpace(leadingTargetLocation, move.Selection.Color);
			return 0;
		}
		else
		{
			SetSpace(move.EnemyColumn[0], move.Selection.Color);
			leadingIndex = move.EnemyColumn.Count - 1;
			leadingTargetLocation = GetNeighborLocation(move.EnemyColumn[leadingIndex], move.Direction);
			if (!ValidLocation(leadingTargetLocation)) return 1;
			else
			{
				var enemyColor = (move.Selection.Color == 'B') ? 'W' : 'B';
				SetSpace(leadingTargetLocation, enemyColor);
				return 0;
			}
		}
	}

	private int UndoMoveSideStep(Move move)
	{

		foreach (var location in move.Selection.Column)
		{
			SetSpace(location, move.Selection.Color);
			SetSpace(GetNeighborLocation(location, move.Direction), 'O');
		}
		return 0;
	}

	
	private int UndoMoveInLine(Move move)
	{
		
		var emptyIndex = (move.Direction == move.Selection.Direction) ? 0 : move.Selection.Column.Count - 1;
		SetSpace(move.Selection.Column[emptyIndex], move.Selection.Color);
		
		int leadingIndex; Vector leadingTargetLocation;
		if (move.EnemyColumn.Count == 0)
		{
			leadingIndex = (emptyIndex == 0) ? move.Selection.Column.Count - 1 : 0;
			leadingTargetLocation = GetNeighborLocation(move.Selection.Column[leadingIndex], move.Direction);
			SetSpace(leadingTargetLocation, 'O');
			return 0;
		}
		
		else
		{
			var enemyColor = (move.Selection.Color == 'B') ? 'W' : 'B';
			SetSpace(move.EnemyColumn[0], enemyColor);
			leadingIndex = move.EnemyColumn.Count - 1;
			leadingTargetLocation = GetNeighborLocation(move.EnemyColumn[leadingIndex], move.Direction);
			
			if (!ValidLocation(leadingTargetLocation)) return -1;
			
			else
			{
				SetSpace(leadingTargetLocation, 'O');
				return 0;
			}
		}
	}


	private static Vector GetNeighborLocation(Vector location, string direction)
	{
		
		if (direction == "") return new Vector(-1, -1);
		
		var delta = directionVectors[direction];
	
		if (location.x == 4 && direction[0] == 'S')
		{
			delta.y--;
		}
		else if (location.x > 4 && direction.Length == 2)
		{
			delta.y += (direction[0] == 'N') ? 1 : -1;
		}
		return location + delta;
	}


	private static bool ValidLocation(Vector loc)
	{
		return 0 <= loc.x && loc.x < height && 0 <= loc.y && loc.y < rowLengths[loc.x];
	}


	private void SetSpace(Vector location, char space)
	{
		board[location.x][location.y] = space;
	}
}


public struct Vector
{
	public int x, y;

	public Vector(int x, int y)
	{
		this.x = x; this.y = y;
	}

	public static Vector operator +(Vector v1, Vector v2)
	{
		return new Vector(v1.x + v2.x, v1.y + v2.y);
	}


	public static bool operator ==(Vector v1, Vector v2)
	{
		return v1.x == v2.x && v1.y == v2.y;
	}

	public static bool operator !=(Vector v1, Vector v2)
	{
		return v1.x != v2.x || v1.y != v2.y;
	}

	public override bool Equals(object o)
	{
		var v = (Vector)o;
		return x == v.x && y == v.y;
	}

	public override int GetHashCode()
	{
		return x + y;
	}
}

// Struct for create a selection
public struct Selection
{
	//return the selection column
	public ReadOnlyCollection<Vector> Column { get { return column.AsReadOnly(); } }

	//the direction of selection
	public string Direction { get; private set; }
	
	//the selected marble's colour
	public char Color { get; private set; }

	//a list of selected marble's position
	private List<Vector> column;
        //constructor
	public Selection(List<Vector> column, string direction, char color)
	{
		this.column = column;
		Direction = direction;
		Color = color;
	}
}

// Struct for create a move
public class Move
{
	//define a selection class data type
	public Selection Selection { get; private set; }

	//return the enemy column
	public ReadOnlyCollection<Vector> EnemyColumn
	{
		get
		{
			if (enemyColumn == null) return null;
			else return enemyColumn.AsReadOnly();
		}
	}

	//the direction of move
	public string Direction { get; private set; }
	//a list of selected opponent marble's position
	private List<Vector> enemyColumn;
	//constructor
	public Move(Selection selection, List<Vector> enemyColumn, string direction)
	{
		Selection = selection;
		this.enemyColumn = enemyColumn;
		Direction = direction;
	}

}
