// This class contains the game state, such as the current player, scores, etc.
using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random=System.Random;

public class AGame : MonoBehaviour
{
	// Board references instance of logical representation of game board.
	// Cannot be set publicly. 
	public Board Board { get; private set; }

	// Reference to the displayed board.
	public ABoardDisplay boardDisplay;

	// Players are represented by the chars 'B' and 'W'.
	// CurrentPlayer references player whose turn it is. Cannot be set publicly.
	public char CurrentPlayer { get; private set; }

	// Is the game over?
	public bool GameOver { get; private set; }
	
	public int acc;

	// Common strings used to display information to user stored for convenience.
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



	// Reference to text object displaying current status of game (current turn / game over).
	public Text gameStatus;

	// scores['B'] and scores['W'] store black and white scores, respectively.
	private Dictionary<char, int> scores;

	// displayedScores['B'/'W'] store references to text objects displaying black / white scores.
	private Dictionary<char, Text> displayedScores;

	
	void Awake()
	{
		// Create a board and initialize game state.
		Board = new Board();
		CurrentPlayer = 'B';
		GameOver = false;
		scores = new Dictionary<char, int> { {'B', 0}, {'W', 0} };

		// boardDisplay = GameObject.Find("BoardDisplay").GetComponent<ABoardDisplay>();

		// Find text objects displaying scores and game status.
		var blackScoreText = GameObject.Find("BlackScore").GetComponent<Text>();
		var whiteScoreText = GameObject.Find("WhiteScore").GetComponent<Text>();
		displayedScores = new Dictionary<char, Text> { {'B', blackScoreText}, {'W', whiteScoreText} };
		// gameStatus = GameObject.Find("GameStatus").GetComponent<Text>();
	}

