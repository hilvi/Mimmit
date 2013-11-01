using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Collections;

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
	private Texture2D _tickTexture;
	private Texture2D _resetTexture;
	private Texture2D _saveTexture;
	#endregion
	
	public PaintToolbar (ColoringGameManager manager, Rect region, 
		Vector2 paletteAnchor, Vector2 buttonInset, 
		Texture2D[] paintBrushTextures, Texture2D eraserTexture,
		Texture2D tickTexture, Texture2D resetTexture,
		Texture2D saveTexture) {
		
		_manager = manager;
		_toolbarRegion = region;
		_eraserTexture = eraserTexture;
		_tickTexture = tickTexture;
		_resetTexture = resetTexture;
		_saveTexture = saveTexture;
		
		_saveToolRegion = new Rect(810,120,100,100);
		
		_eraseToolRegion = new Rect(790,240,60,60);
		_resetToolRegion = new Rect(870,240,60,60);
		
		// TODO, possibly make more elegant, ugly constants
		const int __gridWidth = 4, __gridHeight = 8;
		_colorPalletteRegion = new Rect[__gridWidth * __gridHeight];
		const float __cellWidth = 30f;
		for (int y = 0; y < __gridHeight; y++) {
			for (int x = 0; x < __gridWidth; x++)  {
				float xx = (paletteAnchor.x + x * __cellWidth) + (buttonInset.x / 2f);
				float yy = (paletteAnchor.y + y * __cellWidth) + (buttonInset.y / 2f);
				_colorPalletteRegion[y * __gridWidth + x] = 
					new Rect(xx, yy, __cellWidth - buttonInset.x, __cellWidth - buttonInset.y);
			}
		}

		byte bit = 0x00;

		// Setup color pallette. Start from top-left, row-major order
		_colorPallette.Add(-1, new PaintBrush(-1, "Erase", Color.white));
		for (int i = 0; i < __gridWidth * __gridHeight; i++) {
			PaintBrush __pb = new PaintBrush(i, "x", PaintBrush.customPallette[0], paintBrushTextures[0]);

			float incr = (1f - 0.1f) / __gridWidth;
			float r = 0.1f, g = 0.1f, b = 0.1f;
			
			if (IsBitSet(bit, 2) == false) {
				r = incr;
				r += Mathf.Clamp01(incr * (i % __gridWidth));
			}
			if (IsBitSet(bit, 1) == false) {
				g = incr;
				g += Mathf.Clamp01(incr * (i % __gridWidth));
			}
			if (IsBitSet(bit, 0) == false) {
				b = incr;
				b += Mathf.Clamp01(incr * (i % __gridWidth));
			}

			if ((i+1) % __gridWidth == 0) {
				bit += 1;
			}

			Texture2D __t = new Texture2D(paintBrushTextures[0].width, paintBrushTextures[0].height);
			for (int y = 0; y < __t.height; y++) {
				for (int x = 0; x < __t.width; x++) {
					Color __tc = paintBrushTextures[0].GetPixel(x, y);
					__tc.r = r;
					__tc.g = g;
					__tc.b = b;
					__t.SetPixel(x, y, __tc);
				}
			}
			__t.Apply();
			
			__pb.color = new Color(r, g, b);
			__pb.texture = __t;
			_colorPallette.Add(i, __pb);
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
		GUI.DrawTexture(_resetToolRegion, _resetTexture);
		if(_saveTexture != null)
			GUI.DrawTexture(_saveToolRegion, _saveTexture);
		
		for (int i = 0; i < _colorPalletteRegion.Length; i++) {
			#if UNITY_EDITOR
			GUI.Box(_colorPalletteRegion[i], _colorPallette[i].name);
			#endif
			
			GUI.DrawTexture(_colorPalletteRegion[i], _colorPallette[i].texture);
			
			if (i == CurrentBrush.id) {
				// Offset tick to bottom right corner and resize
				Rect __r = _colorPalletteRegion[i];
				__r.x += __r.width / 2f;
				__r.y += __r.height / 2f;
				__r.width = __r.width / 2f;
				__r.height = __r.height / 2f;
				
				GUI.DrawTexture(__r, _tickTexture);	
			} else if (CurrentBrush.id == -1) {
				Rect __r = _eraseToolRegion;
				__r.x += __r.width / 2f;
				__r.y += __r.height / 2f;
				__r.width = __r.width / 2f;
				__r.height = __r.height / 2f;
				
				GUI.DrawTexture(__r, _tickTexture);	
			}
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
	
	private bool IsBitSet(byte b, int pos) {
		return (b & (1 << pos)) != 0;
	}
}

