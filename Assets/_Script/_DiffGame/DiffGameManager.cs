using UnityEngine;
using System.Collections.Generic;

public class DiffGameManager : GameManager 
{
	public Vector2[] rs;
	public Dictionary<Rect, bool> errs = new Dictionary<Rect, bool>();
	public Texture2D tick;
	public Texture2D cross;
	public float size;
	GUIStyle nostyle = new GUIStyle();
	public GUIText text;
	int errorLeft;
	public GameObject musicObject;
	static GameObject obj;
	public AudioClip music;
	List<Rect> misses = new List<Rect>();
	Rect gameArea;
	// Use this for initialization
	public override void  Start () 
	{
		base.Start ();
		SetGameState(GameState.Running);
				
		errorLeft = rs.Length;
		
		//300 is half of picture size
		Vector2 picturePos = new Vector2(Screen.width/2-300, Screen.height/2-300);
		gameArea = new Rect(picturePos.x, picturePos.y+300, 600, 300);
		foreach(Vector3 vec in rs) {
			Rect err = new Rect();
			err.width = err.height = size;//vec.z;
			err.center = (Vector2)vec + picturePos;
			errs.Add(err, false);
		}
		if(InGameMenuGUI.music == null)
		{
		  	InGameMenuGUI.music = (GameObject)Instantiate(musicObject);
			InGameMenuGUI.music.audio.clip = music;
			InGameMenuGUI.music.audio.Play ();
			InGameMenuGUI.music.audio.loop = true;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetMouseButtonDown(0)) {
			Vector2 pos = InputManager.MouseScreenToGUI();
			bool draw = true;
			foreach(Rect err in errs.Keys) {
				if(err.Contains(pos)) {
					draw = false;
					break;
				}
			}
			if(draw && GetGameState() != GameState.Paused && gameArea.Contains(pos)) {
				Rect miss = new Rect();
				miss.width = miss.height = size;
				miss.center = InputManager.MouseScreenToGUI();
				misses.Add(miss);
			}
		}
		if(errorLeft == 0)SetGameState(GameState.Over);
		text.text = errorLeft.ToString (); 
	}
	
	void OnGUI()
	{
		foreach(Rect miss in misses) {
			GUI.DrawTexture(miss, cross);
		}
		foreach(Rect err in new List<Rect>(errs.Keys)) {
			if(GUI.Button(err, "", nostyle) && !errs[err] && GetGameState() != GameState.Paused) {
				errs[err] = true;
				errorLeft--;
			}
			if(errs[err])
			{
				GUI.Box (err,tick,nostyle);	
			}
		}
		
#if UNITY_EDITOR
		foreach(Rect err in errs.Keys) {
			GUI.Box(err, "x");
		}
#endif
	}
}
