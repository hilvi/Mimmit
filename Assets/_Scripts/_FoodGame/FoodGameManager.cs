using UnityEngine;
using System.Collections;
using System.Collections.Generic;

	public class FoodGameManager : GameManager {

	#region MEMBERS
	/*
	public Rect measurePreparingArea;
	public Rect measureToolsArea;
	public Rect measureIngredientsArea;

	private FoodObject[] ingredients;
	*/
	public GameObject musicObject;
	public AudioClip music;
	#endregion

	#region UNITY_METHODS
	public override void Start () 
	{
		base.Start();
		SetGameState(GameState.Running);
		
		if(InGameMenuGUI.music == null)
		{
			InGameMenuGUI.music = (GameObject)Instantiate(musicObject);
			InGameMenuGUI.music.audio.clip = music;
			InGameMenuGUI.music.audio.Play();
		}

		/*
		// tentative measurements
		measurePreparingArea = new Rect(200f, 300f, 560f, 300f);
		measureToolsArea = new Rect(120f, 300f, 80f, 300f);
		measureIngredientsArea = new Rect(200f, 220f, 60f, 60f);

		float xOffset = 0f;
		ingredients = new FoodObject[9];
		for (int i = 0; i < ingredients.Length; i++) {
			Rect r = new Rect(measureIngredientsArea);
			r.x += xOffset;
			xOffset += 60f;
			ingredients[i] = new FoodObject("food" + i, r);
		}

		dragging = false;*/
	}

	void Update () {
	
	}
	/*
	private string foodNameOnCursor;
	private bool dragging;

	void OnGUI () {
		GUI.Box(measurePreparingArea, "prep_rect");
		GUI.Box(measureToolsArea, "tools_rect");
	
		var e = Event.current;
		Vector2 mp = e.mousePosition;
	
		bool leftMouse = (e.button == 0);
		bool mouseDown = (e.type == EventType.mouseDown);
		bool mouseUp = (e.type == EventType.mouseUp);
		bool mouseHold = (e.type == EventType.mouseDrag);
		bool mouseIngrd = MouseHitOnIngredient(mp);

		if (mouseDown)
			dragging = true;
		if (mouseUp)
			dragging = false;

		DrawIngredients();

		if (mouseUp) {
			if (measurePreparingArea.Contains(mp)) {
				Debug.Log("Dropped " + foodNameOnCursor + "into preparing area");
			}
		}

		if (dragging) {
			Rect newRect = new Rect(mp.x - 30f, mp.y - 30f, 60f, 60f);
			GUI.Box(newRect, foodNameOnCursor);
		} else {
			foodNameOnCursor = "null";
		}
	}
	*/
	#endregion

	/*
	private bool MouseHitOnIngredient (Vector2 mousePosition) {
		foreach (FoodObject r in ingredients) {
			if (r.Contains(mousePosition)) {
				foodNameOnCursor = r.FoodName;
				return true;
			}
		}
		return false;
	}

	private void DrawIngredients () {
		foreach (FoodObject r in ingredients) {
			r.Draw();
		}
	}*/
}
