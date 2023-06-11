/*
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
*/