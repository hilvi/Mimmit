using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PandaGameManager : GameManager
{
    #region MEMBERS
    // Boilerplate
    public GameObject musicObject;
	public AudioClip music;
    // Line Drawing 
    public GameObject linePrefab;
	public MovieTexture mainTutorial;
	public MovieTexture boulderTutorial;
    private MagicLine _line;
    private GameObject _lineContainer; // Store lines, keep hierarchy clean
    private enum LineDrawState { Neutral, Drawing, Erasing, Locked }
    private LineDrawState _currentLineDrawState = LineDrawState.Neutral;
    #endregion

    #region UNITY_METHODS
    public override void Awake()
    {
        // Boilerplate
		base.Awake();

		// Hack to override tutorial for different levels
		if (base.currentLevel == 1) 
		{
			SetGameState(GameState.Tutorial);
			var __gui = GetComponent<InGameMenuGUI>();
			__gui.tutorial = mainTutorial;
		}
		else if (base.currentLevel == 4) 
		{
			SetGameState(GameState.Tutorial);
			var __gui = GetComponent<InGameMenuGUI>();
			__gui.tutorial = boulderTutorial;
		}

        if (InGameMenuGUI.music == null)
        {
            InGameMenuGUI.music = (GameObject)Instantiate(musicObject);
            InGameMenuGUI.music.audio.clip = music;
            InGameMenuGUI.music.audio.Play();
            InGameMenuGUI.music.audio.loop = true;
        }

        _lineContainer = new GameObject("Line Container");
    }
	
	void Update () {
        // If game is paused, prevent any interaction
        if (GetGameState() != GameState.Running && GetGameState() != GameState.Pregame)
            return;

        // Make decision on which state is currently active
        switch (_currentLineDrawState) {
            case LineDrawState.Neutral:
                // We only select a draw state when current state is neutral
                if (Input.GetMouseButtonDown(0)) 
                {
                    _currentLineDrawState = LineDrawState.Drawing;

                    // Preserve active line, because GetComponent is relatively expensive 
                    var __o = (GameObject)(Instantiate(linePrefab));
                    __o.transform.parent = _lineContainer.transform;
                    _line = __o.GetComponent<MagicLine>();

                    // Set starting position
                    Vector3 __t = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    _line.SetStartingPosition(new Vector2(__t.x, __t.y));
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    _currentLineDrawState = LineDrawState.Erasing;
                }
                break;
            case LineDrawState.Drawing:
                // Reset to neutral state 
                if (Input.GetMouseButtonUp(0))
                {
                    _currentLineDrawState = LineDrawState.Neutral;

                    _line = null;
                }
                break;
            case LineDrawState.Erasing:
                // Reset to neutral state 
                if (Input.GetMouseButtonUp(1))
                {
                    _currentLineDrawState = LineDrawState.Neutral;
                }
                break;
        }

        // Handle draw state
        _HandleDrawState(_currentLineDrawState);
    }

    void OnEnable()
    {
        GoalScript.OnBallCapture += WinGame;
        PandaBallScript.OnBallStuck += LoseGame;
        PandaBallScript.OnBallActivate += LockDrawing;
    }

    void OnDisable()
    {
        GoalScript.OnBallCapture -= WinGame;
        PandaBallScript.OnBallStuck -= LoseGame;
        PandaBallScript.OnBallActivate -= LockDrawing;
    }
    #endregion

    #region MEMBERS
    private void WinGame()
    {
        SetGameState(GameState.Won);
    }

    private void LoseGame()
    {
        SetGameState(GameState.Lost);
    }

    private void LockDrawing()
    {
        _currentLineDrawState = LineDrawState.Locked;
    }

    private void _HandleDrawState(LineDrawState state)
    {
        // Drawing is locked, don't do anything
        if (state == LineDrawState.Locked)
            return;

        if (state == LineDrawState.Drawing)
        {
            // Advance magic line by one frame
            if (_line != null)
            {
                _line.Step();
            }
        }
        else if (state == LineDrawState.Erasing)
        {
            // Get mouse position and convert to world pos
            Vector3 __pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            __pos.z = 0f; // Ditch z-axis, because line renderer only accepts Vector3
            Vector2 __pos2d = new Vector2(__pos.x, __pos.y);

            /* 
             * Goal is to find the closest line to cursor.
             * Since lines have multiple points, and we are only interested
             * in endpoints, distances have to be computed for two points for
             * each line instead of one.
             */ 
            var __c = _lineContainer.GetComponentsInChildren<MagicLine>();
            if (__c != null)
            {
                float __furthestPoint = Mathf.Infinity;
                MagicLine __closestLine = null;

                // Find closest line
                foreach (var line in __c)
                {
                    Vector2 __a = line.GetStartPoint();
                    Vector2 __b = line.GetEndPoint();

                    // Select only closest of the two for consideration
                    float __distA = Vector2.Distance(__a, __pos2d);
                    float __distB = Vector2.Distance(__b, __pos2d);
                    float __considered = (__distA < __distB) ? __distA : __distB;

                    if (__considered < __furthestPoint)
                    {
                        __furthestPoint = __considered;
                        __closestLine = line;
                    }
                }

                // Finally strip the line
                if (__closestLine != null)
                {
                    __closestLine.Strip();
                }
            }
        }
    }
    #endregion
}
