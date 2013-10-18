using UnityEngine;
using System.Collections;

/// <summary>
/// In game menu GUI.
/// Attached to the game manager object
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class InGameMenuGUI : MonoBehaviour 
{	
	public Texture gameTitleTexture;
	public float gamePreviewWidthToScreenWidthRatio = 0.75f;
	public float barHeightToScreenHeightRatio = 0.25f;
	public float gamePreviewArrowHeightRation = 0.2f;//  height ration of the white speach arrow pointing to character to total height of preview screen
	public Texture Restart,PlayButton,MainMenuButton,PauseButton;
	public static int currentLevel = 1;
	public static string selectedGameName;
	public static GameObject music;
	
	private GameManager _gameManager;
	private bool _callOnce = false;
	private int _gamesNumber;
	private AudioSource _audioSource;
	
	// Use this for initialization
	void Start () 
	{
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		currentLevel = 1;
	}
	
	void OnGUI() {		
		float screenUnitW = Screen.width/100;
		GameState currentState = _gameManager.GetGameState();
		// While the game is in progress, only display the pause button
		if (currentState == GameState.Running||currentState == GameState.Pregame) 
		{
			if (GUI.Button(new Rect(Screen.width - screenUnitW*11, 2, (Screen.width/10), (Screen.width/10)), PauseButton, MGUI.NoStyle)) 
			{	
				_gameManager.PauseGame();
			}
		}
		else {
			switch (currentState) 
			{
				case GameState.Paused: 
					ShowBottomMenu();
					break;
				case GameState.Over:
					ShowBottomMenu();
					break;
			}
		}	
	}

	
	IEnumerator LoadMainMenu(AudioSource source)
	{
		if (source != null){
			while(source.volume > 0){
				source.volume -= 0.02f;	
				yield return null;
			}
		}
		Time.timeScale = 1.0f;
		ScreenChoice _choice = Manager.GetScreenChoice();
		if(_choice == ScreenChoice.Map)
		{
			Application.LoadLevel("MapWorld");
		}
		else if(_choice == ScreenChoice.Button)
		{
			Application.LoadLevel("ChooseGameScene");
		}else{
			Application.LoadLevel("ChooseGameScene");
		}
		music = null;
		Destroy (source.gameObject);
	}
	IEnumerator LoadWinScene(AudioSource source)
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
	IEnumerator FadeInMusic(AudioSource source)
	{
		if (source != null)
		while(source.volume < 1){
			source.volume += 0.02f;	
			yield return null;
		}
	}
	IEnumerator FadeOutMusic(AudioSource source)
	{
		if (source != null)
		{
			while(source.volume > 0){
				source.volume -= 0.02f;	
				yield return null;
			}
		}
	}
	IEnumerator WaitAndLoadNext()
	{
		yield return StartCoroutine(FadeOutMusic(audio));
		_gameManager.GoToNextLevel();
	}
		
	void ShowBottomMenu()
	{
		GameState currentState = _gameManager.GetGameState();
		
		if(currentState == GameState.Paused)
		{
			if (MGUI.HoveredButton(new Rect(MGUI.Margin*3, Screen.height - (Screen.width/6), Screen.width/7, Screen.width/7), MainMenuButton)) 
			{
				GameObject obj = GameObject.FindGameObjectWithTag("SoundCam");
				StartCoroutine(LoadMainMenu(obj.audio));
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
		else if(currentState == GameState.Over)
		{
			if(!_gameManager.isLastLevel)
			{
				if (MGUI.HoveredButton(new Rect(MGUI.Margin*3, Screen.height - (Screen.width/6), Screen.width/7, Screen.width/7), MainMenuButton)) 
			{
				GameObject obj = GameObject.FindGameObjectWithTag("SoundCam");
				StartCoroutine(LoadMainMenu(obj.audio));
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
					StartCoroutine(LoadWinScene(obj.audio));
				}
			}
		}
	}
	
}