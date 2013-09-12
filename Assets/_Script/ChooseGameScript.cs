using UnityEngine;
using System.Collections;

public class ChooseGameScript : MonoBehaviour {
	
	GUITexture background;
	// Use this for initialization
	Rect mapRect, otterRect, hedgehogRect, treeRect, horseRect, owlRect, dragonRect, bearRect,granmaRect,seaDragonRect;
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
		bearRect = new Rect(460, 70,_edge,_edge);
		granmaRect = new Rect(600,45,_edge,_edge);
		seaDragonRect = new Rect(760,45,_edge,_edge);
	}
	// Update is called once per frame
	void OnGUI ()
	{
		if(GUI.Button (mapRect,"Map"))
		{
			Application.LoadLevel("MapWorld");
		}
		if(GUI.Button (otterRect,"Otter"))
		{
			
		}
		if(GUI.Button (hedgehogRect,"Hedgehog"))
		{
			
		}
		if(GUI.Button (treeRect,"Tree"))
		{
			
		}
		if(GUI.Button (horseRect,"Horse"))
		{
			
		}
		if(GUI.Button (owlRect,"Owl"))
		{
			Application.LoadLevel("Flip_1");
		}
		if(GUI.Button (dragonRect,"Dragon"))
		{
			
		}
		if(GUI.Button (bearRect,"Bear"))
		{
			
		}
		if(GUI.Button (granmaRect,"Granma"))
		{
			
		}
		if(GUI.Button (seaDragonRect,"Sea Dragon"))
		{
			
		}
	}
}
