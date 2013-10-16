using UnityEngine;
using System.Collections;

public class ColoringGameManager : GameManager {
	
	// Defined regions
	public Rect chosenCharRegion;		// 20,20,160,160
	public Rect pictureSelectRegion;	// 20,200,160,380
	public Rect pictureRegion;			// 200,20,560,560
	public Rect toolbarRegion;			// 780,20,160,560
	
	private Texture2D picture;
	
	public override void Start () {
		base.Start();
		
		SetGameState(GameState.Running);
		
		picture = new Texture2D(560, 560);
		for (int x = 0; x < 560; x++) {
			for (int y = 0; y < 560; y++) {
				if (x % 10 == 0 || y % 10 == 0)
					picture.SetPixel(x, y, new Color(1.0f, 0.5f, 0.0f, 1.0f));
				else
					picture.SetPixel(x, y, Color.clear);
			}
		}
		picture.Apply();
	}
	
	void Update () {
	
	}
	
	void OnGUI () {
		GUI.Box(chosenCharRegion, "chosenChar");
		GUI.Box(pictureSelectRegion, "pictureSelect");
		GUI.DrawTexture(pictureRegion, picture, ScaleMode.StretchToFill, true);
		GUI.Box(toolbarRegion, "toolbar");
	}
}
