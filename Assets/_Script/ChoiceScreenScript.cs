using UnityEngine;
using System.Collections;

public class ChoiceScreenScript : MonoBehaviour {

	GUITexture background;
	// Use this for initialization
	Rect blondeRect, bruneRect, foxRect , boyRect, mapRect, buttonRect; 
	bool characterChosen = false;
	void Start () 
	{
		background = GetComponent<GUITexture>();
		// Setting background to full screen
		float width = Screen.width;
		float height = Screen.height;
		Rect rect = new Rect(-width / 2, - height / 2, width, height);
		background.pixelInset = rect;
		
		int _x = 175; int _y = 230; int _width = 150;
		blondeRect 	= new Rect(_x,_y,150,150);
		bruneRect 	= new Rect(_x + _width,_y,_width,_width);
		foxRect 	= new Rect(_x + 2 * _width,_y,_width,_width);
		boyRect 	= new Rect(_x + 3 * _width,_y,_width,_width);
		
		float _height = 100; float _ySecondButtons = 450;
		mapRect = new Rect(_x + 75, _ySecondButtons,_width,_height);
		buttonRect = new Rect(_x  + 2* _width + 75,_ySecondButtons,_width,_height);
	}
	

	void OnGUI()
	{
		if(GUI.Button(blondeRect,"Blonde"))
		{
			Manager.SetCharacter(Character.Blonde);
			characterChosen = true;
		}
		if(GUI.Button(bruneRect,"Brune"))
		{
			Manager.SetCharacter(Character.Brune);
			characterChosen = true;
		}
		if(GUI.Button(foxRect,"Fox"))
		{
			Manager.SetCharacter(Character.Fox);
			//characterChosen = true;
		}
		if(GUI.Button(boyRect,"Boy"))
		{
			Manager.SetCharacter(Character.Boy);
			//characterChosen = true;
		}
		if(characterChosen)
		{
			if(GUI.Button(mapRect,"Map"))
			{
				Manager.SetScreenChoice(ScreenChoice.Map);
				Application.LoadLevel("MapWorld");
			}
			if(GUI.Button(buttonRect,"ButtonScreenScene"))
			{
				Manager.SetScreenChoice(ScreenChoice.Button);
				Application.LoadLevel("ChooseGameScene");
			}
		}
	}
}
