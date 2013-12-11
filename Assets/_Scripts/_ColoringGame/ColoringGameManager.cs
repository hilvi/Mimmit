//#define DEVELOPER_MODE

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColoringGameManager : GameManager
{

    #region PUBLIC
    public Rect pictureSelectRegion;	// 20,200,160,380
    public Rect pictureRegion;			// 200,20,560,560

    public GameObject musicObject;
    public AudioClip music;

    /*
     * These pictures will be read/write unlocked, so we always 
     * have to make a copy before modifying any of them.
     */
    public Texture2D[] cachedPictures;
    // Textures
    public Texture2D tickTexture;
    public Texture2D upArrowTexture;
    public Texture2D downArrowTexture;
    public Texture2D cursor;
    // Audioclips
    public AudioClip clickColor;
    public AudioClip clickErase;
    public AudioClip clickButton;
    #endregion

    #region PRIVATE
    private PictureSelector _pictureSelector;
    private PaintFrame _frame;
    private PaintToolbar _toolbar;

    private int _currentPictureIndex = 0;
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
        }

        // Initialize picture selector
        _pictureSelector = new PictureSelector(this, pictureSelectRegion, cachedPictures,
            upArrowTexture, downArrowTexture);

        // Initialize frame
        _frame = new PaintFrame(this, pictureRegion, cachedPictures[0]);

        // Initialize toolbar
        _toolbar = new PaintToolbar(this);
        _toolbar.colorRegionArray = colorArray;
        _toolbar.eraseToolRegion = new Rect(787, 344, 50, 50);
        _toolbar.resetToolRegion = new Rect(847, 344, 50, 50);
        _toolbar.saveToolRegion = new Rect(904, 344, 50, 50);
        _toolbar.colorArray = colArray;
        _toolbar.ChangeColor(colArray[0], colorArray[0]);
        _toolbar.tickTexture = tickTexture;
    }

    void Update()
    {
        // If game is not running, ignore any action
        if (GetGameState() != GameState.Running)
            return;

        if (Input.GetMouseButtonDown(0))
            _HandleMouseClick();

        if (pictureSelectRegion.Contains(InputManager.MouseScreenToGUI()))
        {
            float __mouse = Input.GetAxis("Mouse ScrollWheel");
            if (__mouse > 0)
            {
                _pictureSelector.HandleMouseWheel(1);
            }
            else if (__mouse < 0)
            {
                _pictureSelector.HandleMouseWheel(-1);
            }
        }
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
        if (pictureRegion.Contains(Event.current.mousePosition))
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

    public void PlayClickSound()
    {
        audio.clip = clickColor;
        audio.volume = 0.5f;
        audio.Play();
    }

    public void PlayEraseSound()
    {
        audio.clip = clickErase;
        audio.volume = 0.5f;
        audio.Play();
    }

    private void _HandleMouseClick()
    {
        Vector3 __p = InputManager.MouseScreenToGUI();

        // Click on the preview section
        if (pictureSelectRegion.Contains(__p))
        {
            PlayClickSound();
            _pictureSelector.HandleMouse(__p);
        }
        // Click on the drawing
        else if (pictureRegion.Contains(__p))
        {
            _frame.Paint(__p, _toolbar.selectedColor);
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
