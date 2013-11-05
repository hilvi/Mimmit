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
	private GameManager _gameManager;
	private Rect _pauseButtonRegion;
	private Rect _mainMenuButtonRegion;
	private Rect _restartButtonRegion;
	private Rect _nextLevelButtonRegion;
	#endregion
	
	#region UNITY_METHODS
	void Start ()
	{
		FadeIn ();
		_gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();

		_pauseButtonRegion = new Rect (
            Screen.width - Screen.width / 100 * 11f,
            2f,
            (Screen.width / 10),
            (Screen.width / 10));

		_mainMenuButtonRegion = new Rect (
			MGUI.margin * 3, 
			Screen.height - (Screen.width / 6), 
			Screen.width / 7, 
			Screen.width / 7);
		
		_restartButtonRegion = new Rect (
			Screen.width - (Screen.width / 2 + Screen.width / 14),
			Screen.height - (Screen.width / 6), 
			Screen.width / 7, 
			Screen.width / 7);
		
		_nextLevelButtonRegion = new Rect (
			Screen.width - (Screen.width / 3 - Screen.width / 7),
			Screen.height - (Screen.width / 6), 
			Screen.width / 7, 
			Screen.width / 7);
	}
	
	void OnGUI ()
	{
		GameState __currentState = _gameManager.GetGameState ();
		// While the game is in progress, only display the pause button
		if (__currentState == GameState.Running || __currentState == GameState.Pregame) {
			if (GUI.Button (_pauseButtonRegion, PauseButton, MGUI.noStyle)) {	
				_gameManager.PauseGame ();
			}
		} else {
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
			LoadLevel ("ChooseGameScene");
		} else {
			LoadLevel ("ChooseGameScene");
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
				LoadLevel ("ChooseGameScene");
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