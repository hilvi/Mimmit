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
	private Texture2D _eraserTexture;
	#endregion
	
	public PaintToolbar (ColoringGameManager manager, Rect region, 
		Vector2 paletteAnchor, Vector2 buttonInset, 
		Texture2D[] paintBrushTextures, Texture2D eraserTexture) {
		_manager = manager;
		_toolbarRegion = region;
		_eraserTexture = eraserTexture;

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
		for (int i = 0; i <= 7; i++) {
			_colorPallette.Add(i, new PaintBrush("x", PaintBrush.customPallette[i], paintBrushTextures[i]));
		}

		CurrentBrush = _colorPallette[0]; // Set default brush 
	}

	public void OnGUI() {
		#if UNITY_EDITOR
		GUI.Box(_toolbarRegion, "toolbar");
		GUI.Box(_eraseToolRegion, "eraseTool");
		GUI.Box(_resetToolRegion, "resetTool");
		GUI.Box(_saveToolRegion, "save");
		#endif
		
		GUI.DrawTexture(_eraseToolRegion, _eraserTexture);
		
		for (int i = 0; i < _colorPalletteRegion.Length; i++) {
			#if UNITY_EDITOR
			GUI.Box(_colorPalletteRegion[i], _colorPallette[i].name);
			#endif
			
			GUI.DrawTexture(_colorPalletteRegion[i], _colorPallette[i].texture);			
		}
	}
	
	public void HandleMouse(Vector2 position) {
		if (_eraseToolRegion.Contains(position)) {
			// Select erase tool
			CurrentBrush = _colorPallette[-1];
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
			}
		}
	}
	
	private void _SavePicture() {
		Texture2D __t = _manager.GetPictureFromFrame();
		if (__t == null)
			return;
		
		// Encode texture to byte array
		byte[] __pngData = __t.EncodeToPNG();
		
		// Further encode that to Base64 (html-friendly format)
		string __b64 = EncodePNGToBase64(__pngData);
		
		/* Invoke JS-function showImageWindow()
		 * Example implementation thanks to Petri:
		 * 
         *	<script type="text/javascript">
	     *      function showImageWindow(imageData) {
	     *          var win =  window.open("", "", "width=600, height=600");
	     *          win.document.body.innerHTML = '<img src="' + imageData + '">';
	     *      }
	     *  </script>
	     */
		Application.ExternalCall("showImageWindow", __b64);
	}
	
	private string EncodePNGToBase64(byte[] pngData) {
  		return "data:image/png;base64," +
    		Convert.ToBase64String(pngData, Base64FormattingOptions.None);
	}
}

