using System;
using UnityEngine;
using System.Collections.Generic;

public class PictureSelector
{
	#region MEMBERS
	private ColoringGameManager _manager;
#if UNITY_EDITOR
	private Rect _pictureSelectRegion;		// 20,200,160,380
#endif
	private Rect _selectUpBtnRegion;		// 20,200,160,40
	private Rect _selectDownBtnRegion;		// 20,540,160,40
	private Rect[] _selectPictureRegion;	// 
	
	private int _pictureIndexOffset = 0;
	private const int _pictureCount = 10;
	private List<string> _pictureNames = new List<string>();
	private Texture2D[] _thumbnails;
	private Texture2D _upArrowTexture;
	private Texture2D _downArrowTexture;
	#endregion
	
	#region UNITY_METHODS
	public void OnGui() {
		#if UNITY_EDITOR
		GUI.Box(_pictureSelectRegion, "pictureSelect");
		for (int i = 0; i < 3; i++) {
			GUI.Box(_selectPictureRegion[i], _pictureNames[_pictureIndexOffset + i]);	
		}
		#endif
		
		for (int i = 0; i < 3; i++) {
			GUI.DrawTexture(_selectPictureRegion[i], _thumbnails[_pictureIndexOffset + i]);	
		}
		
		GUI.DrawTexture(_selectUpBtnRegion, _upArrowTexture);
		GUI.DrawTexture(_selectDownBtnRegion, _downArrowTexture);
	}
	#endregion
	
	#region METHODS
	public PictureSelector (ColoringGameManager manager, Rect region, Texture2D[] thumbnails,
		Texture2D upArrowTexture, Texture2D downArrowTexture) 
	{
#if UNITY_EDITOR
		_pictureSelectRegion = region;
		_pictureSelectRegion = new Rect(20,200,160,380);
#endif
		_manager = manager;
		_thumbnails = thumbnails;
		_upArrowTexture = upArrowTexture;
		_downArrowTexture = downArrowTexture;

		_selectUpBtnRegion = new Rect(100, 100, 40, 30);
		_selectDownBtnRegion = new Rect(100, 565, 40, 30);
		
		for (int i = 0; i < _pictureCount; i++) 
		{
			_pictureNames.Add("Picture"+i.ToString());
		}
		
		// TODO, possibly make more elegant, ugly constants and non-integer values
		float __v = 140; // Vertical coordinate
		_selectPictureRegion = new Rect[4];

		for (int i = 0; i < 3; i++) 
		{
			_selectPictureRegion[i] = new Rect(50f, __v, 135, 135);
			__v += 140;
		}
		
		// TODO, load picture thumbnails here
	}
	
	public void HandleMouse(Vector2 position) 
	{
		// Navigation buttons
		if (_selectUpBtnRegion.Contains(position)) {
			_pictureIndexOffset--;
		} else if (_selectDownBtnRegion.Contains(position)) {
			_pictureIndexOffset++;
		}
		
		// Prevent index overflow
		_pictureIndexOffset = Mathf.Clamp(_pictureIndexOffset, 0, _pictureNames.Count - 4);
		
		// Picture selection
		for (int i = 0; i < 3; i++) {
			if (_selectPictureRegion[i].Contains(position)) {
				// TODO load new picture
				//Debug.Log("selected picture"+_pictureNames[_pictureIndexOffset + i]);
				_manager.LoadPictureByIndex(_pictureIndexOffset + i);
			}
		}
	}
	public void HandleMouseWheel(int modifier) 
	{
		// Navigation buttons
		_pictureIndexOffset += modifier < 0 ? 1 : -1; 
		// Prevent index overflow
		_pictureIndexOffset = Mathf.Clamp(_pictureIndexOffset, 0, _pictureNames.Count - 3);
		
	}
	#endregion
}



