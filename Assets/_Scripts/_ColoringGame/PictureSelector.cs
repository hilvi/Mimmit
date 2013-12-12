using System;
using UnityEngine;

public class PictureSelector
{
    #region MEMBERS
    // Reference
    private ColoringGameManager _manager;
    // Regions
    private Rect _scrollRegion;
    private Rect _selectUpBtnRegion;
    private Rect _selectDownBtnRegion;
    private Rect[] _selectPictureRegion;

    // Keeps track of which picture is being selected
    private int _pictureIndexOffset = 0;
    private int _visiblePictures = 2;
    // Textures
    private Texture2D[] _thumbnails;
    private Texture2D _upArrowTexture;
    private Texture2D _downArrowTexture;
    // Style for buttons
    private GUIStyle _defaultStyle = new GUIStyle();
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
    public PictureSelector(ColoringGameManager manager, Rect region, Texture2D[] thumbnails,
        Texture2D upArrowTexture, Texture2D downArrowTexture)
    {
        // Set references and textures
        _manager = manager;
        _thumbnails = thumbnails;
        _upArrowTexture = upArrowTexture;
        _downArrowTexture = downArrowTexture;

        // Set regions
        _scrollRegion = region;
        _selectUpBtnRegion = new Rect(90f, 140f, 40f, 30f);
        _selectDownBtnRegion = new Rect(90f, 520f, 40f, 30f);

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



