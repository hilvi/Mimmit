using UnityEngine;
using System.Collections;

public class BackgroundScript : MonoBehaviour {

	GUITexture background;
	void Start () 
	{
		background = GetComponent<GUITexture>();
		// Setting background to full screen
		float width = Screen.width;
		float height = Screen.height;
		Rect rect = new Rect(-width / 2, - height / 2, width, height);
		background.pixelInset = rect;
	}
}
