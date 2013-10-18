using System;
using UnityEngine;

public class PaintFrame
{
	private ColoringGameManager _manager;
	
	private Rect _pictureRegion;
	
	public Texture2D Picture {
		get; set;	
	}
	
	public PaintFrame (ColoringGameManager manager, Rect region, Texture2D picture) {
		_manager = manager;
		_pictureRegion = region;
		Picture = picture;
	}
	
	public void OnGUI() {
		if (Picture != null)
			GUI.DrawTexture(_pictureRegion, Picture, ScaleMode.StretchToFill, true);
	}
	
	public void Paint(Vector2 position, Color color) {
		position.y = Screen.height - position.y; // y-axis flips yet again
		
		// Convert global cursor position to local texture position
		Vector2 __p = position - 
			new Vector2(_pictureRegion.x, _pictureRegion.y); // Offset origin
		
		// Get color of pixel under cursor
		Color cursorColor = Picture.GetPixel((int)__p.x, (int)__p.y);
		
		// Ignore black pixels
		// Using distance function, because of floating-point precision issue
		if (Vector4.Distance(cursorColor, Color.black) < 0.1f) {
			Debug.Log("color is black, return");
			return;
		}
		// Ignore pixel colors that are same as current brush color
		if (Vector4.Distance(cursorColor, color) < 0.1f) {
			Debug.Log("identical color, return");
			return;
		}
		
		// Begin flood fill
		Picture.FloodFillArea((int)__p.x, (int)__p.y, color);
		
		// Save picture after setPixel operations
		Picture.Apply();
	}
}


