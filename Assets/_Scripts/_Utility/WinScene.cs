using UnityEngine;
using System.Collections;

/// <summary>
/// Use in the WinScene to display the movie texture.
/// </summary>
public class WinScene : Overlay 
{
	#region MEMBERS
	public MovieTexture movie;
	public Texture2D texture;
	private GUIStyle _noStyle = new GUIStyle();
	private Rect _textRect;
	private Rect _buttonRect;
	#endregion

	#region UNITY_METHODS
	void Start()
	{
		float width = Screen.width;
		float height = Screen.height;
		float _edge = 100;
		_buttonRect = new Rect (width / 2 - _edge / 2, height - 1.5f * _edge, _edge, _edge);
		_textRect = new Rect(0,0,960,600);
		movie.Play();
	}
	void OnGUI()
	{
		GUI.Box(_textRect,movie,_noStyle);
		if (MGUI.HoveredButton (_buttonRect, texture)) 
		{
			if (Application.CanStreamedLevelBeLoaded ("ChoiceScene")) 
			{
				LoadLevel ("ChoiceScene");
			}
		}
	}
	#endregion
}
