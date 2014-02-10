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
	public Texture restart, playButton, mainMenuButton, pauseButton,tutorialButton;
	public static GameObject music;
	public MovieTexture tutorial;
	public Texture2D frame;
	public Texture2D cross;
    public Texture2D backPause;

	private GameManager _gameManager;
	private Rect _tutorialButtonRegion;
	private Rect _pauseButtonRegion;
	private Rect _mainMenuButtonRegion;
	private Rect _restartButtonRegion;
	private Rect _nextLevelButtonRegion;
	// Tutorial rects
	public Rect tutorialVidRegion;
	public Rect tutorialFrame;
	public Rect crossRect;
	private GUIStyle noStyle = new GUIStyle();
	private GameState _previousState;
    private GUITexture backPauseTexture;
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
		float __offset = 85f;
		_pauseButtonRegion = new Rect(__margin + __offset, __margin, __sizeButton, __sizeButton);
		_tutorialButtonRegion = new Rect(__margin + __offset + __sizeButton , __margin, __sizeButton, __sizeButton);

        float __widthA = 205f;
		float __widthB = __width / 10;
        float margin = 190f;

		_mainMenuButtonRegion = new Rect (
			margin, 
			__height - (__widthA), 
			__widthB, 
			__widthB);
		
		_restartButtonRegion = new Rect (
			__width / 2f - __widthB / 2f,
			__height - (__widthA), 
			__widthB, 
			__widthB);
		
		_nextLevelButtonRegion = new Rect (
			__width - (margin + __widthB),
			__height - (__widthA), 
			__widthB, 
			__widthB);
		_previousState = _gameManager.GetGameState ();

        backPauseTexture = GetComponent<GUITexture>();
        if (backPauseTexture == null)
        {
            backPauseTexture = gameObject.AddComponent<GUITexture>();
        }
        backPauseTexture.texture = backPause;
        backPauseTexture.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
        transform.localScale = new Vector3(0f,0f,0f);
        backPauseTexture.enabled = false;
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
			GUI.Box (tutorialVidRegion,tutorial,noStyle);
			GUI.depth = __depth;
			tutorial.Play ();
			if(GUI.Button (tutorialFrame,frame, noStyle) || GUI.Button (crossRect, cross, noStyle))
			{
				tutorial.Stop();
				if(_previousState == GameState.Tutorial)
					_gameManager.SetGameState(GameState.Pregame);
				else
					_gameManager.SetGameState(_previousState);
			}
			return;
		}
		// While the game is in progress, only display the pause button
		if (__currentState == GameState.Running || __currentState == GameState.Pregame) 
		{
			if (GUI.Button (_pauseButtonRegion, pauseButton, MGUI.noStyle)) 
			{
                backPauseTexture.enabled = true;
				_gameManager.PauseGame ();
			}
			if (tutorial != null && GUI.Button (_tutorialButtonRegion, tutorialButton, MGUI.noStyle)) 
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

		string __exitScene = "GameSelectionScene";
		if(!string.IsNullOrEmpty(_gameManager.exitScene))
			__exitScene = _gameManager.exitScene;

		ScreenChoice __choice = Manager.GetScreenChoice ();
		if (__choice == ScreenChoice.Map) 
		{
			LoadLevel ("MapWorld");
		} 
		else if (__choice == ScreenChoice.Button) 
		{
			LoadLevel (__exitScene);
		} 
		else 
		{
			LoadLevel (__exitScene);
		}
		
		if (source != null) {
			while (source.volume > 0) 
			{
				source.volume -= 0.04f;	
				yield return null;
			}
		}
				
		music = null;
		Destroy (source.gameObject);
	}
	
	IEnumerator _LoadWinScene (AudioSource source)
	{
		
		if (source != null) 
		{
			while (source.volume > 0) 
			{
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
		switch (_gameManager.GetGameState ()) 
		{
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
        
		if (MGUI.HoveredButton (_mainMenuButtonRegion, mainMenuButton)) 
		{
			GameObject obj = GameObject.FindGameObjectWithTag ("SoundCam");
			//Only for debug for horse game since SoundCam object is not there yet.
			if (obj != null)
				StartCoroutine (_LoadMainMenu (obj.audio));
			else 
			{
				Time.timeScale = 1.0f;
				LoadLevel ("GameSelectionScene");
			}
		} 
		else if (MGUI.HoveredButton (_restartButtonRegion, restart)) 
		{
			_gameManager.RestartGame ();
		} 
		else if (MGUI.HoveredButton (_nextLevelButtonRegion, playButton)) 
		{
            backPauseTexture.enabled = false;
			_gameManager.UnpauseGame ();
		}
	}
	
	private void _HandleWonState ()
	{
		if (MGUI.HoveredButton (_mainMenuButtonRegion, mainMenuButton)) 
		{
			GameObject obj = GameObject.FindGameObjectWithTag ("SoundCam");
			StartCoroutine (_LoadMainMenu (obj.audio));
		} 
		else if (MGUI.HoveredButton (_restartButtonRegion, restart)) 
		{
			_gameManager.RestartGame ();
		} 
		else if (MGUI.HoveredButton (_nextLevelButtonRegion, playButton)) 
		{
			if(!_gameManager.isLastLevel)
				_gameManager.GoToNextLevel();
			else
				StartCoroutine(_LoadWinScene(GameObject.FindGameObjectWithTag("SoundCam").audio));
		}
	}
	
	private void _HandleLostState ()
	{
		if (MGUI.HoveredButton (_mainMenuButtonRegion, mainMenuButton)) 
		{
			GameObject obj = GameObject.FindGameObjectWithTag ("SoundCam");
			StartCoroutine (_LoadMainMenu (obj.audio));
		} 
		else if (MGUI.HoveredButton (_restartButtonRegion, restart)) 
		{
			_gameManager.RestartGame ();
		}
	}
	#endregion
}