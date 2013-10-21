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
	void Start () 
	{
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		currentLevel = 1;
	}
	
	void OnGUI() 
	{		
		float __screenUnitW = Screen.width/100;
		GameState __currentState = _gameManager.GetGameState();
		// While the game is in progress, only display the pause button
		if (__currentState == GameState.Running||__currentState == GameState.Pregame) 
		{
			if (GUI.Button(new Rect(Screen.width - __screenUnitW*11, 2, (Screen.width/10), (Screen.width/10)), PauseButton, MGUI.NoStyle)) 
			{	
				_gameManager.PauseGame();
			}
		}
		else {
			_ShowBottomMenu();
		}	
	}
	#endregion
	
	#region METHODS
	IEnumerator _LoadMainMenu(AudioSource source)
	{
		if (source != null){
			while(source.volume > 0){
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
		}else{
			Application.LoadLevel("ChooseGameScene");
		}
		music = null;
		Destroy (source.gameObject);
	}
	
	IEnumerator _LoadWinScene(AudioSource source)
	{
		if (source != null){
			while(source.volume > 0){
				source.volume -= 0.02f;	
				yield return null;
			}
		}
		Time.timeScale = 1.0f;
		Application.LoadLevel("WinScene");
		music = null;
		Destroy (source.gameObject);
	}
		
	void _ShowBottomMenu()
	{
		GameState __currentState = _gameManager.GetGameState();
		print (__currentState);
		if(__currentState == GameState.Paused)
		{
			if (MGUI.HoveredButton(new Rect(MGUI.Margin*3, Screen.height - (Screen.width/6), Screen.width/7, Screen.width/7), MainMenuButton)) 
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
			// Middle Button always show when menu is on
			if (MGUI.HoveredButton(new Rect(Screen.width -(Screen.width/2 + Screen.width/14),Screen.height - (Screen.width/6), 
				Screen.width/7, Screen.width/7), Restart)) 
			{
				_gameManager.RestartGame();
			}
			if (MGUI.HoveredButton(new Rect(Screen.width - (Screen.width/3 - Screen.width/7), Screen.height - (Screen.width/6), 
				Screen.width/7, Screen.width/7), PlayButton))
			{
				_gameManager.UnpauseGame();
			}
		}
		else if(__currentState == GameState.Won)
		{
			if(!_gameManager.isLastLevel)
			{
				if (MGUI.HoveredButton(new Rect(MGUI.Margin*3, Screen.height - (Screen.width/6), Screen.width/7, Screen.width/7), MainMenuButton)) 
				{
					GameObject obj = GameObject.FindGameObjectWithTag("SoundCam");
					StartCoroutine(_LoadMainMenu(obj.audio));
				}
				// Middle Button always show when menu is on
				if (MGUI.HoveredButton(new Rect(Screen.width -(Screen.width/2 + Screen.width/14),Screen.height - (Screen.width/6), 
					Screen.width/7, Screen.width/7), Restart)) 
				{
					_gameManager.RestartGame();
				}
				if (MGUI.HoveredButton(new Rect(Screen.width - (Screen.width/3 - Screen.width/7), Screen.height - (Screen.width/6), 
					Screen.width/7, Screen.width/7), PlayButton) && !_gameManager.isLastLevel) 
				{
						_gameManager.GoToNextLevel();		
				}
			}else
			{
				if(!_callOnce)
				{
					_callOnce = true;
					GameObject obj = GameObject.FindGameObjectWithTag("SoundCam");
					StartCoroutine(_LoadWinScene(obj.audio));
				}
			}
		}
		else if(__currentState == GameState.Lost)
		{
			if (MGUI.HoveredButton(new Rect(MGUI.Margin*3, Screen.height - (Screen.width/6), Screen.width/7, Screen.width/7), MainMenuButton)) 
			{
				GameObject obj = GameObject.FindGameObjectWithTag("SoundCam");
				StartCoroutine(_LoadMainMenu(obj.audio));
			}
			// Middle Button always show when menu is on
			if (MGUI.HoveredButton(new Rect(Screen.width -(Screen.width/2 + Screen.width/14),Screen.height - (Screen.width/6), 
				Screen.width/7, Screen.width/7), Restart)) 
			{
				_gameManager.RestartGame();
			}
		}
	}	
	#endregion
}