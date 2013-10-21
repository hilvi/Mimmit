using System;
using UnityEngine;
using System.Collections.Generic;

public class PictureSelector
{
	#region PRIVATE
	private ColoringGameManager _manager;
	
	private Rect _pictureSelectRegion;		// 20,200,160,380
	private Rect _selectUpBtnRegion;		// 20,200,160,40
	private Rect _selectDownBtnRegion;		// 20,540,160,40
	private Rect[] _selectPictureRegion;	// 
	
	private int _pictureIndexOffset = 0;
	private List<string> _pictureNames = new List<string>();
	#endregion
	
	public PictureSelector (ColoringGameManager manager, Rect region) {
		_pictureSelectRegion = region;
		
		_pictureSelectRegion = new Rect(20,200,160,380);
		_selectUpBtnRegion = new Rect(20,200,160,40);
		_selectDownBtnRegion = new Rect(20,540,160,40);
		
		for (int i = 1; i <= 10; i++) {
			_pictureNames.Add("Picture"+i.ToString());
		}
		
		// TODO, possibly make more elegant, ugly constants and non-integer values
		float __v = 252.5f; // Vertical coordinate
		_selectPictureRegion = new Rect[4];
		for (int i = 0; i < 4; i++) {
			_selectPictureRegion[i] = new Rect(67.5f, __v, 65f, 65f);
			__v += 70f;
		}
		
		// TODO, load picture thumbnails here
	}
	
	public void OnGui() {
		GUI.Box(_pictureSelectRegion, "pictureSelect");
		GUI.Box(_selectUpBtnRegion, "up");
		GUI.Box(_selectDownBtnRegion, "down");
		for (int i = 0; i < 4; i++) {
			GUI.Box(_selectPictureRegion[i], _pictureNames[_pictureIndexOffset + i]);	
		}
	}
	
	public void HandleMouse(Vector2 position) {
		// Navigation buttons
		if (_selectUpBtnRegion.Contains(position)) {
			_pictureIndexOffset--;
		} else if (_selectDownBtnRegion.Contains(position)) {
			_pictureIndexOffset++;
		}
		
		// Prevent index overflow
		_pictureIndexOffset = Mathf.Clamp(_pictureIndexOffset, 0, _pictureNames.Count - 4);
		
		// Picture selection
		for (int i = 0; i < 4; i++) {
			if (_selectPictureRegion[i].Contains(position)) {
				// TODO load new picture
				Debug.Log("selected picture"+_pictureNames[_pictureIndexOffset + i]);
			}
		}
	}
}



