using UnityEngine;
using System.Collections.Generic;

public class DiffGameManager : GameManager
{
	#region MEMBERS
	public Rect[] errors;
	public Dictionary<Rect, bool> errs = new Dictionary<Rect, bool> ();
	public Texture2D tick;
	public Texture2D cross;
	public float clickSize;
	public GUIText text;
	public Texture2D originalPicture;
	public Texture2D errorPicture;
	public GameObject musicObject;
	public AudioClip music;
	public AudioClip missSound;
	public AudioClip hitSound;
	
	private Texture2D _borderOrigPic, _borderErrPic;
	
	private int _errorLeft;
	private AudioSource _audioSource;
	private Dictionary<Rect, float> _misses = new Dictionary<Rect, float> ();
	private List<Rect> _hits = new List<Rect> ();
	//private Rect _gameArea;
	private Rect _leftFrame, _rightFrame;
	
	private CircularCounterScript _counter;
	#endregion
	
	#region UNITY_METHODS
	public override void  Start ()
	{
		base.Start ();
		SetGameState (GameState.Running);

		_errorLeft = errors.Length;
		
		//300 is half of picture size
		//Vector2 picturePos = new Vector2 (Screen.width / 2 - 300, Screen.height / 2 - 300);
		//_gameArea = new Rect (picturePos.x, picturePos.y + 300, 600, 300);
		foreach (Rect rect in errors) {
			//Rect err = rect;
			//err.x += picturePos.x;
			//err.y += picturePos.y;
			//errs.Add (err, false);
			errs.Add(rect, false);
		}
		
		if (InGameMenuGUI.music == null) {
			InGameMenuGUI.music = (GameObject)Instantiate (musicObject);
			InGameMenuGUI.music.audio.clip = music;
			InGameMenuGUI.music.audio.Play ();
			InGameMenuGUI.music.audio.loop = true;
		}
		_audioSource = GetComponent<AudioSource> ();
		
		_leftFrame = new Rect (20f, 20f + (560f / 2.25f) - (321f / 2f), 450f, 321f);
		_rightFrame = _leftFrame;
		_rightFrame.x += _leftFrame.width + 20f;
		
		_borderOrigPic = _CreateBorders(originalPicture, Color.black, 10);
		_borderErrPic = _CreateBorders(errorPicture, Color.black, 10);
		
		_counter = GetComponent<CircularCounterScript>();
		_counter.SetPosition(Screen.width / 2f - 50f, 40f);
		
		text.enabled = false;
	}

	void Update ()
	{
		// Check if player found an error
		if (Input.GetMouseButtonDown (0)) {
			Vector2 pos = InputManager.MouseScreenToGUI ();
			if (GetGameState () != GameState.Paused && 
				GetGameState () != GameState.Won &&
				_rightFrame.Contains(pos)) {
				_Hit (pos);
			}
		}
		
		// End game if all errors are found
		if (_errorLeft == 0)
			SetGameState (GameState.Won);
		text.text = _errorLeft.ToString (); 
		
		// Update counter
		_counter.SetActiveSectors(7 - _errorLeft);
		
		// Reduce alpha on all crosses, remove from dict if alpha = 0f
		var __keys = new List<Rect>(_misses.Keys);
		foreach (var k in __keys) {
			float newAlpha = _misses[k] - Time.deltaTime;
			newAlpha = Mathf.Clamp(newAlpha, 0f, 2f);
			
			if (newAlpha <= 0f)
				_misses.Remove(k);
			else
				_misses[k] = newAlpha;
		}
	}
	
	void OnGUI ()
	{
		GUI.DrawTexture (_leftFrame, _borderOrigPic);
		GUI.DrawTexture (_rightFrame, _borderErrPic);
		
		foreach (var m in _misses) {
			Color tempCol = GUI.color;
			GUI.color = new Color(1f, 1f, 1f, m.Value);
			GUI.DrawTexture (m.Key, cross);
			GUI.color = tempCol;
		}

		foreach (Rect hit in _hits) {
			GUI.DrawTexture (hit, tick);
		}
		
		#if UNITY_EDITOR
		for (int i = 0; i < errors.Length; i++) {
			GUI.Box(errors[i], i.ToString());
		}
		#endif
		
		_counter.Draw();
	}
	#endregion
	
	#region METHODS
	private void _Hit (Vector2 pos)
	{
		Rect click = new Rect ();
		click.width = click.height = clickSize;
		foreach (Rect err in new List<Rect>(errs.Keys)) {
			if (err.Contains (pos)) {
				if (!errs [err]) {
					errs [err] = true;
					click.center = err.center;
					_hits.Add (click);
					_errorLeft--;
					_audioSource.clip = hitSound;
					_audioSource.Play ();
				}
				return;
			}
		}
		_audioSource.clip = missSound;
		_audioSource.Play ();
		click.center = pos;
		_misses.Add (click, 2f);
	}
	
	private Texture2D _CreateBorders (Texture2D a, Color borderColor, int borderThickness)
	{
		Texture2D __newTexture = new Texture2D (a.width, a.height);
		for (int y = 0; y < a.height; y++) {
			for (int x = 0; x < a.width; x++) {
				if (y < borderThickness || 
					y > a.height - borderThickness ||
					x < borderThickness ||
					x > a.width - borderThickness) {
					__newTexture.SetPixel (x, y, borderColor);
				} else {
					__newTexture.SetPixel (x, y, a.GetPixel (x, y));
				}
			}
		}
		
		__newTexture.Apply ();
		return __newTexture;
	}
	#endregion
}
