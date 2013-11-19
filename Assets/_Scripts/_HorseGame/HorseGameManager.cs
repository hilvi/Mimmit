using UnityEngine;
using System.Collections;

public class HorseGameManager : GameManager
{
	enum Winner
	{
		Player,
		Bird,
		None
	}

	#region MEMBERS
	public BirdScript birdScript;
	public AudioClip music;
	public GameObject musicObject;
	public float progressBarLength = 500;
	public Texture2D playerTexture;
	public Texture2D birdTexture;
	public float progressCharacterSize = 50;
	
	private CharacterWidgetScript _characterWidget;
	private HorseCharacterController _horseScript;
	private Winner _winner = Winner.None;
	private bool _playerFinished = false;
	private bool _birdFinished = false;
	private bool _gameOver;
	private Rect _progressBar;
	private Rect _progressPlayerPos;
	private Rect _progressBirdPos;
	private Transform _playerPosition;
	private Transform _birdPosition;
	private float _levelLength;
	public Texture2D _levelTexture;
	#endregion

	
	#region UNITY_METHODS
	public override void Start ()
	{
		base.Start ();
		if (InGameMenuGUI.music == null)
        {
			InGameMenuGUI.music = (GameObject)Instantiate (musicObject);
			InGameMenuGUI.music.audio.clip = music;
			InGameMenuGUI.music.audio.Play ();
		}
		_characterWidget = GetComponent<CharacterWidgetScript> ();
		SetGameState (GameState.Pregame);
		
		float __width = Screen.width / 2;
		Transform __finishPosition = GameObject.Find ("FinishLine").transform;
		_progressBar = new Rect (0, 0, progressBarLength, 25);
		_progressBar.center = new Vector2 (__width, 25);
		_playerPosition = GameObject.Find ("Player").transform;
		_birdPosition = GameObject.Find ("Bird").transform;
		
		_levelLength = __finishPosition.position.x - _playerPosition.position.x;
		_progressPlayerPos = new Rect (0, 0, progressCharacterSize, progressCharacterSize);
		_progressPlayerPos.center = new Vector2 (0, 25);
		_progressBirdPos = new Rect (0, 0, progressCharacterSize, progressCharacterSize);
		_progressBirdPos.center = new Vector2 (0, 25);
		
			
		gameObject.AddComponent<GUIText> ();
		gameObject.transform.position = new Vector3 (0.5f, 0.5f, 0);
		guiText.font = (Font)Resources.Load ("Fonts/Gretoon");
		guiText.fontSize = 60;
		guiText.alignment = TextAlignment.Center;
		guiText.anchor = TextAnchor.MiddleCenter;
		_horseScript = GameObject.Find ("Player").GetComponent<HorseCharacterController> ();
		
		StartCoroutine(_InitiateCountdown());
	}
	
	void Update ()
	{
		if (!_gameOver)
			return;
		if (_winner == Winner.Player) {
			_characterWidget.TriggerHappyEmotion ();
			SetGameState (GameState.Won);
		} else if (_winner == Winner.Bird) {
			_characterWidget.TriggerSadEmotion ();
			SetGameState (GameState.Lost);
		}
	}
	
	void OnGUI ()
	{	
		GUI.DrawTexture (_progressBar, _levelTexture);
		_progressBirdPos.x = _PositionToProgress (_birdPosition);
		GUI.DrawTexture (_progressBirdPos, birdTexture);
		_progressPlayerPos.x = _PositionToProgress (_playerPosition);
		GUI.DrawTexture (_progressPlayerPos, playerTexture);
		
		if (_gameOver) {
			if (_winner == Winner.Bird) {
				guiText.text = "Hävisit\nYritä uudelleen";
			} else {
				guiText.text = "Voitit";
			}
		}
	}
	#endregion 
	
	#region METHODS
	public void PlayerFinish ()
	{
		_playerFinished = true;
		_horseScript.SetSpeed (0);
		if (_birdFinished == false)
			_winner = Winner.Player;
		if (_birdFinished && _playerFinished) {
			_gameOver = true;
		}
	}
	
	public void BirdFinish ()
	{
		_birdFinished = true;
		birdScript.SetSpeed (0);
		if (_playerFinished == false)
			_winner = Winner.Bird;
		if (_birdFinished && _playerFinished) {
			_gameOver = true;
		}
	}
	
	private float _PositionToProgress (Transform pos)
	{
		float __currentPos = pos.transform.position.x;
		__currentPos = __currentPos / _levelLength * progressBarLength;
		return _progressBar.x + __currentPos - 15;
	}
	
	private IEnumerator _InitiateCountdown ()
	{
		CountdownManager __cdm = GetComponent<CountdownManager> ();
		__cdm.SetSpawnPosition(new Vector3(0f, 2f, -1f));
		
		while (!__cdm.CountdownDone) {
			yield return null;
		}
		
		base.SetGameState (GameState.Running);
		
		yield return null;
	}
	#endregion
}
