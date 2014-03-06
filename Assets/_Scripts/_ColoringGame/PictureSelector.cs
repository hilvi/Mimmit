using System;
using UnityEngine;

[System.Serializable]
public class PictureSelector
{
    #region MEMBERS
    #if UNITY_EDITOR
    // Debug controls
    public bool drawRegions;
    #endif
    // Regions
    public Rect _scrollRegion;
    public Rect _selectUpBtnRegion;
    public Rect _selectDownBtnRegion;
    public Rect[] _selectPictureRegion;
    // Keeps track of which picture is being selected
    private int _pictureIndexOffset = 0;
    private int _visiblePictures = 2;
    // Textures
    public Texture2D _upArrowTexture;
    public Texture2D _downArrowTexture;
    private Texture2D[] _thumbnails;
    // Style for buttons
    private GUIStyle _defaultStyle = new GUIStyle();
    // References
    private ColoringGameManager _manager;
    #endregion

    #region UNITY_METHODS
    public void OnGUI()
    {
        #if UNITY_EDITOR
        if (drawRegions)
        {
            GUI.Box(_scrollRegion, "scroll");
            GUI.Box(_selectUpBtnRegion, "up");
            GUI.Box(_selectDownBtnRegion, "down");
            foreach (Rect r in _selectPictureRegion)
            {
                GUI.Box(r, "lol");
            }
        }
        #endif

        // Forbid any interaction when game is not running
        bool __isRunning = (_manager.GetGameState() == GameState.Running);

        // If cursor is within scroll region, enable image scrolling
        if (__isRunning)
        {
            if (_scrollRegion.Contains(Event.current.mousePosition))
            {
                if (Event.current.type == EventType.scrollWheel)
                {
                    float __delta = Event.current.delta.y;
                    if (__delta < 0) 
                    {
                        ScrollImages(ScrollDirection.Up);
                    }
                    else if (__delta > 0)
                    {
                        ScrollImages(ScrollDirection.Down);
                    }
                }
            }
        }

        // Draw thumbnails
        for (int i = 0; i < _visiblePictures; i++)
        {
            if (GUI.Button(_selectPictureRegion[i], _thumbnails[_pictureIndexOffset + i], _defaultStyle))
            {
                if (__isRunning)
                {
                    _manager.LoadPictureByIndex(_pictureIndexOffset + i);
                }
            }
        }

        // Handle button events for scrolling
        if (__isRunning)
        {
            if (GUI.Button(_selectUpBtnRegion, "", _defaultStyle))
            {
                ScrollImages(ScrollDirection.Up);
            }

            if (GUI.Button(_selectDownBtnRegion, "", _defaultStyle))
            {
                 ScrollImages(ScrollDirection.Down);
            }
        }

        // Draw image scrolling buttons
        GUI.DrawTexture(_selectUpBtnRegion, _upArrowTexture);
        GUI.DrawTexture(_selectDownBtnRegion, _downArrowTexture);
    }
    #endregion

    #region METHODS
    public void SetManager(ColoringGameManager manager)
    {
        _manager = manager;
    }

    public void SetThumbnails(Texture2D[] thumbnails)
    {
        _thumbnails = thumbnails;
    }

    public void Initialize()
    {
        // Set default style
        _defaultStyle.alignment = TextAnchor.MiddleCenter;
    }

    public enum ScrollDirection { Up, Down }
    public void ScrollImages(ScrollDirection direction)
    {
        if (direction == ScrollDirection.Up)
            _pictureIndexOffset--;
        else
            _pictureIndexOffset++;

        // Prevent index overflow
        _pictureIndexOffset = Mathf.Clamp(_pictureIndexOffset, 0, _thumbnails.Length - _visiblePictures);
    }
    #endregion
}



