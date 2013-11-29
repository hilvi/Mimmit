using UnityEngine;
using System.Collections;

public class FrontPageScript : Overlay
{	
	#region MEMBERS
	public Texture2D texture;
	Rect _buttonRect;
	GUITexture _background;
	#endregion

	#region UNITY_METHODS
	void Start ()
	{
		_background = GetComponent<GUITexture> ();
		// Setting background to full screen
		float width = Screen.width;
		float height = Screen.height;
		Rect rect = new Rect (-width / 2, - height / 2, width, height);
		if(_background != null)
			_background.pixelInset = rect;
		float _edge = 100;
		_buttonRect = new Rect (width / 2 - _edge / 2, height - 1.5f * _edge, _edge, _edge);
	}

	void OnGUI ()
	{
		if (MGUI.HoveredButton (_buttonRect, texture)) 
		{
			if (Application.CanStreamedLevelBeLoaded ("ChoiceScene")) 
			{
				Application.LoadLevel ("ChoiceScene");
			}
		}
	}
	#endregion
}
