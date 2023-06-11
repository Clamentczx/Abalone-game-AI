/*	
		CurrentPlayer='W';

		test0=transfer_keys(AllSelection('W'));

		test1=transfer_values(AllSelection('W'));

		test=EnableMove(test0, test1);

		Random rand = new Random();
		
		int num;

		do{
			num =rand.Next(test0.Count); 
		
		}while(test[num].Count==0);
		
		Selection test3= new Selection(test0[num], test1[num][0], CurrentPlayer);
		
		int nun = rand.Next(test[num].Count);

		enemycolumn=EnemyCol(test0[num],test1[num][0],test[num][nun]);
	
		Move test4;
		
		if(SameAxis(test1[num][0], test[num][nun])==false){
			test4= new Move(test3, null, test[num][nun]);
		}
		else{
			test4= new Move(test3, enemycolumn, test[num][nun]);
		}

		
		int scoreDelta1 = Board.Move(test4); 

		Debug.Log("-------------");
		Debug.Log("operation index:"+num);
		Debug.Log("-------------");
		Debug.Log("direction:"+test[num][nun]);
		Debug.Log("-------------");
		Debug.Log("evaluation score:"+evaluation('W'));
		NextTurn(scoreDelta1);
		boardDisplay.UpdateView();
*/