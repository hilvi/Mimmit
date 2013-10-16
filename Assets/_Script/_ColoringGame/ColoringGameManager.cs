using UnityEngine;
using System.Collections;

public class ColoringGameManager : GameManager {
	
	// Base regions
	public Rect chosenCharRegion;		// 20,20,160,160
	public Rect pictureSelectRegion;	// 20,200,160,380
	public Rect pictureRegion;			// 200,20,560,560
	public Rect toolbarRegion;			// 780,20,160,560
	
	// Toolbar button regions
	public Rect eraseToolRegion;		// 800,40,120,120
	public Rect undoToolRegion;			// 800,180,120,120
	public Rect[] colorPalletteRegion;	// anchor: 800,320 size:60,60
	
	private Texture2D picture;
	
	public override void Start () {
		base.Start();
		
		SetGameState(GameState.Running);
		
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
		
		colorPalletteRegion = new Rect[8];
		Vector2 anchor = new Vector2(800f, 320f);
		Vector2 minimize = new Vector2(10f, 10f);
		for (int y = 0; y < 4; y++) {
			for (int x = 0; x < 2; x++)  {
				float xx = (anchor.x + x * 60f) + (minimize.x/2f);
				float yy = (anchor.y + y * 60f) + (minimize.y/2f);
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
		
		// Toolbar buttons
		GUI.Box(eraseToolRegion, "eraseTool");
		GUI.Box(undoToolRegion, "undoTool");
		
		foreach(Rect r in colorPalletteRegion) {
			GUI.Box(r, "color");
		}
	}
	
	private void HandleMouseClick() {
		Vector2 mousePosition = Input.mousePosition;
		
		if (chosenCharRegion.Contains(mousePosition)) {
			HandleChosenCharClick();
		} else if (pictureSelectRegion.Contains(mousePosition)) {
			HandlePictureSelectionClick();
		} else if (pictureRegion.Contains(mousePosition)) {
			HandlePictureClick();
		} else if (toolbarRegion.Contains(mousePosition)) {
			HandleToolbarClick();
		}
	}
	
	private void HandleChosenCharClick() {
		//TODO
		Debug.Log("clicked on chosen char");
	}
	
	private void HandlePictureSelectionClick() {
		//TODO
		Debug.Log("clicked on picture selection panel");
	}
	
	private void HandlePictureClick() {		
		Vector2 t = Input.mousePosition - 
			new Vector3(pictureRegion.x, pictureRegion.y, 0); // Offset origin
		floodFill(Mathf.FloorToInt(t.x), Mathf.FloorToInt(t.y), Color.white, Color.blue);
		picture.Apply();
	}
	
	private void HandleToolbarClick() {
		//TODO
		Debug.Log("clicked on toolbar");
	}
	
	/*
	 * Flood fill algorithm
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
