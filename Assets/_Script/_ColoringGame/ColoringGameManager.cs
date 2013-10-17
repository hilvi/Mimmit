using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColoringGameManager : GameManager {
	
	public Rect chosenCharRegion;		// 20,20,160,160

	public Rect pictureSelectRegion;	// 20,200,160,380
	public Rect selectUpBtnRegion;		// 20,200,160,40
	public Rect selectDownBtnRegion;	// 20,540,160,40
	public Rect[] selectPictureRegion;	// 
	private int pictureIndexOffset;
	private List<string> pictureNames;

	public Rect pictureRegion;			// 200,20,560,560
	private Texture2D picture;

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
	
	private Dictionary<int, PaintBrush> colorPallette; // start from top-left, row order
	private PaintBrush currentBrush;

	
	public override void Start () {
		base.Start();
		SetGameState(GameState.Running);

		#region DEBUG_GRID_TEXTURE
		picture = new Texture2D(560, 560);
		for (int x = 0; x < 560; x++) {
			for (int y = 0; y < 560; y++) {
				if (x % 40 == 0 || y % 40 == 0)
					picture.SetPixel(x, y, Color.black);
				else
					picture.SetPixel(x, y, Color.white);
			}
		}
		picture.Apply();
		#endregion
		#region DEBUG_GEN_PICTURE_NAMES
		pictureIndexOffset = 0;
		pictureNames = new List<string>();
		for (int i = 1; i <= 10; i++) {
			pictureNames.Add("Picture"+i.ToString());
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
		colorPallette = new Dictionary<int, PaintBrush>();
		colorPallette.Add(0, new PaintBrush("Blue", Color.blue));
		colorPallette.Add(1, new PaintBrush("Magenta", Color.magenta));
		colorPallette.Add(2, new PaintBrush("Cyan", Color.cyan));
		colorPallette.Add(3, new PaintBrush("Red", Color.red));
		colorPallette.Add(4, new PaintBrush("Green", Color.green));
		colorPallette.Add(5, new PaintBrush("Yellow", Color.yellow));
		colorPallette.Add(6, new PaintBrush("Grey", new Color(0.330f, 0.330f, 0.330f)));
		colorPallette.Add(7, new PaintBrush("Orange", new Color(1.000f, 0.500f, 0.000f)));
		
		currentBrush = colorPallette[0]; // Set default brush 
		#endregion
	}
	
	void Update () {
		if (Input.GetMouseButtonDown(0)) 
			HandleMouseClick();
	}
	
	void OnGUI () {
		#region BASE_LAYER
		GUI.Box(chosenCharRegion, "chosenChar");
		GUI.Box(pictureSelectRegion, "pictureSelect");
		GUI.DrawTexture(pictureRegion, picture, ScaleMode.StretchToFill, true);
		GUI.Box(toolbarRegion, "toolbar");
		#endregion
		#region PICTURE_SELECT_LAYER
		GUI.Box(selectUpBtnRegion, "up");
		GUI.Box(selectDownBtnRegion, "down");
		for (int i = 0; i < 4; i++) {
			GUI.Box(selectPictureRegion[i], pictureNames[pictureIndexOffset + i]);	
		}
		#endregion
		#region TOOLBAR_LAYER
		GUI.Box(eraseToolRegion, "eraseTool");
		GUI.Box(undoToolRegion, "undoTool");

		for (int i = 0; i < colorPalletteRegion.Length; i++) {
			GUI.Box(colorPalletteRegion[i], colorPallette[i].name);
		}
		#endregion
	}
	
	#region CURSOR_HANDLING_FUNCTIONS
	private void HandleMouseClick() {
		Vector2 mousePosition = Input.mousePosition;
		mousePosition.y = Screen.height - mousePosition.y; // y-axis flips
		
		if (chosenCharRegion.Contains(mousePosition)) {
			HandleChosenCharClick(mousePosition);
		} else if (pictureSelectRegion.Contains(mousePosition)) {
			HandlePictureSelectionClick(mousePosition);
		} else if (pictureRegion.Contains(mousePosition)) {
			HandlePictureClick(mousePosition);
		} else if (toolbarRegion.Contains(mousePosition)) {
			HandleToolbarClick(mousePosition);
		}
	}
	
	private void HandleChosenCharClick(Vector2 position) {
		//TODO
		Debug.Log("clicked on chosen char");
	}
	
	private void HandlePictureSelectionClick(Vector2 position) {
		// Navigation buttons
		if (selectUpBtnRegion.Contains(position)) {
			pictureIndexOffset--;
		} else if (selectDownBtnRegion.Contains(position)) {
			pictureIndexOffset++;
		}
		
		// Prevent index overflow
		pictureIndexOffset = Mathf.Clamp(pictureIndexOffset, 0, pictureNames.Count - 4);
		
		// Picture selection
		for (int i = 0; i < 4; i++) {
			if (selectPictureRegion[i].Contains(position)) {
				// TODO load new picture
				Debug.Log("selected picture"+pictureNames[pictureIndexOffset + i]);
			}
		}
	}
	
	private void HandlePictureClick(Vector2 position) {
		position.y = Screen.height - position.y; // y-axis flips yet again
		
		// Convert global cursor position to local texture position
		Vector2 t = position - 
			new Vector2(pictureRegion.x, pictureRegion.y); // Offset origin
		
		// Get color of pixel under cursor
		Color cursorColor = picture.GetPixel((int)t.x, (int)t.y);
		
		// Ignore black pixels
		// Using distance function, because of floating-point precision issue
		if (Vector4.Distance(cursorColor, Color.black) < 0.1f) {
			Debug.Log("color is black, return");
			return;
		}
		// Ignore pixel colors that are same as current brush color
		if (Vector4.Distance(cursorColor, currentBrush.color) < 0.1f) {
			Debug.Log("identical color, return");
			return;
		}
		
		// Begin flood fill
		floodFill((int)t.x, (int)t.y, cursorColor, currentBrush.color);
		
		// Save picture after setPixel operations
		picture.Apply();
	}
	
	private void HandleToolbarClick(Vector2 position) {
		for (int i = 0; i < colorPalletteRegion.Length; i++) {
			if (colorPalletteRegion[i].Contains(position)) {
				Debug.Log ("selected"+colorPallette[i].name);
				
				// Set new brush
				currentBrush = colorPallette[i];
			}
		}
	}
	#endregion
	
	/*
	 * Recursive flood fill algorithm
	 * http://en.wikipedia.org/wiki/Flood_fill
	 */ 
	private void floodFill(int x, int y, Color target, Color replacement) {
		if (picture.GetPixel(x, y) != target)
			return;

		picture.SetPixel(x, y, replacement);
		
		floodFill(x - 1, y, target, replacement);
		floodFill(x + 1, y, target, replacement);
		floodFill(x, y - 1, target, replacement);
		floodFill(x, y + 1, target, replacement);
		return;
	}
}
