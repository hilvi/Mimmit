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
}

public class DiffGameManager : GameManager
{
    #region MEMBERS
    // Textures
    public Texture2D tick;
    public Texture2D cross;
    public Texture2D originalPicture;
    public Texture2D errorPicture;
    private Texture2D _borderTexture;
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
    private Rect _leftFrame, _rightFrame;
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
        }
        _audioSource = GetComponent<AudioSource>();

        // Set references
        _backgroundTexture = GameObject.Find("Background").GetComponent<GUITexture>();

        // Init regions
        _leftFrame = new Rect(20f, 20f + (560f / 2.25f) - (321f / 2f), 450f, 321f);
        _rightFrame = _leftFrame;
        _rightFrame.x += _leftFrame.width + 20f;

        // Create border texture
        _borderTexture = _CreateBorderTexture(450, 321, Color.black, 10);

        // Init counter
        _counter = GetComponent<CircularCounterScript>();
        _counter.SetPosition(Screen.width / 2f - 50f, 40f);

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

        // Init background
        _backgroundTexture.texture = pictureObjects[currentLevel].background;

        // For debugging purposes, generate fake errors 
        for (int i = 0; i < pictureObjects.Length; i++)
        {
            if (pictureObjects[i].errors.Length == 0)
            {
                pictureObjects[i].errors = new Rect[7];
                for (int j = 0; j < 7; j++)
                {
                    Rect __r = new Rect();
                    __r.x = 490f + j * 50f;
                    __r.y = 120f;
                    __r.width = 50f;
                    __r.height = 50f;
                    pictureObjects[i].errors[j] = __r;
                }
            }
        }

        // Init errors
        _errorsLeft = 7;
        foreach (Rect rect in pictureObjects[currentLevel].errors)
            _errorsFound.Add(rect, false);
    }

    void Update()
    {
        // Check if player found an error
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = InputManager.MouseScreenToGUI();
            if (GetGameState() != GameState.Paused &&
                GetGameState() != GameState.Won &&
                _rightFrame.Contains(pos))
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
        GUI.DrawTexture(_leftFrame, pictureObjects[currentLevel].originalPicture);
        GUI.DrawTexture(_rightFrame, pictureObjects[currentLevel].errorPicture);
        // Draw borders
        GUI.DrawTexture(_leftFrame, _borderTexture);
        GUI.DrawTexture(_rightFrame, _borderTexture);

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
        int __index = 0;
        foreach (var o in pictureObjects[currentLevel].errors)
            GUI.Box(o, (__index++).ToString());
        #endif

        // Draw circular counter
        _counter.Draw();
    }
    #endregion

    #region METHODS
    public override void GoToNextLevel()
    {
        // Begin level transition
        StartCoroutine(LevelTransition());
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

        // Swap background
        _backgroundTexture.texture = pictureObjects[currentLevel].background;

        // Reload all errors
        _errorsFound.Clear();
        foreach (Rect r in pictureObjects[currentLevel].errors)
        {
            _errorsFound.Add(r, false);
        }

        // Empty misses
        _misses.Clear();

        // Reset error counter
        _errorsLeft = 7;

        // Reset state
        SetGameState(GameState.Running);

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
        _misses.Add(click, 2f);

        _audioSource.clip = missSound;
        _audioSource.Play();
    }

    private Texture2D _CreateBorderTexture(int width, int height, Color borderColor, int borderThickness)
    {
        Texture2D __newTexture = new Texture2D(width, height);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (y < borderThickness ||
                    y > height - borderThickness ||
                    x < borderThickness ||
                    x > width - borderThickness)
                {
                    __newTexture.SetPixel(x, y, borderColor);
                }
                else
                {
                    __newTexture.SetPixel(x, y, Color.clear);
                }
            }
        }

        __newTexture.Apply();
        return __newTexture;
    }
    #endregion
}
