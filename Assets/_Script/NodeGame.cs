using UnityEngine;
using System.Collections;

public class NodeGame : INode {
	
	public Transform [] track;
	
	bool isGirlOn;
	Rect boxRect, playRect, backRect;
	Movement movement;
	
	// Use this for initialization
	void Start () 
	{
		float width = Screen.width / 2;
		float height = Screen.height /3 * 2;
		boxRect = new Rect(width - width /2, Screen.height/2 - height / 2, width, height);
		float buttonW = Screen.width / 3;
		float buttonH = Screen.height / 5;
		playRect = new Rect(width - buttonW / 2, Screen.height / 2 - buttonH,buttonW, buttonH);
		backRect = new Rect(width - buttonW / 2, Screen.height / 2,buttonW, buttonH);
		movement = GameObject.Find ("Girl").GetComponent<Movement>();
	}
	
	// Update is called once per frame
	void OnGUI () 
	{
		if(!isGirlOn)return;
		GUI.Box(boxRect, "GameName");
		if(GUI.Button (playRect,"Play"))
		{
			
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
