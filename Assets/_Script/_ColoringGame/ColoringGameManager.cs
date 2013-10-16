using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColoringGameManager : GameManager {
	
	// Base regions
	public Rect chosenCharRegion;		// 20,20,160,160
	public Rect pictureSelectRegion;	// 20,200,160,380
	public Rect pictureRegion;			// 200,20,560,560
	public Rect toolbarRegion;			// 780,20,160,560
	
	// Picture select regions
	public Rect selectUpBtnRegion;		// 20,200,160,40
	public Rect selectDownBtnRegion;	// 20,540,160,40
	public Rect[] selectPictureRegion;	// 
	private int pictureOffset;
	private List<string> pictureNames;
	
	// Toolbar button regions
	public Rect eraseToolRegion;		// 800,40,120,120
	public Rect undoToolRegion;			// 800,180,120,120
	public Rect[] colorPalletteRegion;	// anchor: 800,320 size:60,60
	
	private Texture2D picture;
	
	public override void Start () {
		base.Start();
		SetGameState(GameState.Running);
		
		// TODO, remove this, debugging
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
		
		// TODO, remove this, debugging
		pictureOffset = 0;
		pictureNames = new List<string>();
		for (int i = 1; i <= 10; i++) {
			pictureNames.Add("Picture"+i.ToString());
		}
		
		// Picture selection panel init
		// TODO, possibly make more elegant, ugly constants and non-integer values
		float vertical = 252.5f;
		selectPictureRegion = new Rect[4];
		for (int i = 0; i < 4; i++) {
			selectPictureRegion[i] = new Rect(67.5f, vertical, 65f, 65f);
			vertical += 70f;
		}
		
		// Color palette panel init
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
	}
	
	void Update () {
		if (Input.GetMouseButtonDown(0)) 
			HandleMouseClick();
	}
	
	void OnGUI () {
		// Base layer
		GUI.Box(chosenCharRegion, "chosenChar");
		GUI.Box(pictureSelectRegion, "pictureSelect");
		GUI.DrawTexture(pictureRegion, picture, ScaleMode.StretchToFill, true);
		GUI.Box(toolbarRegion, "toolbar");
		
		// Select buttons
		GUI.Box(selectUpBtnRegion, "up");
		GUI.Box(selectDownBtnRegion, "down");

		for (int i = 0; i < 4; i++) {
			GUI.Box(selectPictureRegion[i], pictureNames[pictureOffset + i]);	
		}
		
		// Toolbar buttons
		GUI.Box(eraseToolRegion, "eraseTool");
		GUI.Box(undoToolRegion, "undoTool");
		
		foreach(Rect r in colorPalletteRegion) {
			GUI.Box(r, "color");
		}
	}
	
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
			pictureOffset--;
		} else if (selectDownBtnRegion.Contains(position)) {
			pictureOffset++;
		}
		
		pictureOffset = Mathf.Clamp(pictureOffset, 0, pictureNames.Count - 4);
	}
	
	private void HandlePictureClick(Vector2 position) {
		// Convert global cursor position to local texture position
		Vector2 t = position - 
			new Vector2(pictureRegion.x, pictureRegion.y); // Offset origin
		
		// TODO change replacement color to whatever player has selected
		floodFill(Mathf.FloorToInt(t.x), Mathf.FloorToInt(t.y), Color.white, Color.blue);
		
		// Save picture after setPixel operations
		picture.Apply();
	}
	
	private void HandleToolbarClick(Vector2 position) {
		//TODO
		Debug.Log("clicked on toolbar");
	}
	
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
