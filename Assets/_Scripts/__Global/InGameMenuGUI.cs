using UnityEngine;
using System.Collections;

/// <summary>
/// In game menu GUI.
/// Attached to the game manager object
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class InGameMenuGUI : MonoBehaviour 
{	
	#region MEMBERS
	private GameManager _gameManager;
	private bool _callOnce = false;
	private int _gamesNumber;
	private AudioSource _audioSource;
	private FadeScreen _fade;
	
	private Rect _mainMenuButtonRegion;
	private Rect _restartButtonRegion;
	private Rect _nextLevelButtonRegion;
	
	public Texture gameTitleTexture;
	public float gamePreviewWidthToScreenWidthRatio = 0.75f;
	public float barHeightToScreenHeightRatio = 0.25f;
	public float gamePreviewArrowHeightRation = 0.2f;//  height ration of the white speach arrow pointing to character to total height of preview screen
	public Texture Restart,PlayButton,MainMenuButton,PauseButton;
	public static int currentLevel = 1;
	public static string selectedGameName;
	public static GameObject music;
	#endregion
	#region UNITY_METHODS
	// Use this for initialization
	IEnumerator Start () 
	{
		_fade = gameObject.AddComponent<FadeScreen>();
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		currentLevel = 1;
		
		_mainMenuButtonRegion = new Rect(
			MGUI.Margin*3, 
			Screen.height - (Screen.width/6), 
			Screen.width/7, 
			Screen.width/7);
		
		_restartButtonRegion = new Rect(
			Screen.width -(Screen.width/2 + Screen.width/14),
			Screen.height - (Screen.width/6), 
			Screen.width/7, 
			Screen.width/7);
		
		_nextLevelButtonRegion = new Rect(
			Screen.width - (Screen.width/3 - Screen.width/7),
			Screen.height - (Screen.width/6), 
			Screen.width/7, 
			Screen.width/7);
		
		yield return StartCoroutine(_fade.WaitAndFadeIn());
	}
	
	void OnGUI() 
	{		
		float __screenUnitW = Screen.width/100;
		GameState __currentState = _gameManager.GetGameState();
		// While the game is in progress, only display the pause button
		if (__currentState == GameState.Running || __currentState == GameState.Pregame) 
		{
			Rect __pauseButtonRegion = new Rect(
				Screen.width - __screenUnitW * 11f, 
				2f, 
				(Screen.width/10), 
				(Screen.width/10));
			
			if (GUI.Button(__pauseButtonRegion, PauseButton, MGUI.NoStyle)) 
			{	
				_gameManager.PauseGame();
			}
		}
		else 
		{
			_ShowBottomMenu();
		}	
	}
	#endregion
	
	#region METHODS
	IEnumerator _LoadMainMenu(AudioSource source)
	{
		if (source != null)
		{
			while(source.volume > 0)
			{
				source.volume -= 0.02f;	
				yield return null;
			}
		}
		
		Time.timeScale = 1.0f;
		ScreenChoice __choice = Manager.GetScreenChoice();
		if(__choice == ScreenChoice.Map)
		{
			Application.LoadLevel("MapWorld");
		}
		else if(__choice == ScreenChoice.Button)
		{
			Application.LoadLevel("ChooseGameScene");
		}
		else
		{
			Application.LoadLevel("ChooseGameScene");
		}
		
		music = null;
		Destroy (source.gameObject);
	}
	
	IEnumerator _LoadWinScene(AudioSource source)
	{
		yield return StartCoroutine(_fade.WaitAndFadeOut());
		if (source != null)
		{
			while(source.volume > 0)
			{
				source.volume -= 0.02f;	
				yield return null;
			}
		}
		Time.timeScale = 1.0f;
		Application.LoadLevel("WinScene");
		music = null;
		Destroy (source.gameObject);
	}
	
	IEnumerator _LoadNextLevel()
	{
		yield return StartCoroutine(_fade.WaitAndFadeOut());
		_gameManager.GoToNextLevel();
	}
		
	private void _ShowBottomMenu()
	{
		switch(_gameManager.GetGameState()) 
		{
		case GameState.Paused:
			_HandlePauseState();
			break;
		case GameState.Won:
			_HandleWonState();
			break;
		case GameState.Lost:
			_HandleLostState();
			break;
		}
	}	
	
	private void _HandlePauseState() {
		if (MGUI.HoveredButton(_mainMenuButtonRegion, MainMenuButton)) 
		{
			GameObject obj = GameObject.FindGameObjectWithTag("SoundCam");
			//Only for debug for horse game since SoundCam object is not there yet.
			if(obj != null)
				StartCoroutine(_LoadMainMenu(obj.audio));
			else{
				Time.timeScale = 1.0f;
				Application.LoadLevel("ChooseGameScene");
			}
		}
		else if (MGUI.HoveredButton(_restartButtonRegion, Restart)) 
		{
			_gameManager.RestartGame();
		}
		else if (MGUI.HoveredButton(_nextLevelButtonRegion, PlayButton))
		{
			_gameManager.UnpauseGame();
		}
	}
	
	private void _HandleWonState() {
		if(!_gameManager.isLastLevel)
		{
			if (MGUI.HoveredButton(_mainMenuButtonRegion, MainMenuButton)) 
			{
				GameObject obj = GameObject.FindGameObjectWithTag("SoundCam");
				StartCoroutine(_LoadMainMenu(obj.audio));
			}
			else if (MGUI.HoveredButton(_restartButtonRegion, Restart)) 
			{
				_gameManager.RestartGame();
			}
			else if (MGUI.HoveredButton(_nextLevelButtonRegion, PlayButton)) 
			{
				StartCoroutine(_LoadNextLevel());
			}
		}
		else
		{
			if(!_callOnce)
			{
				_callOnce = true;
				GameObject obj = GameObject.FindGameObjectWithTag("SoundCam");
				StartCoroutine(_LoadWinScene(obj.audio));
			}
		}
	}
	
	private void _HandleLostState() {
		if (MGUI.HoveredButton(_mainMenuButtonRegion, MainMenuButton)) 
		{
			GameObject obj = GameObject.FindGameObjectWithTag("SoundCam");
			StartCoroutine(_LoadMainMenu(obj.audio));
		}
		else if (MGUI.HoveredButton(_restartButtonRegion, Restart)) 
		{
			_gameManager.RestartGame();
		}
	}
	#endregion
}