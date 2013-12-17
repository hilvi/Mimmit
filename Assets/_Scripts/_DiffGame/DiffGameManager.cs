using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DiffPictureObject
{
    public Texture2D originalPicture;
    public Texture2D errorPicture;
    public Texture2D background;
    public Rect[] errors;
    public Color frameColor;
}

public class DiffGameManager : GameManager
{
    #region MEMBERS
    #if UNITY_EDITOR
    // Debug controls
    public bool showCheats = false;
    #endif
    // Textures
    public Texture2D tick;
    public Texture2D cross;
    public Texture2D frame;
    private Texture2D _tempFrame;
    // Size of the click
    public float clickSize;
    // Music members
    public GameObject musicObject;
    public AudioClip music;
    public AudioClip missSound;
    public AudioClip hitSound;
    private AudioSource _audioSource;
    // Every diff game object is here
    public DiffPictureObject[] pictureObjects;
    // Store randomized level order
    private int _levelsFinished = 0;
    private List<int> _levelOrder = new List<int>();
    // Error tracking
    private int _errorsLeft;
    private Dictionary<Rect, bool> _errorsFound = new Dictionary<Rect, bool>();
    // Fading miss sticks
    private Dictionary<Rect, float> _misses = new Dictionary<Rect, float>();
    // Regions
    public Rect leftFrame, rightFrame;
    // References
    private CircularCounterScript _counter;
    private GUITexture _backgroundTexture;
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

