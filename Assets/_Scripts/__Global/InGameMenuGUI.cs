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
	private GameManager _gameManager;
	private Rect _tutorialButtonRegion;
	private Rect _pauseButtonRegion;
	private Rect _mainMenuButtonRegion;
	private Rect _restartButtonRegion;
	private Rect _nextLevelButtonRegion;
	private Rect _tutorialRegion;
	private Rect _tutorialFrame;
	private Rect _crossRect;
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
<<<<<<< HEAD
		float __margin = 100f;
		/*_tutorialButtonRegion = new Rect(__width - __sizeButton * 2f - __margin * 2f,__margin,
		                                 __sizeButton, __sizeButton);

		/*_pauseButtonRegion = new Rect (
            __width - __sizeButton - __margin, __margin,
            __sizeButton, __sizeButton);*/
		float __heightPos= 0;
		_pauseButtonRegion = new Rect(__margin,__heightPos,__sizeButton,__sizeButton);
		_tutorialButtonRegion = new Rect(__margin + __sizeButton , __heightPos, __sizeButton,__sizeButton);
=======
		float __margin = 5f;
		_pauseButtonRegion = new Rect(__margin + 100f, __margin, __sizeButton, __sizeButton);
		_tutorialButtonRegion = new Rect(__margin + + 100f + __sizeButton , __margin, __sizeButton, __sizeButton);
>>>>>>> 30a5bf19ee3101873c9494c87ab8d036f9362b47

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
		_previousState = _gameManager.GetGameState ();
		SetTutorialRect();
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
				if(_previousState == GameState.Tutorial)
				{
					_gameManager.SetGameState(GameState.Pregame);
				}
				else
					_gameManager.SetGameState(_previousState);
			}
			GUI.Box(_crossRect,cross,noStyle);
			return;
		}
		// While the game is in progress, only display the pause button
		if (__currentState == GameState.Running || __currentState == GameState.Pregame) 
		{
			if (GUI.Button (_pauseButtonRegion, pauseButton, MGUI.noStyle)) 
			{
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
		ScreenChoice __choice = Manager.GetScreenChoice ();
		if (__choice == ScreenChoice.Map) 
		{
			LoadLevel ("MapWorld");
		} 
		else if (__choice == ScreenChoice.Button) 
		{
			LoadLevel ("GameSelectionScene");
		} 
		else 
		{
			LoadLevel ("GameSelectionScene");
		}
		
		if (source != null) {
			while (source.volume > 0) 
			{
				source.volume -= 0.02f;	
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
	private void SetTutorialRect()
	{
		string __str = _gameManager.gameName;
		switch(__str)
		{
			case "Flip_":
				_tutorialRegion = new Rect(320,170,360,290);
				_tutorialFrame = new Rect(295,145,400,360);
				_crossRect = new Rect(615,345,50,50);
				break;
			case "Horse_":
				_tutorialRegion = new Rect(320,150,320,290);
				_tutorialFrame = new Rect(290,120,380,360);
				_crossRect = new Rect(580,295,50,50);
				break;
			case "Panda_":
				_tutorialRegion = new Rect(320,160,320,290);
				_tutorialFrame = new Rect(285,135,395,360);
				_crossRect = new Rect(580,180,50,50);	
				break;
		}
	}
	#endregion
}