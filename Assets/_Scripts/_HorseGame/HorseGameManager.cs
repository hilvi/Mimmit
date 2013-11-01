using UnityEngine;
using System.Collections;

public class HorseGameManager : GameManager {
	enum Winner {Player, Bird, None}
	#region MEMBERS
	private Winner _winner = Winner.None;
	private bool _playerFinished = false;
	private bool _birdFinished = false;
	private bool _gameOver;
	private CharacterWidgetScript _characterWidget;
	
	private Rect _progressBar;
	private Rect _progressPlayerPos;
	private Rect _progressBirdPos;
	private Transform _playerPosition;
	private Transform _birdPosition;
	private float _levelLength;
	private Texture2D _levelTexture;
	
	public HorseCharacterController horseScript;
	public BirdScript birdScript;
	public AudioClip music;
	public GameObject musicObject;
	public float progressBarLength = 500;
	public Texture2D playerTexture;
	public Texture2D birdTexture;
	public float progressCharacterSize = 50;
	#endregion
	
	private float PositionToProgress(Transform pos) {
		float __currentPos = pos.transform.position.x;
		__currentPos = __currentPos / _levelLength * progressBarLength;
		return _progressBar.x+__currentPos-15;
	}
	
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
		Transform __finishPosition = GameObject.Find ("FinishLine").transform;
		_progressBar = new Rect(0,0, progressBarLength, 25);
		_progressBar.center = new Vector2(__width, 25);
		_playerPosition = GameObject.Find("Player").transform;
		_birdPosition = GameObject.Find ("Bird").transform;
		
		_levelLength = __finishPosition.position.x - _playerPosition.position.x;
		_progressPlayerPos = new Rect(0, 0, progressCharacterSize, progressCharacterSize);
		_progressPlayerPos.center = new Vector2(0,25);
		_progressBirdPos = new Rect(0, 0, progressCharacterSize, progressCharacterSize);
		_progressBirdPos.center = new Vector2(0,25);
		
		_levelTexture = GameObject.Find ("Background").GetComponentInChildren<BackgroundManager>().background;
		
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
		
		GUI.DrawTexture(_progressBar, _levelTexture);
		_progressBirdPos.x = PositionToProgress(_birdPosition);
		GUI.DrawTexture(_progressBirdPos, birdTexture);
		_progressPlayerPos.x = PositionToProgress(_playerPosition);
		GUI.DrawTexture(_progressPlayerPos, playerTexture);
		
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
