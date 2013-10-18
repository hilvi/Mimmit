using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class PaintToolbar
{
	public PaintBrush CurrentBrush { get; set; }
	
	#region PRIVATE
	private ColoringGameManager _manager;
	
	private Rect _toolbarRegion;		// 780,20,160,560
	private Rect _eraseToolRegion;		// 800,40,120,120
	private Rect _resetToolRegion;		// 800,180,120,120
	private Rect _saveToolRegion;		// 
	private Rect[] _colorPalletteRegion;	// anchor: 800,320 size:60,60
	
	private Dictionary<int, PaintBrush> _colorPallette = new Dictionary<int, PaintBrush>(); 
	#endregion
	
	public PaintToolbar (ColoringGameManager manager, Rect region, 
		Vector2 paletteAnchor, Vector2 buttonInset) {
		_manager = manager;
		_toolbarRegion = region;

		_eraseToolRegion = new Rect(820,120,80,80);
		_resetToolRegion = new Rect(820,220,80,80);
		_saveToolRegion = new Rect(800,40,40,40);
		
		// TODO, possibly make more elegant, ugly constants
		_colorPalletteRegion = new Rect[8];
		for (int y = 0; y < 4; y++) {
			for (int x = 0; x < 2; x++)  {
				float xx = (paletteAnchor.x + x * 60f) + (buttonInset.x / 2f);
				float yy = (paletteAnchor.y + y * 60f) + (buttonInset.y / 2f);
				_colorPalletteRegion[y * 2 + x] = 
					new Rect(xx, yy, 60f - buttonInset.x, 60f - buttonInset.y);
			}
		}
		
		// Setup color pallette. Start from top-left, row-major order
		_colorPallette.Add(-1, new PaintBrush("Erase", Color.white));
		_colorPallette.Add(0, new PaintBrush("Blue", Color.blue));
		_colorPallette.Add(1, new PaintBrush("Magenta", Color.magenta));
		_colorPallette.Add(2, new PaintBrush("Cyan", Color.cyan));
		_colorPallette.Add(3, new PaintBrush("Red", Color.red));
		_colorPallette.Add(4, new PaintBrush("Green", Color.green));
		_colorPallette.Add(5, new PaintBrush("Yellow", Color.yellow));
		_colorPallette.Add(6, new PaintBrush("Grey", new Color(0.330f, 0.330f, 0.330f)));
		_colorPallette.Add(7, new PaintBrush("Orange", new Color(1.000f, 0.500f, 0.000f)));
		
		CurrentBrush = _colorPallette[0]; // Set default brush 
	}

	public void OnGUI() {
		GUI.Box(_toolbarRegion, "toolbar");
		GUI.Box(_eraseToolRegion, "eraseTool");
		GUI.Box(_resetToolRegion, "resetTool");
		GUI.Box(_saveToolRegion, "save");

		for (int i = 0; i < _colorPalletteRegion.Length; i++) {
			GUI.Box(_colorPalletteRegion[i], _colorPallette[i].name);
		}
	}
	
	public void HandleMouse(Vector2 position) {
		if (_eraseToolRegion.Contains(position)) {
			// Select erase tool
			CurrentBrush = _colorPallette[-1];
			Debug.Log ("selected"+CurrentBrush.name);
		}
		
		if (_resetToolRegion.Contains(position)) {
			_manager.ResetPictureToOriginal();		
			Debug.Log ("selected reset tool");
		}
		
		if (_saveToolRegion.Contains(position)) {
			// TODO, open save dialog
			_SavePicture();
			Debug.Log ("Save picture");
		}
		
		// Color pallette
		for (int i = 0; i < _colorPalletteRegion.Length; i++) {
			if (_colorPalletteRegion[i].Contains(position)) {
				// Set new brush
				CurrentBrush = _colorPallette[i];
				
				Debug.Log ("selected"+_colorPallette[i].name);
			}
		}
	}
	
	private void _SavePicture() {
		// Removed for now since we should use some Js methods instead
		/*Texture2D __t = _manager.GetPictureFromFrame();
		
		if (__t ==  null)
			return;
		
		string __path = EditorUtility.SaveFilePanel(
			"Save picture as PNG",
			"",
			"picture.png",
			"png");
		

		if (__path.Length != 0) {
			byte[] __pngBytes = __t.EncodeToPNG();
			//File.WriteAllBytes(__path, __pngBytes); // Make sure PC platform is chosen to use this
			Debug.Log(__path);
		}*/
	}
}

