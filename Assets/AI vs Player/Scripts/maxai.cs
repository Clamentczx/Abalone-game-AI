/*
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
			if(test[num].Count>0){
				Selection test3= new Selection(element, test1[num][0], CurrentPlayer);
				foreach(var subdata in test[num]){
					enemycolumn=EnemyCol(test0[num],test1[num][0],subdata);
					if(SameAxis(test1[num][0], subdata)==false){
						test4= new Move(test3, null, subdata);
					}
					else{
						test4= new Move(test3, enemycolumn, subdata);
					}
					
					foreach(var check in test4.Selection.Column){
						Debug.Log("---"+check.x+"---"+check.y);
					}
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




