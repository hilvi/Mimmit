//#define DEVELOPER_MODE

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColoringGameManager : GameManager
{
    #region MEMBERS
    // Textures
    /*
     * CachedPictures will be read/write unlocked, so we always 
     * have to make a copy before modifying any of them.
     */
    public Texture2D[] cachedPictures;
    public Texture2D cursor;
    // Audio objects
    public GameObject musicObject;
    public AudioClip music;
    public AudioClip paintSound;
    public AudioClip chooseEraserSound;
    public AudioClip chooseColorSound;
    // References
    public PictureSelector _pictureSelector;
    public PaintFrame _frame;
    public PaintToolbar _toolbar;
    // Used to reference current active picture
    private int _currentPictureIndex = 0;
    // Custom cursor
    private Vector2 _hotSpot = Vector2.zero;
    private CursorMode _cursorMode = CursorMode.Auto;
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

            // Reduce volume of bg music, so other effects can be heard.
            InGameMenuGUI.music.audio.volume = 0.5f;
        }

        // Initialize picture selector
        _pictureSelector.SetManager(this);
        _pictureSelector.SetThumbnails(cachedPictures);
        _pictureSelector.Initialize();

        // Initialize frame
        //_frame = new PaintFrame(pictureRegion, cachedPictures[0]);
        _frame.VolatilePicture = cachedPictures[0];

        // Initialize toolbar
        _toolbar.SetManager(this);
        _toolbar.colorRegionArray = colorArray;
        _toolbar.colorArray = colArray;
        _toolbar.ChangeColor(colArray[0], colorArray[0]);
    }

    void Update()
    {
        // If game is not running, ignore any action
        if (GetGameState() != GameState.Running)
            return;

        if (Input.GetMouseButtonDown(0))
            _HandleMouseClick();
    }

    /* Yes, these names are really bad. Didn't think it through.
     * DO NOT rename, it will break 36 settings carefully set in inspector.
     */
    public Rect[] colorArray;  // Should be colorRegionArray
    public Color[] colArray; // Should be colorArray;

    void OnGUI()
    {
        // Render calls for each component 
        _pictureSelector.OnGUI();
        _frame.OnGUI();
        _toolbar.OnGUI();

        // Change cursor texture depending on current region
        if (_frame.pictureRegion.Contains(Event.current.mousePosition))
            Cursor.SetCursor(null, _hotSpot, _cursorMode);
        else
            Cursor.SetCursor(cursor, _hotSpot, _cursorMode);
    }
    #endregion

    #region METHODS
    public void ResetPictureToOriginal()
    {
#if DEVELOPER_MODE
		_frame.VolatilePicture = _CreateDebugGridTexture(560, 560, 40, 40);
#else
        _frame.VolatilePicture = cachedPictures[_currentPictureIndex];
#endif
    }

    public Texture2D GetPictureFromFrame()
    {
        return _frame.VolatilePicture;
    }

    public void LoadPictureByIndex(int index)
    {
        Debug.Log("Loading picture index: " + index.ToString());

        _frame.VolatilePicture = cachedPictures[index];
        _currentPictureIndex = index;
    }

    public void PlayPaintSound()
    {
        audio.clip = paintSound;
        audio.Play();
    }

    public void PlayChooseSound()
    {
        audio.clip = chooseColorSound;
        audio.Play();
    }

    public void PlayEraseSound()
    {
        audio.clip = chooseEraserSound;
        audio.Play();
    }

    private void _HandleMouseClick()
    {
        // Click on the drawing
        Vector3 __p = InputManager.MouseScreenToGUI();
        if (_frame.pictureRegion.Contains(__p))
        {
            _frame.Paint(__p, _toolbar.selectedColor);
            PlayPaintSound();
        }
    }

    private Texture2D _CreateDebugGridTexture(int width, int height, int gridWidth, int gridHeight)
    {
        Texture2D __picture = new Texture2D(width, height);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x % gridWidth == 0 || y % gridHeight == 0)
                    __picture.SetPixel(x, y, Color.black);
                else
                    __picture.SetPixel(x, y, Color.white);
            }
        }
        __picture.Apply();
        return __picture;
    }
    #endregion
}
