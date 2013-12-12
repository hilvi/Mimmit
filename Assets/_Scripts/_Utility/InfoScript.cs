using UnityEngine;
using System.Collections;

public class InfoScript : MonoBehaviour {

	Rect rect;
	string Date,str1,str2,str3,str4,str5,str6,str7,str8;
	void Start () {
		float w = Screen.width;
		float h = Screen.height/2;
		
		rect = new Rect(0,h - h/2, w, h);
		Date = "03/10/2013\n\n";
		str1 = "Hi there. \nWorked a lot of programming things that do not show\n\n";
		str2 = "Front page is ready in my opinion, it matches Pekka's version\n\n";
		str3 = "The character choice is also ready, we reviewed it a little with Teemu to include the banner";
		str4 = "\nThe screen for choosing game has been changed since thelayout has changed.\nI can still change easily how it looks";
		str5 = "\nThe world map is removed since it does not make sense to move around with little to do";
		str6 = "\n Memory game has 3 levels for now, I will add some more and review the background so that cards reveals a picture.\n";
		str7= "At the end, the win screen appears but I am not totally satisfied with the way it looks\n";
		str8 = "Tonight I will add 3 levels of difference game\n";
	}
	
	// Update is called once per frame
	void OnGUI () 
	{
		GUI.Box (rect,Date+str1+ str2 + str3 +str4+str5+str6+str7+str8);
	}
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			Application.LoadLevel("MainScreenScene");
		}
	}
}
