using UnityEngine;
using System.Collections;

public class RightPicScript : MonoBehaviour {

 	GUITexture texture;
	void Start () 
	{
		texture = GetComponent<GUITexture>();
		float width = 600;
		float height = 300;
		Rect rect = new Rect(-width / 2,-300, width, height);
		texture.pixelInset = rect;
	}
}
