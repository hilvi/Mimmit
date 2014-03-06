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
    public Texture2D storeButtonOff;
    public Texture2D storeButtonOn;
    public Rect tutorialVidRegion;
    public Rect tutorialFrame;
    public Rect crossRect;

	private GameManager m_gameManager;
	private Rect r_tutorialButtonRegion;
	private Rect r_pauseButtonRegion;
	private Rect r_mainMenuButtonRegion;
	private Rect r_restartButtonRegion;
	private Rect r_nextLevelButtonRegion;
    private Rect r_storeButtonRect;
	private GUIStyle m_noStyle = new GUIStyle();
	private GameState e_previousState;
    private Texture2D t_storeButton;
	#endregion

	#region UNITY_METHODS
	void Start ()
	{
		FadeIn ();
		m_gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		float __width = Screen.width;
		float __height = Screen.height;
		float __sizeButton = __width / 15;

		float __margin = 5f;
		float __offset = 85f;
		r_pauseButtonRegion = new Rect(__margin + __offset, __margin, __sizeButton, __sizeButton);
		r_tutorialButtonRegion = new Rect(__margin + __offset + __sizeButton , __margin, __sizeButton, __sizeButton);

        float __widthA = 205f;
		float __widthB = __width / 10;
        float margin = 190f;

		r_mainMenuButtonRegion = new Rect (
			margin, 
			__height - (__widthA), 
			__widthB, 
			__widthB);
		
		r_restartButtonRegion = new Rect (
			__width / 2f - __widthB / 2f,
			__height - (__widthA), 
			__widthB, 
			__widthB);
		
		r_nextLevelButtonRegion = new Rect (
			__width - (margin + __widthB),
			__height - (__widthA), 
			__widthB, 
			__widthB);
        e_previousState = m_gameManager.GetGameState ();

        float storeButtonW = 479.4f;
        float storeButtonH = 72f;
        r_storeButtonRect = new Rect(__width / 2f - storeButtonW / 2f, 315f, storeButtonW, storeButtonH);
	}
	
	void OnGUI ()
    {
		GameState __currentState = m_gameManager.GetGameState ();
		if(__currentState == GameState.Tutorial)
		{
            if (tutorial == null)
            {
                m_gameManager.SetGameState(GameState.Pregame);
                return;
            }
			int __depth = GUI.depth;
			GUI.depth = 0;
			GUI.Box (tutorialVidRegion,tutorial,m_noStyle);
			GUI.depth = __depth;
			tutorial.Play ();
			if(GUI.Button (tutorialFrame,frame, m_noStyle) || GUI.Button (crossRect, cross, m_noStyle))
			{
				tutorial.Stop();
				if(e_previousState == GameState.Tutorial)
					m_gameManager.SetGameState(GameState.Pregame);
				else
					m_gameManager.SetGameState(e_previousState);
			}
            
			return;
		}
		// While the game is in progress, only display the pause button
		if (__currentState == GameState.Running || __currentState == GameState.Pregame) 
		{
			if (GUI.Button (r_pauseButtonRegion, pauseButton, MGUI.noStyle)) 
			{
				m_gameManager.PauseGame ();
			}
			if (tutorial != null && GUI.Button (r_tutorialButtonRegion, tutorialButton, MGUI.noStyle)) 
			{
				e_previousState = __currentState;
				m_gameManager.SetGameState(GameState.Tutorial);
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
		if(!string.IsNullOrEmpty(m_gameManager.exitScene))
			__exitScene = m_gameManager.exitScene;

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
		switch (m_gameManager.GetGameState ()) 
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
        if(r_storeButtonRect.Contains(InputManager.MouseScreenToGUI()))
        {
            t_storeButton = storeButtonOn;
        } else
        {
            t_storeButton = storeButtonOff;
        }

        Rect r = new Rect(0f, 0f, 960f, 600f);
        GUI.DrawTexture(r, backPause);
		if (MGUI.HoveredButton (r_mainMenuButtonRegion, mainMenuButton)) 
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
		else if (MGUI.HoveredButton (r_restartButtonRegion, restart)) 
		{
			m_gameManager.RestartGame ();
		}
        
		else if (MGUI.HoveredButton (r_nextLevelButtonRegion, playButton)) 
		{
			m_gameManager.UnpauseGame ();
		}
        else if (GUI.Button(r_storeButtonRect, t_storeButton, m_noStyle))
        {
            Application.OpenURL(@"www.juniori.fi/mimmikoto/tuotteet.shtml");
        }
	}
	
	private void _HandleWonState ()
	{
        
		if (MGUI.HoveredButton (r_mainMenuButtonRegion, mainMenuButton)) 
		{
			GameObject obj = GameObject.FindGameObjectWithTag ("SoundCam");
			StartCoroutine (_LoadMainMenu (obj.audio));
		} 
		else if (MGUI.HoveredButton (r_restartButtonRegion, restart)) 
		{
			m_gameManager.RestartGame ();
		} 
		else if (MGUI.HoveredButton (r_nextLevelButtonRegion, playButton)) 
		{
			if(!m_gameManager.isLastLevel)
				m_gameManager.GoToNextLevel();
			else
				StartCoroutine(_LoadWinScene(GameObject.FindGameObjectWithTag("SoundCam").audio));
		}
	}
	
	private void _HandleLostState ()
	{
		if (MGUI.HoveredButton (r_mainMenuButtonRegion, mainMenuButton)) 
		{
			GameObject obj = GameObject.FindGameObjectWithTag ("SoundCam");
			StartCoroutine (_LoadMainMenu (obj.audio));
		} 
		else if (MGUI.HoveredButton (r_restartButtonRegion, restart)) 
		{
			m_gameManager.RestartGame ();
		}
	}
	#endregion
}