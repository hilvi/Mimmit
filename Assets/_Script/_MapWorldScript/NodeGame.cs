using UnityEngine;
using System.Collections;

public class NodeGame : INode {
	
	public Transform [] track;
	public Texture2D gameTexture;
	
	bool isGirlOn;
	Rect boxRect, playRect, backRect;
	Movement movement;
	public string gameName;
	
	// Use this for initialization
	void Start () 
	{
		float width = Screen.width / 3 ;
		float height = Screen.height / 2;
		boxRect = new Rect(Screen.width/2 - width / 2, height - (0.5f * width), width, width);
		float buttonH = height + 0.5f * width;
		playRect = new Rect(Screen.width / 2 - width /2, buttonH, width / 2, width / 2);
		backRect = new Rect(Screen.width / 2, buttonH, width / 2, width / 2);
		movement = GameObject.Find ("Girl").GetComponent<Movement>();
	}
	
	// Update is called once per frame
	void OnGUI () 
	{
		if(!isGirlOn)return;
		GUI.enabled = true;
		NavigationState currentState = Manager.GetNavigationState();
		if(currentState == NavigationState.Pause)
		{
			GUI.enabled = false;
		}
		//GUI.Box(boxRect, gameTexture);
		GUI.DrawTexture(boxRect,gameTexture);
		if(GUI.Button (playRect,"Play"))
		{
			Application.LoadLevel(gameName);
		}
		if(GUI.Button (backRect,"Back"))
		{
			movement.SetPath(track);
			isGirlOn = false;
		}
	}
	
	public override void SetGirlOn()
	{
		isGirlOn = true;
	}
}
