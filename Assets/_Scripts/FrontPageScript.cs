using UnityEngine;
using System.Collections;

public class FrontPageScript : MonoBehaviour {
	GUITexture background;
	public Texture2D texture;
	// Use this for initialization
	Rect buttonRect;
	void Start () 
	{
		background = GetComponent<GUITexture>();
		// Setting background to full screen
		float width = Screen.width;
		float height = Screen.height;
		Rect rect = new Rect(-width / 2, - height / 2, width, height);
		background.pixelInset = rect;
		float _edge = 100;
		buttonRect = new Rect(width / 2 - _edge / 2, height - 1.5f*_edge,_edge,_edge );
	}
	void OnGUI()
	{
		if(MGUI.HoveredButton(buttonRect,texture))
		{
			Application.LoadLevel("ChoiceScene");
		}
	}
}
