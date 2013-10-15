using UnityEngine;
using System.Collections.Generic;

public class DiffGameManager : GameManager 
{
	public Rect[] errors;
	public Dictionary<Rect, bool> errs = new Dictionary<Rect, bool>();
	public Texture2D tick;
	public Texture2D cross;
	public float clickSize;
	public GUIText text;
	int errorLeft;
	public GameObject musicObject;
	public AudioClip music;
	public AudioClip missSound;
	public AudioClip hitSound;
	AudioSource audioSource;
	List<Rect> misses = new List<Rect>();
	List<Rect> hits = new List<Rect>();
	Rect gameArea;
	// Use this for initialization
	public override void  Start () 
	{
		base.Start ();
		SetGameState(GameState.Running);
				
		errorLeft = errors.Length;
		
		//300 is half of picture size
		Vector2 picturePos = new Vector2(Screen.width/2-300, Screen.height/2-300);
		gameArea = new Rect(picturePos.x, picturePos.y+300, 600, 300);
		foreach(Rect rect in errors) {
			Rect err = rect;
			err.x += picturePos.x;
			err.y += picturePos.y;
			errs.Add(err, false);
		}
		if(InGameMenuGUI.music == null)
		{
		  	InGameMenuGUI.music = (GameObject)Instantiate(musicObject);
			InGameMenuGUI.music.audio.clip = music;
			InGameMenuGUI.music.audio.Play ();
			InGameMenuGUI.music.audio.loop = true;
		}
		audioSource = GetComponent<AudioSource>();
	}
	
	void Hit(Vector2 pos)
	{
		Rect click = new Rect();
		click.width = click.height = clickSize;
		foreach(Rect err in new List<Rect>(errs.Keys)) {
			if(err.Contains(pos)) {
				if(!errs[err]) {
					errs[err] = true;
					click.center = err.center;
					hits.Add(click);
					errorLeft--;
					audioSource.clip = hitSound;
					audioSource.Play();
				}
				return;
			}
		}
		audioSource.clip = missSound;
		audioSource.Play();
		click.center = pos;
		misses.Add (click);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetMouseButtonDown(0)) {
			Vector2 pos = InputManager.MouseScreenToGUI();
			if(GetGameState() != GameState.Paused &&  GetGameState() != GameState.Over && gameArea.Contains(pos)) {
				Hit(pos);
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
		foreach(Rect hit in hits) {
			GUI.DrawTexture(hit, tick);
		}
		
#if UNITY_EDITOR
		foreach(Rect err in errs.Keys) {
			GUI.Box(err, "x");
		}
#endif
	}
}
