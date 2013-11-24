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
    private bool _dragging;
    private MagicLine _line;
    private GameObject _lineContainer; // Store lines, keep hierarchy clean
    #endregion

    #region UNITY_METHODS
    public override void Start()
    {
        // Boilerplate
        base.Start();
        SetGameState(GameState.Running);

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
        if (GetGameState() == GameState.Paused)
            return;

        // Left mouse down - Start drawing
        if (Input.GetMouseButtonDown(0))
        {
            _dragging = true;

            // Preserve active line, because GetComponent is relatively expensive 
            var __o = (GameObject)(Instantiate(linePrefab));
            __o.transform.parent = _lineContainer.transform;
            _line = __o.GetComponent<MagicLine>();
            
            // Set starting position
            Vector3 __t = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _line.SetStartingPosition(new Vector2(__t.x, __t.y));
        }

        //  Left mouse up  - Stop drawing
        if (Input.GetMouseButtonUp(0))
        {
            _dragging = false;
            _line = null;
        }

        // If currently drawing, advance magic line by one frame
        if (_dragging && _line != null)
        {
            _line.Step();
        }
    }
    #endregion
}
