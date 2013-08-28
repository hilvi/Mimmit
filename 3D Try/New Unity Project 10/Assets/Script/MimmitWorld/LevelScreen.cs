using UnityEngine;
using System.Collections;

public class LevelScreen : MonoBehaviour {
	bool trigger=false;
	public MouseLook mouse;
	public MouseLook cam;
	float xPos = Screen.width/10f;
	float yPos = Screen.height/10f;
	float width = Screen.width*4/5f;
	float height = Screen.height*4/5f;
	
	Rect boxRect;
	
	void Start()
	{
		
		//boxRect = new Rect(xPos, yPos, width, height);
		
	}
	void OnGUI(){
		
		if (trigger){
			//GUI.Box(boxRect,"??????");
			GUI.Button(new Rect(xPos,yPos,width/2,height/2f),"Play?");
			//GUI.Button(new Rect(xPos,yPos+height/2f,width,height/2f),"Go Back");
		}
	}
	void OnTriggerEnter(){
		trigger = true;
		mouse.enabled = false;
		cam.enabled=false;
	}
	void OnTriggerExit(){
		trigger = false;
		mouse.enabled = true;
		cam.enabled=true;
	}
}
