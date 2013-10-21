using UnityEngine;
using System.Collections.Generic;

public class DiffGameManager : GameManager 
{
	#region MEMBERS
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
	
	private AudioSource audioSource;
	private List<Rect> _misses = new List<Rect>();
	private List<Rect> _hits = new List<Rect>();
	private Rect _gameArea;
	#endregion
	
	#region UNITY_METHODS
	public override void  Start () 
	{
		base.Start ();
		SetGameState(GameState.Running);
				
		errorLeft = errors.Length;
		
		//300 is half of picture size
		Vector2 picturePos = new Vector2(Screen.width/2-300, Screen.height/2-300);
		_gameArea = new Rect(picturePos.x, picturePos.y+300, 600, 300);
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
		void Update () 
	{
		if(Input.GetMouseButtonDown(0)) {
			Vector2 pos = InputManager.MouseScreenToGUI();
			if(GetGameState() != GameState.Paused &&  GetGameState() != GameState.Won && _gameArea.Contains(pos)) {
				Hit(pos);
			}
		}
		if(errorLeft == 0)SetGameState(GameState.Won);
		text.text = errorLeft.ToString (); 
	}
	
	void OnGUI()
	{
		foreach(Rect miss in _misses) {
			GUI.DrawTexture(miss, cross);
		}
		foreach(Rect hit in _hits) {
			GUI.DrawTexture(hit, tick);
		}
		
#if UNITY_EDITOR
		foreach(Rect err in errs.Keys) {
			GUI.Box(err, "x");
		}
#endif
	}
	#endregion
	
	#region METHODS
	void Hit(Vector2 pos)
	{
		Rect click = new Rect();
		click.width = click.height = clickSize;
		foreach(Rect err in new List<Rect>(errs.Keys)) {
			if(err.Contains(pos)) {
				if(!errs[err]) {
					errs[err] = true;
					click.center = err.center;
					_hits.Add(click);
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
		_misses.Add (click);
	}
	#endregion
}
