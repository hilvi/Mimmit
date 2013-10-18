#define DEVELOPER_MODE

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColoringGameManager : GameManager {
	
	#region PUBLIC
	public Rect chosenCharRegion;		// 20,20,160,160

	public Rect pictureSelectRegion;	// 20,200,160,380
	public Rect selectUpBtnRegion;		// 20,200,160,40
	public Rect selectDownBtnRegion;	// 20,540,160,40
	public Rect[] selectPictureRegion;	// 

	public Rect pictureRegion;			// 200,20,560,560

	public Rect toolbarRegion;			// 780,20,160,560
	public Rect eraseToolRegion;		// 800,40,120,120
	public Rect undoToolRegion;			// 800,180,120,120
	public Rect[] colorPalletteRegion;	// anchor: 800,320 size:60,60
	#endregion
	
	#region PRIVATE
	private int _pictureIndexOffset = 0;
	private List<string> _pictureNames = new List<string>();
	
	private Texture2D _picture;
	
	private struct PaintEvent {
		public int x, y;
		public Color prevColor;
		
		public PaintEvent(int x, int y, Color prevColor) {
			this.x = x;
			this.y = y;
			this.prevColor = prevColor;
		}
	}
	private Stack<PaintEvent> _paintEvents = new Stack<PaintEvent>();
	
	private struct PaintBrush {
		public string name;
		public Color color;
		
		public PaintBrush(string name, Color color) {
			this.name = name;
			this.color = color;
		}
	}
	private Dictionary<int, PaintBrush> _colorPallette = new Dictionary<int, PaintBrush>(); 
	
	private PaintBrush _currentBrush;
	private PaintBrush _eraseBrush;
	#endregion
	
	public override void Start () {
		base.Start();
		SetGameState(GameState.Running);
		
		#if DEVELOPER_MODE
		_picture = _CreateDebugGridTexture(560, 560, 40, 40);

		for (int i = 1; i <= 10; i++) {
			_pictureNames.Add("Picture"+i.ToString());
		}
		#endif

		_InitToolbar(new Vector2(800f, 320f), new Vector2(10f, 10f));
		_InitPictureSelector();
	}
	
	void Update () {
		if (Input.GetMouseButtonDown(0)) 
			_HandleMouseClick();
	}
	
	void OnGUI () {
		#if DEVELOPER_MODE
		GUI.Box(chosenCharRegion, "chosenChar");
		GUI.Box(pictureSelectRegion, "pictureSelect");
		GUI.Box(toolbarRegion, "toolbar");

		GUI.Box(selectUpBtnRegion, "up");
		GUI.Box(selectDownBtnRegion, "down");
		for (int i = 0; i < 4; i++) {
			GUI.Box(selectPictureRegion[i], _pictureNames[_pictureIndexOffset + i]);	
		}

		GUI.Box(eraseToolRegion, "eraseTool");
		GUI.Box(undoToolRegion, "undoTool");

		for (int i = 0; i < colorPalletteRegion.Length; i++) {
			GUI.Box(colorPalletteRegion[i], _colorPallette[i].name);
		}	
		#endif
		
		if (_picture != null)
			GUI.DrawTexture(pictureRegion, _picture, ScaleMode.StretchToFill, true);
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
		
		// Create & store paint event
		PaintEvent __e = new PaintEvent((int)__p.x, (int)__p.y, cursorColor);
		_paintEvents.Push(__e);
	}
	
	private void _HandleToolbarClick(Vector2 position) {
		if (eraseToolRegion.Contains(position)) {
			// Select erase tool
			_currentBrush = _eraseBrush;
		}
		
		if (undoToolRegion.Contains(position)) {
			// No events, return
			if (_paintEvents.Count == 0)
				return;
			
			// Pick most recent event
			PaintEvent __e = _paintEvents.Pop();
			// Re-fill previous area with old color
			_picture.FloodFillArea(__e.x, __e.y, __e.prevColor);
			// Save picture after fill operation
			_picture.Apply();
		}
		
		// Color pallette
		for (int i = 0; i < colorPalletteRegion.Length; i++) {
			if (colorPalletteRegion[i].Contains(position)) {
				Debug.Log ("selected"+_colorPallette[i].name);
				
				// Set new brush
				_currentBrush = _colorPallette[i];
			}
		}
	}
	#endregion
	
	private void _InitPictureSelector() {
		// TODO, possibly make more elegant, ugly constants and non-integer values
		float __v = 252.5f; // Vertical coordinate
		selectPictureRegion = new Rect[4];
		for (int i = 0; i < 4; i++) {
			selectPictureRegion[i] = new Rect(67.5f, __v, 65f, 65f);
			__v += 70f;
		}
		
		// TODO, load picture thumbnails here
	}
	
	private void _InitToolbar(Vector2 paletteAnchor, Vector2 buttonInset) {
		// TODO, possibly make more elegant, ugly constants
		colorPalletteRegion = new Rect[8];
		for (int y = 0; y < 4; y++) {
			for (int x = 0; x < 2; x++)  {
				float xx = (paletteAnchor.x + x * 60f) + (buttonInset.x / 2f);
				float yy = (paletteAnchor.y + y * 60f) + (buttonInset.y / 2f);
				colorPalletteRegion[y * 2 + x] = 
					new Rect(xx, yy, 60f - buttonInset.x, 60f - buttonInset.y);
			}
		}
		
		// Setup color pallette. Start from top-left, row-major order
		_colorPallette.Add(0, new PaintBrush("Blue", Color.blue));
		_colorPallette.Add(1, new PaintBrush("Magenta", Color.magenta));
		_colorPallette.Add(2, new PaintBrush("Cyan", Color.cyan));
		_colorPallette.Add(3, new PaintBrush("Red", Color.red));
		_colorPallette.Add(4, new PaintBrush("Green", Color.green));
		_colorPallette.Add(5, new PaintBrush("Yellow", Color.yellow));
		_colorPallette.Add(6, new PaintBrush("Grey", new Color(0.330f, 0.330f, 0.330f)));
		_colorPallette.Add(7, new PaintBrush("Orange", new Color(1.000f, 0.500f, 0.000f)));
		
		_currentBrush = _colorPallette[0]; // Set default brush 
		_eraseBrush = new PaintBrush("Erase", Color.white);
	}
	
	private Texture2D _CreateDebugGridTexture(int width, int height, 
		int gridWidth, int gridHeight) 
	{
		Texture2D __picture = new Texture2D(width, height);
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (x % gridWidth == 0 || y % gridHeight == 0)
					__picture.SetPixel(x, y, Color.black);
				else
					__picture.SetPixel(x, y, Color.white);
			}
		}
		__picture.Apply();
		return __picture;
	}
	
	private void _ResetPictureToOriginal() {
		
	}
}
