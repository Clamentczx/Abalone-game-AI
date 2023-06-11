// This class contains the game state, such as the current player, scores, etc.
using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random=System.Random;

public class Game : MonoBehaviour
{


	public Board Board { get; private set; }

	public BoardDisplay boardDisplay;
	
	public char CurrentPlayer { get; private set; }
	
	public bool GameOver { get; private set; }
	
	
	public string blackTurnMessage;
	public string whiteTurnMessage;
	public string blackWinsMessage;
	public string whiteWinsMessage;

	public List<string> directions=new List<string>{
			"NW", "NE", "E", "SE", "SW", "W"
	};
	public Dictionary<string,Vector> directionset=new Dictionary<string,Vector>{
		{"NW", new Vector(-1, -1)},
		{"NE", new Vector(-1,  0)},
		{"E",  new Vector(0,   1)},
		{"SE", new Vector(1,   1)},
		{"SW", new Vector(1,   0)},
		{"W",  new Vector(0,  -1)}
	};
	
	public Text gameStatus;

	private Dictionary<char, int> scores;
	
	private Dictionary<char, Text> displayedScores;



	
	void Awake()
	{
		// Create a board and initialize game state.
		Board = new Board();
		CurrentPlayer = 'B';
		GameOver = false;
		scores = new Dictionary<char, int> { {'B', 0}, {'W', 0} };

		// boardDisplay = GameObject.Find("BoardDisplay").GetComponent<BoardDisplay>();

		// Find text objects displaying scores and game status.
		var blackScoreText = GameObject.Find("BlackScore").GetComponent<Text>();
		var whiteScoreText = GameObject.Find("WhiteScore").GetComponent<Text>();
		displayedScores = new Dictionary<char, Text> { {'B', blackScoreText}, {'W', whiteScoreText} };
		// gameStatus = GameObject.Find("GameStatus").GetComponent<Text>();
	}

	public void ProcessMove(Move move)
	{	   		


		
		//player turn
		int scoreDelta =  Board.Move(move) ;

		NextTurn(scoreDelta);

		if (boardDisplay.showingSelectables) boardDisplay.ClearSelectables();

		else if (boardDisplay.showingSelected) boardDisplay.ClearSelected();

		boardDisplay.UpdateView();

		boardDisplay.DisableMoveButtons();

		//progress log

		
		int count=0;
		foreach(var sublist in transfer_keys(AllSelection('W'))){
			
			Debug.Log("-------------"+count);
			foreach(var content in sublist){
				Debug.Log(content.x+""+content.y);
			
			}
			count++;
		}
		int co=0;
		foreach(var sublist in EnableMove(transfer_keys(AllSelection('W')),transfer_values(AllSelection('W')))){
			Debug.Log("-------------"+co);
			foreach(var content in sublist){
				Debug.Log(content);
			
			}
			co++;
		}		
		

		//board view for white side
		/*
		int i = 0;		
		foreach (var row in Board.View())
		{
			int j = 0;
			foreach (var slot in row)
			{
				if (slot == 'W')
				{
					Debug.Log(i+"-"+j);	
				}
				j++;
			}
			i++;
		}
		*/
		//computer turn

		CurrentPlayer='W';

		List<List<string>> test=new List<List<string>>();

		List<List<Vector>> test0=new List<List<Vector>>();

		List<List<string>> test1=new List<List<string>>();

		List<Vector> enemycolumn=new List<Vector>();

		int scoreDelta1;	 

		int num=0;

		int maxeva=int.MinValue;

		test0=transfer_keys(AllSelection('W'));

		test1=transfer_values(AllSelection('W'));

		test=EnableMove(test0, test1);

		Move test5=null;

        	Move test4;			
		
		foreach(var element in test0){
			Debug.Log("-----------------1");
			if(test[num].Count>0){
				Debug.Log("-----------------2");
				Selection test3= new Selection(element, test1[num][0], CurrentPlayer);
				Debug.Log("-----------------3");
				foreach(var subdata in test[num]){
					Debug.Log("-----------------4");
					enemycolumn=EnemyCol(test0[num],test1[num][0],subdata);
					Debug.Log("-----------------5");
					if(SameAxis(test1[num][0], subdata)==false){
						test4= new Move(test3, null, subdata);
					}
					else{
						test4= new Move(test3, enemycolumn, subdata);
					}
					
					Debug.Log("-----------------");
					foreach(var check in test4.Selection.Column){
						Debug.Log("---"+check.x+"---"+check.y);
					}
					Debug.Log("-----------------");
					

					Board.Move(test4);
						
					if(maxeva<=evaluation(CurrentPlayer)){
						if(SameAxis(test1[num][0], subdata)==false){
							test5=new Move(test3, null, subdata);
						}	
						else{
							test5=new Move(test3, enemycolumn, subdata);
						}
						maxeva=evaluation(CurrentPlayer);
					}
					
					Board.UndoMove(test4);
					
					/*
					int i = 0;		
					foreach (var row in Board.View())
					{
						int j = 0;
						foreach (var slot in row)
						{
							if (slot == 'W')
							{
								Debug.Log(i+"-"+j);	
							}
							j++;
						}
						i++;
					}
					*/				
				}
			}	
			num++;
		}

		//diplay which action AI take
		Debug.Log("-----------------");
		foreach(var check in test5.Selection.Column){
			Debug.Log("---"+check.x+"---"+check.y);
		}
		Debug.Log("-----------------");
		
		Debug.Log("----"+test5.Direction);	
	
		scoreDelta1=Board.Move(test5);

		NextTurn(scoreDelta1);
		
		boardDisplay.UpdateView();
	       
	}

	
	private void NextTurn(int scoreDelta)
	{
	
		scores[CurrentPlayer] += scoreDelta;
		displayedScores[CurrentPlayer].text = ScoreMessage(CurrentPlayer);
	
		if (scores[CurrentPlayer] == 6)
		{
			GameOver = true;
			// Display victory message based on who won.
			gameStatus.text = (CurrentPlayer == 'B') ? blackWinsMessage : whiteWinsMessage;
		}
		
		else
		{
			if (GameOver) GameOver = false;
			gameStatus.text = (CurrentPlayer == 'B') ? whiteTurnMessage : blackTurnMessage;
		}
		CurrentPlayer =  'B';
	}

