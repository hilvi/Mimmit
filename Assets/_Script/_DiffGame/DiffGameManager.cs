using UnityEngine;
using System.Collections;

public class DiffGameManager : GameManager 
{
	public Vector2 r1 ,r2 ,r3,r4,r5,r6,r7;
	Rect err1, err2, err3, err4, err5, err6,err7;
	bool err1Found, err2Found, err3Found, err4Found,err5Found, err6Found, err7Found;
	public Texture2D tick;
	public float size;
	GUIStyle nostyle = new GUIStyle();
	public GUIText text;
	int errorLeft = 7;
	public GameObject musicObject;
	static GameObject obj;
	public AudioClip music;
	// Use this for initialization
	public override void  Start () 
	{
		base.Start ();
		SetGameState(GameState.Running);
		float __side = size;
		err1.center = r1;
		err1.width = err1.height = __side;
		err2.center = r2;
		err2.width = err2.height = __side;
		err3.center = r3;
		err3.width = err3.height = __side;
		err4.center = r4;
		err4.width = err4.height = __side;
		err5.center = r5;
		err5.width = err5.height = __side;
		err6.center = r6;
		err6.width = err6.height = __side;
		err7.center = r7;
		err7.width = err7.height = __side;
		if(InGameMenuGUI.music == null)
		{
		  	InGameMenuGUI.music = (GameObject)Instantiate(musicObject);
			InGameMenuGUI.music.audio.clip = music;
			InGameMenuGUI.music.audio.Play ();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(errorLeft == 0)SetGameState(GameState.Over);
		text.text = errorLeft.ToString (); 
	}
	void OnGUI()
	{
		if(GUI.Button(err1,"",nostyle)&& !err1Found)
		{
			err1Found = true;
			errorLeft--;
		}
		if(GUI.Button(err2,"",nostyle) && !err2Found)
		{
			err2Found = true;
			errorLeft--;
		}
		if(GUI.Button(err3,"",nostyle) && !err3Found)
		{
			err3Found = true;
			errorLeft--;
		}
		if(GUI.Button(err4,"",nostyle)&& !err4Found)
		{
			err4Found = true;
			errorLeft--;
		}
		if(GUI.Button(err5,"",nostyle)&& !err5Found)
		{
			err5Found = true;
			errorLeft--;
		}
		if(GUI.Button(err6,"",nostyle)&& !err6Found)
		{
			err6Found = true;
			errorLeft--;
		}
		if(GUI.Button(err7,"",nostyle)&& !err7Found)
		{
			err7Found = true;
			errorLeft--;
		}
		if(err1Found)
		{
			GUI.Box (err1,tick,nostyle);	
		}
		if(err2Found){
			GUI.Box (err2,tick,nostyle);
		}
		if(err3Found){
			GUI.Box (err3,tick,nostyle);
		}
		if(err4Found){
			GUI.Box (err4,tick,nostyle);
		}
		if(err5Found){GUI.Box (err5,tick,nostyle);}
		if(err6Found){GUI.Box (err6,tick,nostyle);}
		if(err7Found){GUI.Box (err7,tick,nostyle);}
#if UNITY_EDITOR
		GUI.Box(err1,"1");
		GUI.Box(err2,"2");
		GUI.Box(err3,"3");
		GUI.Box(err4,"4");
		GUI.Box(err5,"5");
		GUI.Box(err6,"6");
		GUI.Box(err7,"7");
#endif
	}
}
