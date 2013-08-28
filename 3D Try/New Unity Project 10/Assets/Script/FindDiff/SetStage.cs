using UnityEngine;
using System.Collections;

public class SetStage : MonoBehaviour {
	
	public GameObject canvas1, canvas2;
	
	Texture canv1, canv2;
	public int num;
	bool swapped, stopped;
	
	
	
	int index =13;
	void Awake(){
		num = Random.Range(1,index);
		swapped = false;
	}
	// Use this for initialization
	void Start () {
		canv1= (Texture)Resources.Load ("Canvas/canvas1/FD"+num);
		
		canv2= (Texture)Resources.Load ("Canvas/canvas2/FD"+num);
		
		canvas1.renderer.material.SetTexture("_MainTex",canv1);
		
		//canvas2.renderer.material.SetTexture("_MainTex",canv2);
		
		
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnGUI(){
		if(swapped){
				canvas2.renderer.material.SetTexture("_MainTex",canv2);
				GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),canv2);
		}
		else{
				canvas1.renderer.material.SetTexture("_MainTex",canv1);
				GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),canv1);
			}
		if(stopped){
			GUI.Box (new Rect(0,0,Screen.width,Screen.height),"This is Something");
		}
		if(GUI.Button(new Rect(Screen.width-Screen.width/10,0,40,20),"Stop")){
			if (stopped){
				stopped = false;
			}
			else{
				stopped =true;
				
		}
			
		}
		if(GUI.Button(new Rect(Screen.width-Screen.width/10,Screen.height-Screen.height/20,40,20),"Swap")){
			if (swapped){
				swapped = false;
			}
			else{
				swapped =true;
		}
		
		
	}
}
}
