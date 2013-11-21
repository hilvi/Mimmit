using UnityEngine;
using System.Collections;

/// <summary>
/// In game menu GUI.
/// Attached to the game manager object
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class InGameMenuGUI : Overlay
{	
	#region MEMBERS
	public Texture Restart, PlayButton, MainMenuButton, PauseButton;
	public static GameObject music;
	public MovieTexture tutorial;
	public Texture2D frame;
	private GameManager _gameManager;
	private Rect _tutorialButtonRegion;
	private Rect _pauseButtonRegion;
	private Rect _mainMenuButtonRegion;
	private Rect _restartButtonRegion;
	private Rect _nextLevelButtonRegion;
	private Rect _tutorialRegion;
	private Rect _tutorialFrame;
	private GUIStyle noStyle = new GUIStyle();
	private GameState _previousState;
	#endregion
	
	#region UNITY_METHODS
	void Start ()
	{
		FadeIn ();
		_gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		float __width = Screen.width;
		float __height = Screen.height;
		float __sizeButton = __width / 15;
		float __margin = 5f;
		_tutorialButtonRegion = new Rect(__width - __sizeButton * 2f - __margin * 2f,__margin,
		                                 __sizeButton, __sizeButton);

		_pauseButtonRegion = new Rect (
            __width - __sizeButton - __margin, __margin,
            __sizeButton, __sizeButton);


		float __widthA = __width / 6;
		float __widthB = __width / 7;

		_mainMenuButtonRegion = new Rect (
			MGUI.margin * 3, 
			__height - (__widthA), 
			__widthB, 
			__widthB);
		
		_restartButtonRegion = new Rect (
			__width - (__width / 2 + __width / 14),
			__height - (__widthA), 
			__widthB, 
			__widthB);
		
		_nextLevelButtonRegion = new Rect (
			__width - (__width / 3 - __width / 7),
			__height - (__widthA), 
			__widthB, 
			__widthB);

		float __tutWidth = Screen.width / 3;
		float __tutHeight = Screen.height / 2;
		_tutorialRegion = new Rect(__width / 2 - __tutWidth / 2, 
		                           __height / 2 - __tutHeight / 2,
		                           __tutWidth, __tutHeight-10
			);
		float __offside = 60;
		_tutorialFrame = new Rect(__width / 2 - __tutWidth / 2 - __offside / 2, 
		                          __height / 2 - __tutHeight / 2- __offside /2,
		                          __tutWidth + __offside, __tutHeight + __offside
		                          );
		_previousState = _gameManager.GetGameState ();
	}
	
	void OnGUI ()
	{
		GameState __currentState = _gameManager.GetGameState ();
		if(__currentState == GameState.Tutorial)
		{
            if (tutorial == null)
            {
                _gameManager.SetGameState(GameState.Pregame);
                return;
            }
			int __depth = GUI.depth;
			GUI.depth = 0;
			GUI.Box (_tutorialRegion,tutorial,noStyle);
			GUI.depth = __depth;
			tutorial.Play ();
			if(GUI.Button (_tutorialFrame,frame, noStyle))
			{
				tutorial.Stop();
				if(_previousState == GameState.Tutorial){
					_gameManager.SetGameState(GameState.Pregame);
				}
				else
					_gameManager.SetGameState(_previousState);
			}
			return;
		}
		// While the game is in progress, only display the pause button
		if (__currentState == GameState.Running || __currentState == GameState.Pregame) 
		{
			if (GUI.Button (_pauseButtonRegion, PauseButton, MGUI.noStyle)) 
			{
				_gameManager.PauseGame ();
			}
			if (tutorial != null && GUI.Button (_tutorialButtonRegion, PauseButton, MGUI.noStyle)) 
			{
				_previousState = __currentState;
				_gameManager.SetGameState(GameState.Tutorial);
				tutorial.Play();
			}
		} 
		else 
		{
			_ShowBottomMenu ();
		}	
	}
	#endregion
	
	#region METHODS
	IEnumerator _LoadMainMenu (AudioSource source)
	{
		Time.timeScale = 1.0f;
		ScreenChoice __choice = Manager.GetScreenChoice ();
		if (__choice == ScreenChoice.Map) {
			LoadLevel ("MapWorld");
		} else if (__choice == ScreenChoice.Button) {
			LoadLevel ("GameSelectionScene");
		} else {
			LoadLevel ("GameSelectionScene");
		}
		
		if (source != null) {
			while (source.volume > 0) {
				source.volume -= 0.02f;	
				yield return null;
			}
		}
				
		music = null;
		Destroy (source.gameObject);
	}
	
	IEnumerator _LoadWinScene (AudioSource source)
	{
		
		if (source != null) {
			while (source.volume > 0) {
				source.volume -= 0.02f;	
				yield return null;
			}
		}
		LoadLevel ("WinScene");
		Time.timeScale = 1.0f;
		music = null;
		Destroy (source.gameObject);
	}
		
	private void _ShowBottomMenu ()
	{
		switch (_gameManager.GetGameState ()) {
		case GameState.Paused:
			_HandlePauseState ();
			break;
		case GameState.Won:
			_HandleWonState ();
			break;
		case GameState.Lost:
			_HandleLostState ();
			break;
		}
	}
	
	private void _HandlePauseState ()
	{
		if (MGUI.HoveredButton (_mainMenuButtonRegion, MainMenuButton)) {
			GameObject obj = GameObject.FindGameObjectWithTag ("SoundCam");
			//Only for debug for horse game since SoundCam object is not there yet.
			if (obj != null)
				StartCoroutine (_LoadMainMenu (obj.audio));
			else {
				Time.timeScale = 1.0f;
				LoadLevel ("GameSelectionScene");
			}
		} else if (MGUI.HoveredButton (_restartButtonRegion, Restart)) {
			_gameManager.RestartGame ();
		} else if (MGUI.HoveredButton (_nextLevelButtonRegion, PlayButton)) {
			_gameManager.UnpauseGame ();
		}
	}
	
	private void _HandleWonState ()
	{
		if (MGUI.HoveredButton (_mainMenuButtonRegion, MainMenuButton)) {
			GameObject obj = GameObject.FindGameObjectWithTag ("SoundCam");
			StartCoroutine (_LoadMainMenu (obj.audio));
		} else if (MGUI.HoveredButton (_restartButtonRegion, Restart)) {
			_gameManager.RestartGame ();
		} else if (MGUI.HoveredButton (_nextLevelButtonRegion, PlayButton)) {
			if(!_gameManager.isLastLevel)
				_gameManager.GoToNextLevel();
			else
				StartCoroutine(_LoadWinScene(GameObject.FindGameObjectWithTag("SoundCam").audio));
		}
	}
	
	private void _HandleLostState ()
	{
		if (MGUI.HoveredButton (_mainMenuButtonRegion, MainMenuButton)) {
			GameObject obj = GameObject.FindGameObjectWithTag ("SoundCam");
			StartCoroutine (_LoadMainMenu (obj.audio));
		} else if (MGUI.HoveredButton (_restartButtonRegion, Restart)) {
			_gameManager.RestartGame ();
		}
	}
	#endregion
}