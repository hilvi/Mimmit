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
    public static GameObject music;
    public Texture2D restart;
    public Texture2D playButton;
    public Texture2D mainMenuButton;
    public Texture2D pauseButton;
    public Texture2D tutorialButton;
    public Texture2D frame;
    public Texture2D cross;
    public Texture2D backPause;
    public Texture2D storeButtonOff;
    public Texture2D storeButtonOn;
    public MovieTexture tutorial;
    public Rect tutorialVidRegion;
    public Rect tutorialFrame;
    public Rect crossRect;

    private GameManager _gameManager;
    private Rect _tutorialButtonRegion;
    private Rect _pauseButtonRegion;
    private Rect _mainMenuButtonRegion;
    private Rect _restartButtonRegion;
    private Rect _nextLevelButtonRegion;
    private Rect _storeButtonRect;
    private GUIStyle _noStyle = new GUIStyle();
    private GameState _previousState;
    private Texture2D _storeButtonTexture;
    #endregion

    #region UNITY_METHODS
    void Start()
    {
        FadeIn();

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _previousState = _gameManager.GetGameState();

        // in-game menu construction
        {
            float __horizontalOffset = 85f;
            float __buttonMargin = 5f;
            float __buttonSize = Screen.width / 15f;

            _pauseButtonRegion = new Rect(
                __horizontalOffset + __buttonMargin,
                __buttonMargin,
                __buttonSize,
                __buttonSize);

            _tutorialButtonRegion = _pauseButtonRegion;
            _tutorialButtonRegion.x += __buttonSize;
        }

        // pause menu construction - regular buttons
        {
            float __horizontalOffset = 190f;
            float __verticalOffset = Screen.height - 205f;
            float __buttonMargin = 242f;
            float __buttonSize = Screen.width / 10f;

            _mainMenuButtonRegion = new Rect(
                __horizontalOffset,
                __verticalOffset,
                __buttonSize,
                __buttonSize);

            _restartButtonRegion = _mainMenuButtonRegion;
            _nextLevelButtonRegion = _mainMenuButtonRegion;
            _restartButtonRegion.x += __buttonMargin;
            _nextLevelButtonRegion.x += __buttonMargin * 2f;
        }

        // pause menu construction - store button
        {
            float __verticalOffset = 315f;
            float __scalingFactor = 0.85f;
            float __buttonWidth = storeButtonOn.width * __scalingFactor;
            float __buttonHeight = storeButtonOn.height * __scalingFactor;

            _storeButtonRect = new Rect(
                Screen.width / 2f - __buttonWidth / 2f,
                __verticalOffset,
                __buttonWidth,
                __buttonHeight);
        }
    }

    void OnGUI()
    {
        GameState __currentState = _gameManager.GetGameState();
        if (__currentState == GameState.Tutorial)
        {
            if (tutorial == null)
            {
                _gameManager.SetGameState(GameState.Pregame);
                return;
            }

            int __depth = GUI.depth;
            GUI.depth = 0;
            GUI.Box(tutorialVidRegion, tutorial, _noStyle);
            GUI.depth = __depth;
            tutorial.Play();

            if (GUI.Button(tutorialFrame, frame, _noStyle) || GUI.Button(crossRect, cross, _noStyle))
            {
                tutorial.Stop();
				Time.timeScale = 1;
                if (_previousState == GameState.Tutorial)
                {
                    _gameManager.SetGameState(GameState.Pregame);
                }
                else
                {
                    _gameManager.SetGameState(_previousState);
                }
            }
        }
        else if (__currentState == GameState.Running || __currentState == GameState.Pregame)
        {
            if (GUI.Button(_pauseButtonRegion, pauseButton, MGUI.noStyle))
            {
                _gameManager.PauseGame();
            }

            if (tutorial != null && GUI.Button(_tutorialButtonRegion, tutorialButton, MGUI.noStyle))
            {
                _previousState = __currentState;
                _gameManager.SetGameState(GameState.Tutorial);
				Time.timeScale = 0;
                tutorial.Play();
            }
        }
        else
        {
            _ShowBottomMenu();
        }
    }
    #endregion

    #region METHODS
    private IEnumerator _LoadMainMenu(AudioSource source)
    {
        Time.timeScale = 1.0f;

        string __exitScene = "GameSelectionScene";
        if (!string.IsNullOrEmpty(_gameManager.exitScene))
        {
            __exitScene = _gameManager.exitScene;
        }

        ScreenChoice __choice = Manager.GetScreenChoice();
        switch (__choice)
        {
            case ScreenChoice.Map:
                LoadLevel("MapWorld");
                break;
            case ScreenChoice.Button:
                LoadLevel(__exitScene);
                break;
            default:
                LoadLevel(__exitScene);
                break;
        }

        if (source != null)
        {
            while (source.volume > 0)
            {
                source.volume -= 0.04f;
                yield return null;
            }
        }

        music = null;
        Destroy(source.gameObject);
    }

    private IEnumerator _LoadWinScene(AudioSource source)
    {
        if (source != null)
        {
            while (source.volume > 0)
            {
                source.volume -= 0.02f;
                yield return null;
            }
        }

        LoadLevel("WinScene");
        Time.timeScale = 1.0f;
        music = null;
        Destroy(source.gameObject);
    }

    private void _ShowBottomMenu()
    {
        switch (_gameManager.GetGameState())
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

    private void _HandlePauseState()
    {
        bool __cursorInStoreButton = (_storeButtonRect.Contains(InputManager.MouseScreenToGUI()));
        _storeButtonTexture = (__cursorInStoreButton) ? storeButtonOn : storeButtonOff;

        Rect __screenRect = new Rect(0f, 0f, Screen.width, Screen.height);
        GUI.DrawTexture(__screenRect, backPause);
        
        if (MGUI.HoveredButton(_mainMenuButtonRegion, mainMenuButton))
        {
            GameObject __soundCam = GameObject.FindGameObjectWithTag("SoundCam");
            StartCoroutine(_LoadMainMenu(__soundCam.audio));
        }
        else if (MGUI.HoveredButton(_restartButtonRegion, restart))
        {
            _gameManager.RestartGame();
        }

        else if (MGUI.HoveredButton(_nextLevelButtonRegion, playButton))
        {
            _gameManager.UnpauseGame();
        }
        else if (GUI.Button(_storeButtonRect, _storeButtonTexture, _noStyle))
        {
            Application.OpenURL(@"www.juniori.fi/mimmikoto/tuotteet.shtml");
        }
    }

    private void _HandleWonState()
    {
        bool __cursorInStoreButton = (_storeButtonRect.Contains(InputManager.MouseScreenToGUI()));
        _storeButtonTexture = (__cursorInStoreButton) ? storeButtonOn : storeButtonOff;

        Rect __screenRect = new Rect(0f, 0f, Screen.width, Screen.height);
        GUI.DrawTexture(__screenRect, backPause);
        if (MGUI.HoveredButton(_mainMenuButtonRegion, mainMenuButton))
        {
            GameObject __soundCam = GameObject.FindGameObjectWithTag("SoundCam");
            StartCoroutine(_LoadMainMenu(__soundCam.audio));
        }
        else if (MGUI.HoveredButton(_restartButtonRegion, restart))
        {
            _gameManager.RestartGame();
        }
        else if (MGUI.HoveredButton(_nextLevelButtonRegion, playButton))
        {
            if (!_gameManager.isLastLevel)
            {
                _gameManager.GoToNextLevel();
            }
            else
            {
                GameObject __soundCam = GameObject.FindGameObjectWithTag("SoundCam");
                StartCoroutine(_LoadWinScene(__soundCam.audio));
            }
        }
        else if (GUI.Button(_storeButtonRect, _storeButtonTexture, _noStyle))
        {
            Application.OpenURL(@"www.juniori.fi/mimmikoto/tuotteet.shtml");
        }
    }

    private void _HandleLostState()
    {
        if (MGUI.HoveredButton(_mainMenuButtonRegion, mainMenuButton))
        {
            GameObject __soundCam = GameObject.FindGameObjectWithTag("SoundCam");
            StartCoroutine(_LoadMainMenu(__soundCam.audio));
        }
        else if (MGUI.HoveredButton(_restartButtonRegion, restart))
        {
            _gameManager.RestartGame();
        }
    }
    #endregion
}