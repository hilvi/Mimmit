using UnityEngine;
using System.Collections;

public class BannerScript : MonoBehaviour {
	
	GUITexture banner;
	void Start(){
	 	banner = GetComponent<GUITexture>();
		// Setting background to full screen
		float width = 516;
		float height = 160;
		Rect rect = new Rect(-width / 2, 135, width, height);
		banner.pixelInset = rect;
	}
	// Update is called once per frame
	void Update () {
	
	}
}
