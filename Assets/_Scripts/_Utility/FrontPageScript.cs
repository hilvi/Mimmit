using UnityEngine;
using System.Collections;

public class FrontPageScript : Overlay
{	
	#region MEMBERS
	//public Texture2D texture;
	public Texture2D texture;
	public Texture2D cursor;
	Rect _buttonRect;
	Rect _movieRect;
	public MovieTexture movie;
	private GUIStyle _noStyle = new GUIStyle();
	private Vector2 _hotSpot = Vector2.zero;
	private CursorMode _cursorMode = CursorMode.Auto;
	#endregion

	#region UNITY_METHODS
	void Start ()
	{
		//_background = GetComponent<GUITexture> ();
		// Setting background to full screen
		float width = Screen.width;
		float height = Screen.height;
	 	//_movieRect = new Rect (-width / 2, - height / 2, width, height);
		_movieRect = new Rect(0,0,960,600);
		/*if(movie != null)
			_background.pixelInset = rect;*/
		float _edge = 100;
		_buttonRect = new Rect (width / 2 - _edge / 2, height - 1.5f * _edge, _edge, _edge);
		movie.loop = true;
		movie.Play();
	}

	void OnGUI ()
	{
		GUI.Box (_movieRect,movie,_noStyle);
		Cursor.SetCursor(cursor,_hotSpot,_cursorMode);
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
