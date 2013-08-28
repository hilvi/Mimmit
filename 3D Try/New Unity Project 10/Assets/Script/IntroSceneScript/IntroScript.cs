using UnityEngine;
using System.Collections;

public class IntroScript : MonoBehaviour {

	Rect banner;
	Rect buttonPlay, buttonQuit;
	void Start () {
		float width = Screen.width;
		float height = Screen.height;
		float marginW = width / 10;
		float marginH = height / 10;
		
		banner = new Rect(marginW, marginW, 8 * marginW, 2 * marginH);
		buttonPlay = new Rect(marginW * 3, marginH * 4, 4 * marginW, 2* marginH );
		buttonQuit =  new Rect(marginW * 3, marginH * 7, 4 * marginW, 2* marginH );
	}
	
	// Update is called once per frame
	void OnGUI () {
		GUI.Box(banner,"Mimmit");
		if(GUI.Button (buttonPlay,"Play"))
		{
			Application.LoadLevel("MapWorld");
		}
#if UNITY_STANDALONE
		if(GUI.Button(buttonQuit,"Quit"))
		{
			Application.Quit();
		}
#endif
	}
}
