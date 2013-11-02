using UnityEngine;
using System.Collections;

public class DiffGamePictureAnalyzer : MonoBehaviour
{
	#region MEMBERS
	public Texture2D[] originalPictures;
	public Texture2D[] errorPictures;
	private Rect _leftFrame, _rightFrame;
	private Texture2D[] _modifiedOriginalPictures;
	private Texture2D[] _modifiedErrorPictures;
	private Texture2D[] _differenceMaps;
	private int _levelIndex = 0;
	private bool _cheatModeEnabled = false;
	#endregion
	
	#region UNITY_METHODS
	void Start ()
	{
		_modifiedOriginalPictures = new Texture2D[originalPictures.Length];
		_modifiedErrorPictures = new Texture2D[errorPictures.Length];
		
		Color __borderColor = Color.black;
		int __borderThickness = 8;
		for (int i = 0; i < _modifiedOriginalPictures.Length; i++) {
			_modifiedOriginalPictures [i] = 
				_CreateBorders (originalPictures [i], __borderColor, __borderThickness);
			_modifiedErrorPictures [i] = 
				_CreateBorders (errorPictures [i], __borderColor, __borderThickness);
		}
		
		#if UNITY_EDITOR
		_differenceMaps = new Texture2D[originalPictures.Length];
		for (int i = 0; i < _differenceMaps.Length; i++) {
			_differenceMaps[i] = _ComputeDifferenceMap(_modifiedOriginalPictures[i], _modifiedErrorPictures[i]);
		}
		#endif

		_leftFrame = new Rect (20f, 20f + (560f / 2f) - (321f / 2f), 450f, 321f);
		
		_rightFrame = _leftFrame;
		_rightFrame.x += _leftFrame.width + 20f;
	}

	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.RightArrow)) {
			_levelIndex++;
			if (_levelIndex >= originalPictures.Length) {
				_levelIndex = 0;
			}
		} else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			_levelIndex--;
			if (_levelIndex < 0)
				_levelIndex = originalPictures.Length - 1;
		}
		
		#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.Space)) {
			_cheatModeEnabled = !_cheatModeEnabled;
		}
		#endif
	}
	
	void OnGUI ()
	{
		GUI.DrawTexture (_leftFrame, _modifiedOriginalPictures [_levelIndex]);
		GUI.DrawTexture (_rightFrame, _modifiedErrorPictures [_levelIndex]);
		
		#if UNITY_EDITOR
		if (_cheatModeEnabled)
			GUI.DrawTexture(_rightFrame, _differenceMaps[_levelIndex]);
		#endif
	}
	#endregion
	
	#region METHODS
	private Texture2D _ComputeDifferenceMap (Texture2D a, Texture2D b)
	{
		if (a.width != b.width || a.height != b.height) {
			Debug.Log ("Both pictures have to be same size");
			return null;
		}
		
		Texture2D __diffMap = new Texture2D (a.width, a.height);
		Color[] __aPixels = a.GetPixels ();
		Color[] __bPixels = b.GetPixels ();
		Color[] __diffPixels = new Color[__aPixels.Length];
		
		for (int i = 0; i < __aPixels.Length; i++) {
			Color __aColor = __aPixels [i];
			Color __bColor = __bPixels [i];
			
			float __difference = Vector4.Distance (__aColor, __bColor);
			if (__difference < 0.1f) {
				// Pixels are practically the same
				__diffPixels [i] = Color.clear;
			} else {
				// Pixels are very much different
				__diffPixels [i] = (Random.value < 0.5f) ? Color.magenta : Color.white;
				//__diffPixels [i] = __aColor;
			}
		}
		
		__diffMap.SetPixels (__diffPixels);
		__diffMap.Apply ();
		return __diffMap;
	}
	
	private Texture2D _CreateBorders (Texture2D a, Color borderColor, int borderThickness)
	{
		Texture2D __newTexture = new Texture2D (a.width, a.height);
		for (int y = 0; y < a.height; y++) {
			for (int x = 0; x < a.width; x++) {
				if (y < borderThickness || 
					y > a.height - borderThickness ||
					x < borderThickness ||
					x > a.width - borderThickness) {
					__newTexture.SetPixel (x, y, borderColor);
				} else {
					__newTexture.SetPixel (x, y, a.GetPixel (x, y));
				}
			}
		}
		
		__newTexture.Apply ();
		return __newTexture;
	}
	#endregion
}
