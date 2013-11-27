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
	private const int _gridWidth = 3;
	private const int _gridHeight = 6;
	private const float _brushWidth = 66f;
#if UNITY_EDITOR
	private Rect _toolbarRegion;		// 780,20,160,560
#endif
	private Rect _eraseToolRegion;		// 800,40,120,120
	private Rect _resetToolRegion;		// 800,180,120,120
	private Rect _saveToolRegion;		// 
	private Rect[] _colorPalletteRegion;	// anchor: 800,320 size:60,60

	private Dictionary<int, PaintBrush> _colorPallette = new Dictionary<int, PaintBrush> ();
	private Texture2D _eraserTexture;
	private Texture2D _tickTexture;
	private Texture2D _resetTexture;
	private Texture2D _saveTexture;
	#endregion
	
	public PaintToolbar (ColoringGameManager manager, Rect region, 
		Vector2 paletteAnchor, Vector2 buttonInset, 
		Texture2D[] paintBrushTextures, Texture2D eraserTexture,
		Texture2D tickTexture, Texture2D resetTexture,
		Texture2D saveTexture)
	{
		
		_manager = manager;
#if UNITY_EDITOR
		_toolbarRegion = region;
#endif
		_eraserTexture = eraserTexture;
		_tickTexture = tickTexture;
		_resetTexture = resetTexture;
		_saveTexture = saveTexture;
		
		_eraseToolRegion = new Rect (744, 115, 60, 60);
		_resetToolRegion = new Rect (810, 115, 60, 60);
		_saveToolRegion = new Rect (875, 115, 60, 60);
		
	
		// Generate brush grid
		_colorPalletteRegion = new Rect[_gridWidth * _gridHeight];
		for (int y = 0; y < _gridHeight; y++) {
			for (int x = 0; x < _gridWidth; x++) {
				float __xPos = (paletteAnchor.x + x * _brushWidth) + (buttonInset.x / 2f);
				float __yPos = (paletteAnchor.y + y * _brushWidth) + (buttonInset.y / 2f);
				
				_colorPalletteRegion [y * _gridWidth + x] = 
					new Rect (
						__xPos,
						__yPos, 
						_brushWidth - buttonInset.x,
						_brushWidth - buttonInset.y);
			}
		}
		
		// Generate HSL color pallette for each brush
		Color[] __HSLpallette = new Color[_gridWidth * _gridHeight];
		
		__HSLpallette[0] = new Color(0.07f,0.07f,0.07f);				 	
		__HSLpallette[1] = new Color(1f,1f,1f);		
		__HSLpallette[2] = new Color(0.5f,0.5f,0.5f);		
		__HSLpallette[3] = new Color(0f,1f,1f);			
		__HSLpallette[4] = new Color(0f,0f,1f);			
		__HSLpallette[5] = new Color(0f,0f,0.5f);		
		__HSLpallette[6] = new Color(1f,0f,1f);			
		__HSLpallette[7] = new Color(1f,0f,0f);
		__HSLpallette[8] = new Color(0.55f,0f,0f);
		__HSLpallette[9] = new Color(1f,1f,0f);
		__HSLpallette[10] = new Color(1f,0.5f,0f);
		__HSLpallette[11] = new Color(0.65f,0.16f,0.16f);
		__HSLpallette[12] = new Color(0.5f,1f,0f);
		__HSLpallette[13] = new Color(0f,1.0f,0f);
		__HSLpallette[14] = new Color(0f,0.5f,0f);
		__HSLpallette[15] = new Color(1f,0.86f,0.65f);
		__HSLpallette[16] = new Color(0.82f,0.4f,0.1f);
		__HSLpallette[17] = new Color(1f,0.5f,1f);
		
		// Setup the actual color pallette. Start from top-left, row-major order
		_colorPallette.Add (-1, new PaintBrush (-1, "Erase", Color.white));
		for (int i = 0; i < _gridWidth * _gridHeight; i++) {
			// Create a brush
			PaintBrush __pb = new PaintBrush (i, "x", Color.white, paintBrushTextures [0]);
			
			// Select color from pre-generated HSL-pallette
			Color __newColor = __HSLpallette [i];
		
			// Copy template brush texture and re-write its r,g,b values with newly generated HSL values
			Texture2D __t = new Texture2D (paintBrushTextures [0].width, paintBrushTextures [0].height);
			for (int y = 0; y < __t.height; y++) {
				for (int x = 0; x < __t.width; x++) {
					Color __tc = paintBrushTextures [0].GetPixel (x, y);
					__tc.r = __newColor.r;
					__tc.g = __newColor.g;
					__tc.b = __newColor.b;
					__t.SetPixel (x, y, __tc);
				}
			}
			
			// Save texture and add to pallette dictionary
			__t.Apply ();
			__pb.color = __newColor;
			__pb.texture = __t;
			_colorPallette.Add (i, __pb);
		}

		CurrentBrush = _colorPallette [0]; // Set default brush 
	}

	public void OnGUI ()
	{
		#if UNITY_EDITOR
		GUI.Box(_toolbarRegion, "toolbar");
		GUI.Box(_eraseToolRegion, "eraseTool");
		GUI.Box(_resetToolRegion, "resetTool");
		GUI.Box(_saveToolRegion, "save");
		#endif
		
		GUI.DrawTexture (_eraseToolRegion, _eraserTexture);
		GUI.DrawTexture (_resetToolRegion, _resetTexture);
		if (_saveTexture != null)
			GUI.DrawTexture (_saveToolRegion, _saveTexture);
		
		for (int i = 0; i < _colorPalletteRegion.Length; i++) {
			#if UNITY_EDITOR
			GUI.Box(_colorPalletteRegion[i], _colorPallette[i].name);
			#endif
			
			GUI.DrawTexture (_colorPalletteRegion [i], _colorPallette [i].texture);
			
			if (i == CurrentBrush.id) {
				// Offset tick to bottom right corner and resize
				Rect __r = _colorPalletteRegion [i];
				__r.x += __r.width / 2f;
				__r.y += __r.height / 2f;
				__r.width = __r.width / 2f;
				__r.height = __r.height / 2f;
				
				GUI.DrawTexture (__r, _tickTexture);	
			} else if (CurrentBrush.id == -1) {
				Rect __r = _eraseToolRegion;
				__r.x += __r.width / 2f;
				__r.y += __r.height / 2f;
				__r.width = __r.width / 2f;
				__r.height = __r.height / 2f;
				
				GUI.DrawTexture (__r, _tickTexture);	
			}
		}
	}
	
	public void HandleMouse (Vector2 position)
	{
		if (_eraseToolRegion.Contains (position)) 
		{
			// Selects erase tool (id = -1)
			CurrentBrush = _colorPallette [-1];
			CurrentBrush.eraser = true;
		}
		
		if (_resetToolRegion.Contains (position)) 
		{
			_manager.ResetPictureToOriginal ();		
		}
		
		if (_saveToolRegion.Contains (position)) 
		{
			_SavePicture ();
		}
		
		// Color pallette
		for (int i = 0; i < _colorPalletteRegion.Length; i++) {
			if (_colorPalletteRegion [i].Contains (position)) {
				// Set new brush
				CurrentBrush = _colorPallette [i];
				CurrentBrush.eraser = false;
			}
		}
	}
	
	private void _SavePicture ()
	{
		Texture2D __t = _manager.GetPictureFromFrame ();
		if (__t == null)
			return;
		
		// Encode texture to byte array
		byte[] __pngData = __t.EncodeToPNG ();
		
		// Further encode that to Base64 (html-friendly format)
		string __b64 = EncodePNGToBase64 (__pngData);
		
		/* Invoke JS-function showImageWindow()
		 * Example implementation thanks to Petri:
		 * 
         * <script type="text/javascript">
		 *   function showImageWindow(imageData) {
	     *   var win =  window.open("", "", "width=600, height=600");
	     *   win.document.body.innerHTML = '<img src="' + imageData + '">';
	     * }
	     *  </script>
	     */
		Application.ExternalCall ("showImageWindow", __b64);
	}
	
	private string EncodePNGToBase64 (byte[] pngData)
	{
		return "data:image/png;base64," +
    		Convert.ToBase64String (pngData, Base64FormattingOptions.None);
	}
	
	private bool IsBitSet (byte b, int pos)
	{
		return (b & (1 << pos)) != 0;
	}
}

