using System;
using UnityEngine;

[System.Serializable]
public class PictureSelector
{
    #region MEMBERS
    // Regions
    public Rect _scrollRegion;
    public Rect _selectUpBtnRegion;
    public Rect _selectDownBtnRegion;
    private Rect[] _selectPictureRegion;
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
        // If cursor is within scroll region, enable image scrolling
        if (_scrollRegion.Contains(Event.current.mousePosition))
        {
            if (Event.current.type == EventType.scrollWheel)
            {
                float __delta = Event.current.delta.y;
                if (__delta < 0)
                    ScrollImages(ScrollDirection.Up);
                else if (__delta > 0)
                    ScrollImages(ScrollDirection.Down);
            }
        }

        // Draw thumbnails
        for (int i = 0; i < _visiblePictures; i++)
        {
            if (GUI.Button(_selectPictureRegion[i], _thumbnails[_pictureIndexOffset + i], _defaultStyle))
                _manager.LoadPictureByIndex(_pictureIndexOffset + i);
        }

        // Draw image scrolling buttons
        if (GUI.Button(_selectUpBtnRegion, _upArrowTexture, _defaultStyle))
            ScrollImages(ScrollDirection.Up);
        if (GUI.Button(_selectDownBtnRegion, _downArrowTexture, _defaultStyle))
            ScrollImages(ScrollDirection.Down);
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
        // Set visible thumbnails
        float __v = 180; // Vertical coordinate
        _selectPictureRegion = new Rect[2];
        for (int i = 0; i < 2; i++)
        {
            _selectPictureRegion[i] = new Rect(40f, __v, 140f, 160f);
            __v += 170;
        }

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