	// Restart game.
	public void Restart()
	{
		// Create new board, reset turn to black, and zero scores.
		Board = new Board();
		CurrentPlayer = 'B';
		gameStatus.text = blackTurnMessage;
		foreach (var player in "BW")
		{
			// Debug.Log(player);
			scores[player] = 0;
			displayedScores[player].text = ScoreMessage(player);
		}
		
		// Update display.
		boardDisplay.Restart();
	}





	// Helper function for creating player score message string.
	private string ScoreMessage(char player)
	{
		string prefix = (player == 'B') ? "Black: " : "White: ";
		return $"{prefix} {scores[player].ToString()}";
	}


	// check whether move inline or move side
	private bool SameAxis(string direction1, string direction2)
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


//decision part

	
	//abstract the all possible selected marble column 
	private List<Dictionary<Vector,String>> AllSelection(char player){
		List<Dictionary<Vector,String>> res=new List<Dictionary<Vector,String>>();
		Dictionary<Vector,String> col=new Dictionary<Vector,String>();
		


		int i = 0;		
		foreach (var row in Board.View())
		{
			int j = 0;
			foreach (var slot in row)
			{
				if (slot == player)
				{

					//single 		
					col.Add(new Vector(i,j),"");
					res.Add(new Dictionary<Vector,String>(col));
					col.Clear();
					foreach(var direction in directions)
					{	
						//double
						bool x=false;
						col.Add(new Vector(i,j),direction);
						if(ValidLocation(DirectionMove(col.ElementAt(0).Key, direction))==true && Board.GetSpace(DirectionMove(col.ElementAt(0).Key, direction))==player){
	col.Add(DirectionMove(col.ElementAt(0).Key, direction),direction);
	res.Add(new Dictionary<Vector,String>(col));
	x=true;
                                                }
						//triple
						if(x==true){
											if(ValidLocation(DirectionMove(col.ElementAt(1).Key, direction))==true && Board.GetSpace(DirectionMove(col.ElementAt(1).Key, direction))==player){
	col.Add(DirectionMove(col.ElementAt(1).Key, direction),direction);
	res.Add(new Dictionary<Vector,String>(col));
                                                                                        }
						}
						col.Clear();
					}
				}
				j++;
			}
			i++;
		}
		return res;
	}



	private bool ValidLocation(Vector loc){
		return 0 <= loc.x && loc.x < Board.height && 0 <= loc.y && loc.y < Board.rowLengths[loc.x];
	}

