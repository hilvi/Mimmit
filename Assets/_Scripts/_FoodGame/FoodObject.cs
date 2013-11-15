using UnityEngine;
using System.Collections;

public class FoodObject {

	private string foodName;
	private Rect defaultRect;

	public FoodObject(string foodName, Rect defaultRect) {
		FoodName = foodName;
		this.defaultRect = defaultRect;
	}

	public void Draw () {
		GUI.Box (defaultRect, FoodName);
	}

	public bool Contains (Vector2 point) {
		return defaultRect.Contains(point);
	}

	public string FoodName { 
		get {
			return foodName;
		} set {
			foodName = value;
		} 
	}

	public Vector2 GetCenterPosition () {
		Vector2 v = new Vector2(defaultRect.x, defaultRect.y);
		v.x -= defaultRect.width / 2f;
		v.y -= defaultRect.height / 2f;
		return v;
	}
}
