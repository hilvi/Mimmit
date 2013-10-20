using UnityEngine;
using System.Collections;

public class FinishScript : MonoBehaviour {
	enum Winner {Player, Bird, None}
	#region MEMBERS
	private Winner _winner = Winner.None;
	private bool _playerFinished = false;
	private bool _birdFinished = false;
	private Rect _boxRect;
	public GameManager manager;
	public HorseCharacterController horseScript;
	public BirdScript birdScript;
	private bool _gameOver;
	#endregion
	
	#region UNITY_METHODS
	
	void Start () 
	{
		float __width = Screen.width / 2;
		float __height = Screen.height / 2;
		_boxRect = new Rect(Screen.width /2  - __width / 2, Screen.height / 2 - __height / 2, __width , __height);
	}
	void Update(){
		print(manager.GetGameState());
	}
	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.name == "Player")
		{
			_playerFinished = true;
			horseScript.SetSpeed(0);
			if(_birdFinished == false)_winner = Winner.Player;
		}
		if(col.gameObject.name == "Bird")
		{
			_birdFinished = true;
			birdScript.SetSpeed(0);
			if(_playerFinished == false)_winner = Winner.Bird;
		}
		if(_birdFinished && _playerFinished) _gameOver = true;//manager.SetGameState(GameState.Over);
		
	}
	void OnGUI()
	{
		if(_gameOver)
		{
			if(_winner == Winner.Bird)
			{
				GUI.Box (_boxRect, "Sorry, try again");
			}else{
				GUI.Box (_boxRect, "Youhouuuu you won!!!");
			}
		}
	}
	#endregion
	
	#region METHODS
	#endregion
}
