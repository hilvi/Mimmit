using UnityEngine;
using System.Collections;

public class InfoScript : MonoBehaviour {

	Rect rect;
	string Date,str1,str2,str3,str4,str5,str6,str7,str8;
	void Start () {
		float w = Screen.width;
		float h = Screen.height/2;
		
		rect = new Rect(0,h - h/2, w, h);
		Date = "18/09/2013\n\n";
		str1 = "Hi there. \nMainly worked on getting the navigation between different parts of the game working\n";
		str2 = "It is now possible to pause the game and all buttons will have no actions";
		str3 = "I repositioned a ittle the buttons on the ButtonScene\n The ButtonScene is the one with all games as buttons as opposed to the World Map";
		str4 = "\nThe cards are now straight to avoid the sharp edges.\n";
		str5 = "\nNext step, I will add some more levels to the memory game and start the difference game.";
	}
	
	// Update is called once per frame
	void OnGUI () 
	{
		GUI.Box (rect,Date+str1+ str2 + str3 +str4+str5);
	}
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			Application.LoadLevel("MainScreenScene");
		}
	}
}
