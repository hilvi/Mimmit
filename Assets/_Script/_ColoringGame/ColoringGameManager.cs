using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColoringGameManager : GameManager {
	
	#region CHOSEN_CHAR_REGION_VARS
	public Rect chosenCharRegion;		// 20,20,160,160
	#endregion
	#region PICTURE_SELECT_REGION_VARS
	public Rect pictureSelectRegion;	// 20,200,160,380
	public Rect selectUpBtnRegion;		// 20,200,160,40
	public Rect selectDownBtnRegion;	// 20,540,160,40
	public Rect[] selectPictureRegion;	// 
	private int pictureIndexOffset;
	private List<string> pictureNames;
	#endregion
	#region PICTURE_REGION_VARS
	public Rect pictureRegion;			// 200,20,560,560
	private Texture2D picture;
	#endregion
	#region TOOLBAR_REGION_VARS
	public Rect toolbarRegion;			// 780,20,160,560
	public Rect eraseToolRegion;		// 800,40,120,120
	public Rect undoToolRegion;			// 800,180,120,120
	public Rect[] colorPalletteRegion;	// anchor: 800,320 size:60,60
	#endregion
	
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
		foreach(Rect r in colorPalletteRegion) {
			GUI.Box(r, "color");
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
		if (selectUpBtnRegion.Contains(position)) {
			pictureIndexOffset--;
		} else if (selectDownBtnRegion.Contains(position)) {
			pictureIndexOffset++;
		}
		
		pictureIndexOffset = Mathf.Clamp(pictureIndexOffset, 0, pictureNames.Count - 4);
	}
	
	private void HandlePictureClick(Vector2 position) {
		position.y = Screen.height - position.y; // y-axis flips yet again
		
		// Convert global cursor position to local texture position
		Vector2 t = position - 
			new Vector2(pictureRegion.x, pictureRegion.y); // Offset origin
		
		// TODO change replacement color to whatever player has selected
		floodFill((int)t.x, (int)t.y, Color.white, Color.blue);
		
		// Save picture after setPixel operations
		picture.Apply();
	}
	
	private void HandleToolbarClick(Vector2 position) {
		//TODO
		Debug.Log("clicked on toolbar");
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
