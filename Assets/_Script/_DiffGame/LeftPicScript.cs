using UnityEngine;
using System.Collections;

public class LeftPicScript : MonoBehaviour {

	GUITexture texture;
	void Start () 
	{
		texture = GetComponent<GUITexture>();
		float width = 600;
		float height = 300;
		Rect rect = new Rect(-width / 2, -1, width, height);
		texture.pixelInset = rect;
	}
}
