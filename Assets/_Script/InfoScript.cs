using UnityEngine;
using System.Collections;

public class InfoScript : MonoBehaviour {

	Rect rect;
	string str1,str2,str3,str4,str5,str6,str7,str8;
	void Start () {
		float w = Screen.width;
		float h = Screen.height/2;
		
		rect = new Rect(0,h - h/2, w, h);
		str1 = "Hi there. \nSo I made modifications based on the pdf from Meri. I simply took the picture from there as background\nI also took some of the buttons and quickly edited them. That is why they don't look good.\n";
		str7 = "The front page will later get some animated characters on the back\n\n";
		str2 = "I created the screen for choosing the character. For the moment the boy and the fox cannot be chosen because I have not yet added them for the map\n\n";
		str6 = "Still something stroke me, what is the point of choosing a character if not using the map?\nThat may be confusing to choose something that has no influence on the rest.\n";
		str3 = "The cards are now squared on the memory game\n\n";
		str4 = "The buttons will not show the grey square. \nEverywhere there is a grey square means that the pic below needs to be taken off the background and made as a single texture for the button\n";
		str5 = "It is always the same music but this is just because I did not take the time to edit the audio files.\n\n\nPress space to continue";
		str8 = "The layout of the screen with all the buttons is missing the buttons to get back from there. \nAt the moment the user cannot change character without passing by the map";
	}
	
	// Update is called once per frame
	void OnGUI () 
	{
		GUI.Box (rect,str1+ str7 + str2 +str6 + str3 + str4+ str8 + str5);
	}
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			Application.LoadLevel("MainScreenScene");
		}
	}
}
