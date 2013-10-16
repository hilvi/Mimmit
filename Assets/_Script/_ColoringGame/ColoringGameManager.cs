using UnityEngine;
using System.Collections;

public class ColoringGameManager : GameManager {
	
	// Defined regions
	public Rect chosenCharRegion;		// 20,20,160,160
	public Rect pictureSelectRegion;	// 20,200,160,380
	public Rect pictureRegion;			// 200,20,560,560
	public Rect toolbarRegion;			// 780,20,160,560
	
	public override void Start () {
		base.Start();
		
		SetGameState(GameState.Running);
	}
	
	void Update () {
	
	}
	
	void OnGUI () {
		GUI.Box(chosenCharRegion, "chosenChar");
		GUI.Box(pictureSelectRegion, "pictureSelect");
		GUI.Box(pictureRegion, "picture");
		GUI.Box(toolbarRegion, "toolbar");
	}
}