	private Vector DirectionMove(Vector location, string direction)
	{
		
		if (direction == "") return new Vector(-1, -1);
		
		var delta = directionset[direction];
	
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


	private List<List<Vector>> transfer_keys(List<Dictionary<Vector,String>> result){
		List<List<Vector>> ans=new List<List<Vector>>();
		List<Vector> vectorset;
		foreach(var content in result){
			vectorset = content.Keys.ToList();
			ans.Add(new List<Vector>(vectorset));
		}
		return ans;
	}

	private List<List<string>> transfer_values(List<Dictionary<Vector,String>> res){
		List<List<string>> ans=new List<List<string>>();
		List<string> vectorset;
		foreach(var content in res){
			vectorset = content.Values.ToList();
			ans.Add(new List<string>(vectorset));
		}
		return ans;
	}

	private List<List<string>> EnableMove(List<List<Vector>> vector, List<List<string>> select_dir){
		List<List<string>> ans= new List<List<string>>(); 
		int index=0;
		Vector leader=new Vector();
		List<string> moveset=new List<string>();
		foreach(var vectorcol in vector){
			string dir=select_dir[index][0];
			foreach(var direction in directions){
				//movestepside
				bool move_leader=false;
				bool valid=true;
				bool moveok=true;
				if(SameAxis(dir, direction)==false){
					foreach(var point in vectorcol){
						if(ValidLocation(DirectionMove(point, direction))!=true){ 
							valid=false;
							break;
						}
						else{
							if(Board.GetSpace(DirectionMove(point, direction))!='O'){
								moveok=false;
								break;
							}
						}
					}
					if(moveok==true && valid==true){moveset.Add(direction);}
	
				}
				else{
					if(dir==direction){
						leader = vectorcol[vectorcol.Count-1];
					}
					else{
						leader = vectorcol[0];
					}

					if(ValidLocation(DirectionMove(leader, direction))==true){ 
						List<Vector> enemycol=new List<Vector>();
						if(Board.GetSpace(DirectionMove(leader, direction))=='O'){move_leader=true;}
						if(Board.GetSpace(DirectionMove(leader, direction))=='B'){
								while(ValidLocation(DirectionMove(leader, direction))==true && Board.GetSpace(DirectionMove(leader, direction))=='B'){
									enemycol.Add(DirectionMove(leader, direction));
									leader=new Vector(DirectionMove(leader, direction).x,DirectionMove(leader, direction).y);
								}
								if(ValidLocation(DirectionMove(leader, direction))==true){
									if(vectorcol.Count>enemycol.Count && Board.GetSpace(leader)=='O'){
										move_leader=true;
									}
								}
								else{
									if(vectorcol.Count>enemycol.Count){
										move_leader=true;
									}
								}					
						}
					}
					if(move_leader==true){
						moveset.Add(direction);
					}	
				}
			}
			ans.Add(new List<string>(moveset));
			moveset.Clear();
			index++;
		}
		return ans;
	}
	
	private List<Vector> EnemyCol(List<Vector> col, string sel_direction, string mov_direction){
		List<Vector> ans=new List<Vector>();
		Vector leader=new Vector();
		if(SameAxis(sel_direction, mov_direction)==false){
			return ans;
		}
		else{
			if(sel_direction==mov_direction){
				leader = col[col.Count-1];
			}
			else{
				leader = col[0];
			}

			while(ValidLocation(DirectionMove(leader, mov_direction))==true && Board.GetSpace(DirectionMove(leader, mov_direction))=='B'){
				ans.Add(DirectionMove(leader, mov_direction));
				leader=new Vector(DirectionMove(leader, mov_direction).x,DirectionMove(leader, mov_direction).y);
			}
		}
		return ans;
	}


	




//evaluation part


	private int centre_distance(){
		Vector centre= new Vector(4,4);
		int i = 0;
 		int num=0;
		foreach (var row in Board.View())
		{
			int j = 0;
			foreach (var slot in row)
			{
				if (slot == 'W')
				{
					num=num+(Math.Abs(i-centre.x)+Math.Abs(j-centre.y))/2;
				}
				j++;
			}
			i++;
		}
		return num;
	}


	private int marbles(){
		int i = 0;
 		int num=0;
		foreach (var row in Board.View())
		{
			int j = 0;
			foreach (var slot in row)
			{
				if (slot == 'B')
				{
					num++;
				}
				j++;
			}
			i++;
		}
		return num;
	}

	private int evaluation(char player){
		return -(10*marbles()+centre_distance());
        } 
	

}