	public void ProcessMove()
	{	   	

		int count=0;
		foreach(var sublist in transfer_keys(AllSelection('B'))){
			
			Debug.Log("-------------"+count);
			foreach(var content in sublist){
				Debug.Log(content.x+""+content.y);
			
			}
			count++;
		}
		int co=0;
		foreach(var sublist in EnableMove(transfer_keys(AllSelection('B')),transfer_values(AllSelection('B')),'B')){
			Debug.Log("-------------"+co);
			foreach(var content in sublist){
				Debug.Log(content);
			
			}
			co++;
		}
		
	
		
		//black computer turn
		
		//RANDOM
		CurrentPlayer='B';
		List<List<string>> action=new List<List<string>>();
		List<List<Vector>> action0=new List<List<Vector>>();
		List<List<string>> action1=new List<List<string>>();
		List<Vector> enemycolumn2=new List<Vector>();

		action0=transfer_keys(AllSelection('B'));

		action1=transfer_values(AllSelection('B'));

		action=EnableMove(action0, action1, 'B');

		Random rand1 = new Random();
		
		int num1;

		do{
			num1 =rand1.Next(action0.Count); 
		
		}while(action[num1].Count==0);
		
		Selection action3= new Selection(action0[num1], action1[num1][0], CurrentPlayer);
		
		int nun1 = rand1.Next(action[num1].Count);

		enemycolumn2=EnemyCol(action0[num1],action1[num1][0],action[num1][nun1],'W');
	
		Move action4;
		
		if(SameAxis(action1[num1][0], action[num1][nun1])==false){
			action4= new Move(action3, null, action[num1][nun1]);
		}
		else{
			action4= new Move(action3, enemycolumn2, action[num1][nun1]);
		}
		
		int scoreDelta = Board.Move(action4); 

		NextTurn(scoreDelta);
		boardDisplay.UpdateView();
		

	
		/*
		//maxai
		CurrentPlayer='B';

		List<List<string>> action=new List<List<string>>();

		List<List<Vector>> action0=new List<List<Vector>>();

		List<List<string>> action1=new List<List<string>>();

		List<Vector> enemycolumn1=new List<Vector>();

		int scoreDelta;	 

		int num1=0;

		int maxeva1=int.MinValue;

		action0=transfer_keys(AllSelection('B'));

		action1=transfer_values(AllSelection('B'));

		action=EnableMove(action0, action1, CurrentPlayer);

		Move action5=null;

        	Move action4;			
		
		foreach(var element in action0){
			
			if(action[num1].Count>0){
				
				Selection action3= new Selection(element, action1[num1][0], CurrentPlayer);
				
				foreach(var subdata in action[num1]){
					
					enemycolumn1=EnemyCol(action0[num1],action1[num1][0],subdata,'W');
					
					if(SameAxis(action1[num1][0], subdata)==false){
						action4= new Move(action3, null, subdata);
					}
					else{
						action4= new Move(action3, enemycolumn1, subdata);
					}
					
					Debug.Log("-----------------");
					foreach(var check in action4.Selection.Column){
						Debug.Log("---"+check.x+"---"+check.y);
					}
					Debug.Log("-----------------");
					

					Board.Move(action4);
						
					if(maxeva1<=evaluation(CurrentPlayer)){
						if(SameAxis(action1[num1][0], subdata)==false){
							action5=new Move(action3, null, subdata);
						}	
						else{
							action5=new Move(action3, enemycolumn1, subdata);
						}
						maxeva1=evaluation(CurrentPlayer);
					}
					
					Board.UndoMove(action4);
					
					
					int i = 0;		
					foreach (var row in Board.View())
					{
						int j = 0;
						foreach (var slot in row)
						{
							if (slot == 'B')
							{
								Debug.Log(i+"-"+j);	
							}
							j++;
						}
						i++;
					}
									
				}
			}	
			num1++;
		}

		//diplay which action AI take
		Debug.Log("-----------------");
		foreach(var check in action5.Selection.Column){
			Debug.Log("---"+check.x+"---"+check.y);
		}
		Debug.Log("-----------------");
		
		Debug.Log("----"+action5.Direction);	
	
		scoreDelta=Board.Move(action5);

		NextTurn(scoreDelta);
		
		boardDisplay.UpdateView();

		*/




		
		//white computer turn
		/*
		//RANDOM
		CurrentPlayer='W';
		
		List<List<string>> test=new List<List<string>>();
		List<List<Vector>> test0=new List<List<Vector>>();
		List<List<string>> test1=new List<List<string>>();
		List<Vector> enemycolumn=new List<Vector>();

		test0=transfer_keys(AllSelection('W'));

		test1=transfer_values(AllSelection('W'));

		test=EnableMove(test0, test1, 'W');

		Random rand = new Random();
		
		int num;

		do{
			num =rand.Next(test0.Count); 
		
		}while(test[num].Count==0);
		
		Selection test3= new Selection(test0[num], test1[num][0], CurrentPlayer);
		
		int nun = rand.Next(test[num].Count);

		enemycolumn=EnemyCol(test0[num],test1[num][0],test[num][nun],'B');
	
		Move test4;
		
		if(SameAxis(test1[num][0], test[num][nun])==false){
			test4= new Move(test3, null, test[num][nun]);
		}
		else{
			test4= new Move(test3, enemycolumn, test[num][nun]);
		}

		
		int scoreDelta1 = Board.Move(test4); 

		NextTurn(scoreDelta1);
		boardDisplay.UpdateView();
		*/
		

		/*
		//maxai
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

		test=EnableMove(test0, test1, CurrentPlayer);

		Move test5=null;

        	Move test4;			
		
		foreach(var element in test0){
			
			if(test[num].Count>0){
				
				Selection test3= new Selection(element, test1[num][0], CurrentPlayer);
				
				foreach(var subdata in test[num]){
					
					enemycolumn=EnemyCol(test0[num],test1[num][0],subdata,'B');
					
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
						maxeva=evaluation1(CurrentPlayer);
					}
					
					Board.UndoMove(test4);
					
					
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
		*/
		
		
		//minimax    		

		CurrentPlayer='W';

		List<List<string>> test=new List<List<string>>();

		List<List<Vector>> test0=new List<List<Vector>>();

		List<List<string>> test1=new List<List<string>>();

		List<Vector> enemycolumn=new List<Vector>();

		int scoreDelta1;	 

		int num=0;

		int imaxeva=int.MaxValue;

		test0=transfer_keys(AllSelection('W'));

		test1=transfer_values(AllSelection('W'));

		test=EnableMove(test0, test1,'W');

        	Move test4;

		Move test5=null;			
		
		foreach(var element in test0){
			
			if(test[num].Count>0){
				
				Selection test3= new Selection(element, test1[num][0], CurrentPlayer);
				
				foreach(var subdata in test[num]){
					
					enemycolumn=EnemyCol(test0[num],test1[num][0],subdata,'B');
					
					if(SameAxis(test1[num][0], subdata)==false){
						test4= new Move(test3, null, subdata);
					}
					else{
						test4= new Move(test3, enemycolumn, subdata);
					}

					Board.Move(test4);	 

					int insidenum=0;
					
					List<List<string>> other=new List<List<string>>();

					List<List<Vector>> other0=new List<List<Vector>>();

					List<List<string>> other1=new List<List<string>>();

					List<Vector> enemycolumn_other=new List<Vector>();
		
					Move other4;					

					other0=transfer_keys(AllSelection('B'));

					other1=transfer_values(AllSelection('B'));

					other=EnableMove(other0, other1,'B');
				
					int imineva=int.MaxValue;
	
					foreach(var insidelement in other0){
						if(other[insidenum].Count>0){
							Selection other3= new Selection(insidelement, other1[insidenum][0], 'B');	
							foreach(var insidesubdata in other[insidenum]){
				
								enemycolumn_other=EnemyCol(other0[insidenum],other1[insidenum][0],insidesubdata,'W');
					
								if(SameAxis(other1[insidenum][0], insidesubdata)==false){
									other4= new Move(other3, null, insidesubdata);
								}
								else{
									other4= new Move(other3, enemycolumn_other, insidesubdata);
								}
								Debug.Log("-----------");
								foreach(var list in other4.Selection.Column){
									Debug.Log("-----"+list.x+"----"+list.y);
								}
								Debug.Log(other4.Direction);
								Debug.Log("-----------");

								Board.Move(other4);
								
					
								if(imineva>=evaluation('B')){
									imineva=evaluation('B');
								}
								Board.UndoMove(other4);
							}
						}
						insidenum++;
					}
					
					if(imaxeva>=imineva){
						if(SameAxis(test1[num][0], subdata)==false){
							test5= new Move(test3, null, subdata);
						}
						else{
							test5= new Move(test3, enemycolumn, subdata);
						}
					}
					Board.UndoMove(test4);
									
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
		// Update scores.
		scores[CurrentPlayer] += scoreDelta;
		displayedScores[CurrentPlayer].text = ScoreMessage(CurrentPlayer);
		// End game if current player's score is now 6.
		if (scores[CurrentPlayer] == 6)
		{
			GameOver = true;
			// Display victory message based on who won.
			gameStatus.text = (CurrentPlayer == 'B') ? blackWinsMessage : whiteWinsMessage;
		}
		// Otherwise indicate the next turn.

		// In that case game is no longer over.
		else
		{
			if (GameOver) GameOver = false;
			gameStatus.text = (CurrentPlayer == 'B') ? whiteTurnMessage : blackTurnMessage;
		}
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





	
	private string ScoreMessage(char player)
	{
		string prefix = (player == 'B') ? "Black: " : "White: ";
		return $"{prefix} {scores[player].ToString()}";
	}


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














//Searching part

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

	private List<List<string>> EnableMove(List<List<Vector>> vector, List<List<string>> select_dir, char player ){
		char enemyz;
		if(player=='W'){
			enemyz='B';
		}
		else{
			enemyz='W';
		}
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
					if(moveok==true && valid==true){
						moveset.Add(direction);
					}
	
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
						if(Board.GetSpace(DirectionMove(leader, direction))=='O'){
								move_leader=true;
						}
						if(Board.GetSpace(DirectionMove(leader, direction))==enemyz){
								while(ValidLocation(DirectionMove(leader, direction))==true && Board.GetSpace(DirectionMove(leader, direction))==enemyz){
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
	
	private List<Vector> EnemyCol(List<Vector> col, string sel_direction, string mov_direction, char player){
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

			while(ValidLocation(DirectionMove(leader, mov_direction))==true && Board.GetSpace(DirectionMove(leader, mov_direction))==player){
			ans.Add(DirectionMove(leader, mov_direction));
			leader=new Vector(DirectionMove(leader, mov_direction).x,DirectionMove(leader, mov_direction).y);
			}
		}
		return ans;
	}


	




//evaluation part


	private int centre_distance(char player){
		Vector centre= new Vector(4,4);
		int i = 0;
 		int num=0;
		foreach (var row in Board.View())
		{
			int j = 0;
			foreach (var slot in row)
			{
				if (slot == player)
				{
					num=num+(Math.Abs(i-centre.x)+Math.Abs(j-centre.y))/2;
				}
				j++;
			}
			i++;
		}
		return num;
	}


	private int marbles(char enemy){
		int i = 0;
 		int num=0;
		foreach (var row in Board.View())
		{
			int j = 0;
			foreach (var slot in row)
			{
				if (slot == enemy)
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
		char enemy;
		if(player =='W'){
			enemy='B';
		}
		else{
			enemy='W';
		}
		return -(10*marbles(enemy)+centre_distance(player));
        } 


	private int evaluation1(char player){
		char enemy;
		if(player =='W'){
			enemy='B';
		}
		else{
			enemy='W';
		}
		return -(10*marbles(enemy)+centre_distance(player));
        } 




}
