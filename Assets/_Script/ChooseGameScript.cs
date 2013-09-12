using UnityEngine;
using System.Collections;

public class ChooseGameScript : MonoBehaviour {
	
	GUITexture background;
	
	public Texture2D otter, hedgehog, tree, horse, owl, dragon,bear,granma,seaDragon;
	Rect mapRect, otterRect, hedgehogRect, treeRect, horseRect, owlRect, dragonRect, bearRect,granmaRect,seaDragonRect;
	Rect characterBoxRect;
	public Texture2D blonde, brune, fox, boy;
	Texture2D chosen;
	GUIStyle noStyle = new GUIStyle();

	Texture2D GetChosenCharacter ()
	{
		Character _character = Manager.GetCharacter();
		switch(_character)
		{
			case Character.Blonde:
				return blonde;
			case Character.Brune:
				return blonde;
			case Character.Boy:
				return blonde;
			case Character.Fox:
				return blonde;
			case Character.None:
				return blonde;
			default:
				return blonde;
		}
	}
	
	void Start () 
	{
		background = GetComponent<GUITexture>();
		// Setting background to full screen
		float width = Screen.width;
		float height = Screen.height;
		Rect rect = new Rect(-width / 2, - height / 2, width, height);
		background.pixelInset = rect;
		
		mapRect = new Rect(500,220,370,305);
		float _edge = 100;
		otterRect = new Rect(40,400,_edge,_edge);
		hedgehogRect = new Rect(200,385,_edge,_edge);
		treeRect = new Rect(105, 285,_edge,_edge);
		horseRect = new Rect(300,285,_edge,_edge);
		owlRect = new Rect(200, 185,_edge,_edge);
		dragonRect = new Rect(320, 110,_edge,_edge);
		bearRect = new Rect(460, 65,_edge,_edge);
		granmaRect = new Rect(600,45,_edge,_edge);
		seaDragonRect = new Rect(760,45,_edge,_edge);
		characterBoxRect = new Rect(20,20,200,200);
		chosen = GetChosenCharacter();
	}
	// Update is called once per frame
	void OnGUI ()
	{
		GUI.Box (characterBoxRect,chosen,noStyle);
		if(GUI.Button (mapRect,"Map"))
		{
			Application.LoadLevel("MapWorld");
		}
		if(MGUI.HoveredButton (otterRect,otter))
		{
			
		}
		if(MGUI.HoveredButton (hedgehogRect,hedgehog))
		{
			
		}
		if(MGUI.HoveredButton (treeRect,tree))
		{
			
		}
		if(MGUI.HoveredButton (horseRect,horse))
		{
			
		}
		if(MGUI.HoveredButton (owlRect,owl))
		{
			Application.LoadLevel("Flip_1");
		}
		if(MGUI.HoveredButton (dragonRect,dragon))
		{
			
		}
		if(MGUI.HoveredButton (bearRect,bear))
		{
			
		}
		if(MGUI.HoveredButton (granmaRect,granma))
		{
			
		}
		if(MGUI.HoveredButton (seaDragonRect,seaDragon))
		{
			
		}
	}
}