            // Reduce volume of bg music, so other effects can be heard.
            InGameMenuGUI.music.audio.volume = 0.5f;
        }
        _audioSource = GetComponent<AudioSource>();

        // Set references
        _backgroundTexture = GameObject.Find("Background").GetComponent<GUITexture>();
        _counter = GetComponent<CircularCounterScript>();

        // Generate level order indices
        for (int i = 0; i < pictureObjects.Length; i++)
            _levelOrder.Add(i);

        // Shuffle list (Fisher-Yates shuffle)
        for (int i = 0; i < _levelOrder.Count; i++)
        {
            int __temp = _levelOrder[i];
            int __randomIndex = Random.Range(i, _levelOrder.Count);
            _levelOrder[i] = _levelOrder[__randomIndex];
            _levelOrder[__randomIndex] = __temp;
        }

        // Set first level as zeroth element in order list
        currentLevel = _levelOrder[_levelsFinished];

        // Construct the level
        ConstructLevel();
    }

    void Update()
    {
        // Check if player found an error
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = InputManager.MouseScreenToGUI();
            if (GetGameState() != GameState.Paused &&
                GetGameState() != GameState.Won &&
                rightFrame.Contains(pos))
            {
                _Hit(pos);
            }
        }

        // End game if all errors are found
        if (_errorsLeft == 0)
            SetGameState(GameState.Won);

        // Update counter
        _counter.SetActiveSectors(7 - _errorsLeft);

        // Reduce alpha on all crosses, remove from dict if alpha = 0f
        var __keys = new List<Rect>(_misses.Keys);
        foreach (var k in __keys)
        {
            float newAlpha = _misses[k] - Time.deltaTime;
            newAlpha = Mathf.Clamp(newAlpha, 0f, 2f);

            if (newAlpha <= 0f)
                _misses.Remove(k);
            else
                _misses[k] = newAlpha;
        }

        if (Input.GetMouseButtonDown(1))
            StartCoroutine(LevelTransition());
    }

    void OnGUI()
    {
        // Draw pictures
        GUI.DrawTexture(leftFrame, pictureObjects[currentLevel].originalPicture);
        GUI.DrawTexture(rightFrame, pictureObjects[currentLevel].errorPicture);

        // Draw misses
        foreach (var m in _misses)
        {
            Color tempCol = GUI.color;
            GUI.color = new Color(1f, 1f, 1f, m.Value);
            GUI.DrawTexture(m.Key, cross);
            GUI.color = tempCol;
        }

        // Draw ticks on errors that have been found already
        foreach (var e in _errorsFound)
        {
            if (e.Value)
                GUI.DrawTexture(e.Key, tick);
        }

        // Draw cheat/debug boxes
        #if UNITY_EDITOR
        // Cheats are disabled by default, please enable them in inspector
        if (showCheats)
        {
            int __index = 0;
            foreach (var o in pictureObjects[currentLevel].errors)
                GUI.Box(o, (__index++).ToString());
        }
        #endif

        // Draw frame
        GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), _tempFrame);

        // Draw circular counter
        _counter.Draw();
    }
    #endregion

    #region METHODS
    public override void GoToNextLevel()
    {
        // Begin level transition
        StartCoroutine(LevelTransition());
        Time.timeScale = 1f;
    }

    public override void RestartGame()
    {
        // Begin level restart
        StartCoroutine(LevelRestart());
        Time.timeScale = 1f;
    }

    /* 
     * Levels are always constructed using ConstructLevel-method.
     * It takes care of every little critical detail related to
     * each individual level
     */ 
    private void ConstructLevel()
    {
        // Swap background
        _backgroundTexture.texture = pictureObjects[currentLevel].background;

        // Re-paint frame and counter
        _counter.Create(pictureObjects[currentLevel].frameColor);
        _counter.SetPosition(Screen.width / 2f - 50f, 50f);

        // Init frame
        Color __themeColor = pictureObjects[currentLevel].frameColor;
        _tempFrame = new Texture2D(frame.width, frame.height);
        for (int y = 0; y < frame.height; y++)
        {
            for (int x = 0; x < frame.width; x++)
            {
                Color __pixel = frame.GetPixel(x, y);
                // The reason why pixels is compared to Color.blue, is because
                // frame is defaulted to Color(0, 0, 255). This works like a 
                // green screen, every blue pixel will be replaced to unique color
                // that is defined by individual levels.
                Color __replacement = (__pixel == Color.blue) ? __themeColor : Color.clear;
                _tempFrame.SetPixel(x, y, __replacement);
            }
        }
        _tempFrame.Apply();

        // Reload all errors
        _errorsFound.Clear();
        foreach (Rect r in pictureObjects[currentLevel].errors)
            _errorsFound.Add(r, false);

        // Empty misses
        _misses.Clear();

        // Reset error counter
        _errorsLeft = 7;

        // Reset state
        SetGameState(GameState.Running);
    }

    private IEnumerator LevelTransition()
    {
        // Start fading out
        yield return StartCoroutine(_fade.FadeOut());

        // Behind the scenes routine
        // Advance to next level
        _levelsFinished++;

        // If this is final level, set "isLastLevel" to true, forcing game to 
        // load balloon scene once user beats final diff game level.
        if (_levelsFinished == pictureObjects.Length - 1)
            isLastLevel = true;

        // Update current level index
        currentLevel = _levelOrder[_levelsFinished];

        // Construct level
        ConstructLevel();

        // Start fading in
        yield return StartCoroutine(_fade.FadeIn());
    }

    private IEnumerator LevelRestart()
    {
        // Start fading out
        yield return StartCoroutine(_fade.FadeOut());

        // Construct level
        ConstructLevel();

        // Start fading in
        yield return StartCoroutine(_fade.FadeIn());
    }

    private void _Hit(Vector2 pos)
    {
        Rect click = new Rect();

        foreach (Rect err in new List<Rect>(_errorsFound.Keys))
        {
            if (err.Contains(pos))
            {
                if (!_errorsFound[err])
                {
                    _errorsFound[err] = true;
                    click.width = click.height = Mathf.Min(err.height, err.width);
                    click.center = err.center;
                    _errorsLeft--;

                    _audioSource.clip = hitSound;
                    _audioSource.Play();
                }
                return;
            }
        }
        click.width = click.height = clickSize;
        click.center = pos;

        if (!_misses.ContainsKey(click))
            _misses.Add(click, 2f);

        _audioSource.clip = missSound;
        _audioSource.Play();
    }
    #endregion
}
