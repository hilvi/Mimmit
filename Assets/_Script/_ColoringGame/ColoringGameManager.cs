using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColoringGameManager : GameManager {
	
	public Rect chosenCharRegion;		// 20,20,160,160

	public Rect pictureSelectRegion;	// 20,200,160,380
	public Rect selectUpBtnRegion;		// 20,200,160,40
	public Rect selectDownBtnRegion;	// 20,540,160,40
	public Rect[] selectPictureRegion;	// 
	private int _pictureIndexOffset;
	private List<string> _pictureNames;

	public Rect pictureRegion;			// 200,20,560,560
	private Texture2D _picture;

	public Rect toolbarRegion;			// 780,20,160,560
	public Rect eraseToolRegion;		// 800,40,120,120
	public Rect undoToolRegion;			// 800,180,120,120
	public Rect[] colorPalletteRegion;	// anchor: 800,320 size:60,60
	
	private class PaintBrush {
		public string name;
		public Color color;
		
		public PaintBrush(string name, Color color) {
			this.name = name;
			this.color = color;
		}
	}
	
	private Dictionary<int, PaintBrush> _colorPallette; // start from top-left, row order
	private PaintBrush _currentBrush;
	
	public override void Start () {
		base.Start();
		SetGameState(GameState.Running);

		#region DEBUG_GRID_TEXTURE
		_picture = new Texture2D(560, 560);
		for (int x = 0; x < 560; x++) {
			for (int y = 0; y < 560; y++) {
				if (x % 200 == 0 || y % 200 == 0)
					_picture.SetPixel(x, y, Color.black);
				else
					_picture.SetPixel(x, y, Color.white);
			}
		}
		_picture.Apply();
		#endregion
		#region DEBUG_GEN_PICTURE_NAMES
		_pictureIndexOffset = 0;
		_pictureNames = new List<string>();
		for (int i = 1; i <= 10; i++) {
			_pictureNames.Add("Picture"+i.ToString());
		}
		#endregion
		#region DEBUG_PICTURE_SELECTION_LAYOUT
		// TODO, possibly make more elegant, ugly constants and non-integer values
		float vertical = 252.5f;
		selectPictureRegion = new Rect[4];
		for (int i = 0; i < 4; i++) {
			selectPictureRegion[i] = new Rect(67.5f, vertical, 65f, 65f);
			vertical += 70f;
		}
		#endregion
		#region DEBUG_COLOR_PALLETTE_LAYOUT
		// TODO, possibly make more elegant, ugly constants
		colorPalletteRegion = new Rect[8];
		Vector2 paletteAnchor = new Vector2(800f, 320f);
		Vector2 minimize = new Vector2(10f, 10f);
		for (int y = 0; y < 4; y++) {
			for (int x = 0; x < 2; x++)  {
				float xx = (paletteAnchor.x + x * 60f) + (minimize.x/2f);
				float yy = (paletteAnchor.y + y * 60f) + (minimize.y/2f);
				colorPalletteRegion[y*2+x] = new Rect(xx, yy, 60f - minimize.x, 60f - minimize.y);
			}
		}
		
		// Setup colors
		_colorPallette = new Dictionary<int, PaintBrush>();
		_colorPallette.Add(0, new PaintBrush("Blue", Color.blue));
		_colorPallette.Add(1, new PaintBrush("Magenta", Color.magenta));
		_colorPallette.Add(2, new PaintBrush("Cyan", Color.cyan));
		_colorPallette.Add(3, new PaintBrush("Red", Color.red));
		_colorPallette.Add(4, new PaintBrush("Green", Color.green));
		_colorPallette.Add(5, new PaintBrush("Yellow", Color.yellow));
		_colorPallette.Add(6, new PaintBrush("Grey", new Color(0.330f, 0.330f, 0.330f)));
		_colorPallette.Add(7, new PaintBrush("Orange", new Color(1.000f, 0.500f, 0.000f)));
		
		_currentBrush = _colorPallette[0]; // Set default brush 
		#endregion
	}
	
	void Update () {
		if (Input.GetMouseButtonDown(0)) 
			_HandleMouseClick();
	}
	
	void OnGUI () {
		#region BASE_LAYER
		GUI.Box(chosenCharRegion, "chosenChar");
		GUI.Box(pictureSelectRegion, "pictureSelect");
		GUI.DrawTexture(pictureRegion, _picture, ScaleMode.StretchToFill, true);
		GUI.Box(toolbarRegion, "toolbar");
		#endregion
		#region PICTURE_SELECT_LAYER
		GUI.Box(selectUpBtnRegion, "up");
		GUI.Box(selectDownBtnRegion, "down");
		for (int i = 0; i < 4; i++) {
			GUI.Box(selectPictureRegion[i], _pictureNames[_pictureIndexOffset + i]);	
		}
		#endregion
		#region TOOLBAR_LAYER
		GUI.Box(eraseToolRegion, "eraseTool");
		GUI.Box(undoToolRegion, "undoTool");

		for (int i = 0; i < colorPalletteRegion.Length; i++) {
			GUI.Box(colorPalletteRegion[i], _colorPallette[i].name);
		}
		#endregion
	}
	
	#region CURSOR_HANDLING_FUNCTIONS
	private void _HandleMouseClick() {
		Vector2 __p = Input.mousePosition;
		__p.y = Screen.height - __p.y; // y-axis flips
		
		if (chosenCharRegion.Contains(__p)) {
			_HandleChosenCharClick(__p);
		} else if (pictureSelectRegion.Contains(__p)) {
			_HandlePictureSelectionClick(__p);
		} else if (pictureRegion.Contains(__p)) {
			_HandlePictureClick(__p);
		} else if (toolbarRegion.Contains(__p)) {
			_HandleToolbarClick(__p);
		}
	}
	
	private void _HandleChosenCharClick(Vector2 position) {
		//TODO
		Debug.Log("clicked on chosen char");
	}
	
	private void _HandlePictureSelectionClick(Vector2 position) {
		// Navigation buttons
		if (selectUpBtnRegion.Contains(position)) {
			_pictureIndexOffset--;
		} else if (selectDownBtnRegion.Contains(position)) {
			_pictureIndexOffset++;
		}
		
		// Prevent index overflow
		_pictureIndexOffset = Mathf.Clamp(_pictureIndexOffset, 0, _pictureNames.Count - 4);
		
		// Picture selection
		for (int i = 0; i < 4; i++) {
			if (selectPictureRegion[i].Contains(position)) {
				// TODO load new picture
				Debug.Log("selected picture"+_pictureNames[_pictureIndexOffset + i]);
			}
		}
	}
	
	private void _HandlePictureClick(Vector2 position) {
		position.y = Screen.height - position.y; // y-axis flips yet again
		
		// Convert global cursor position to local texture position
		Vector2 __p = position - 
			new Vector2(pictureRegion.x, pictureRegion.y); // Offset origin
		
		// Get color of pixel under cursor
		Color cursorColor = _picture.GetPixel((int)__p.x, (int)__p.y);
		
		// Ignore black pixels
		// Using distance function, because of floating-point precision issue
		if (Vector4.Distance(cursorColor, Color.black) < 0.1f) {
			Debug.Log("color is black, return");
			return;
		}
		// Ignore pixel colors that are same as current brush color
		if (Vector4.Distance(cursorColor, _currentBrush.color) < 0.1f) {
			Debug.Log("identical color, return");
			return;
		}
		
		// Begin flood fill
		_picture.FloodFillArea((int)__p.x, (int)__p.y, _currentBrush.color);
		
		// Save picture after setPixel operations
		_picture.Apply();
	}
	
	private void _HandleToolbarClick(Vector2 position) {
		for (int i = 0; i < colorPalletteRegion.Length; i++) {
			if (colorPalletteRegion[i].Contains(position)) {
				Debug.Log ("selected"+_colorPallette[i].name);
				
				// Set new brush
				_currentBrush = _colorPallette[i];
			}
		}
	}
	#endregion
}
