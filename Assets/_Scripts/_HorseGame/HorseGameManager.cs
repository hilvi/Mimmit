using UnityEngine;
using System.Collections;

public class HorseGameManager : GameManager {
	enum Winner {Player, Bird, None}
	#region MEMBERS
	private Winner _winner = Winner.None;
	private bool _playerFinished = false;
	private bool _birdFinished = false;
	private bool _gameOver;
	private Rect _boxRect;
	private CharacterWidgetScript _characterWidget;
	
	public HorseCharacterController horseScript;
	public BirdScript birdScript;
	public AudioClip music;
	public GameObject musicObject;
	#endregion
	
	#region UNITY_METHODS
	public override void Start () 
	{
		base.Start ();
		if(InGameMenuGUI.music == null)
		{
		  	InGameMenuGUI.music = (GameObject)Instantiate(musicObject);
			InGameMenuGUI.music.audio.clip = music;
			InGameMenuGUI.music.audio.Play();
		}
		_characterWidget = GetComponent<CharacterWidgetScript>();
		SetGameState(GameState.Running);
		float __width = Screen.width / 2;
		float __height = Screen.height / 2;
		_boxRect = new Rect(Screen.width /2  - __width / 2, Screen.height / 2 - __height / 2, __width , __height);
		
		gameObject.AddComponent<GUIText>();
		gameObject.transform.position = new Vector3(0.5f, 0.5f, 0);
		guiText.font = (Font)Resources.Load("Fonts/Gretoon");
		guiText.fontSize = 60;
		guiText.alignment = TextAlignment.Center;
		guiText.anchor = TextAnchor.MiddleCenter;
	}
	
	void Update () 
	{
		if(!_gameOver)return;
		if(_winner == Winner.Player)
		{
			_characterWidget.TriggerHappyEmotion();
			SetGameState(GameState.Won);
		}
		else if(_winner == Winner.Bird)
		{
			_characterWidget.TriggerSadEmotion();
			SetGameState(GameState.Lost);
		}
	}
	
	void OnGUI()
	{
#if UNITY_EDITOR
		float fps  = 1/Time.deltaTime;
		GUI.Box (new Rect(0,0,100,50),fps.ToString());
#endif
		if(_gameOver)
		{
			if(_winner == Winner.Bird)
			{
				guiText.text = "Hävisit\nYritä uudelleen";
			}else{
				guiText.text = "Voitit";
			}
		}
	}
	#endregion 
	
	#region METHODS
	public void PlayerFinish()
	{
		_playerFinished = true;
		horseScript.SetSpeed(0);
		if(_birdFinished == false)_winner = Winner.Player;
		if(_birdFinished && _playerFinished)
		{
			_gameOver = true;
		}
	}
	
	public void BirdFinish()
	{
		_birdFinished = true;
		birdScript.SetSpeed(0);
		if(_playerFinished == false)_winner = Winner.Bird;
		if(_birdFinished && _playerFinished)
		{
			_gameOver = true;
		}
	}
	#endregion
}
