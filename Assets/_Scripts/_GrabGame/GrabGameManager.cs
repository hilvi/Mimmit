using UnityEngine;
using System.Collections.Generic;

public class GrabGameManager : GameManager
{
	public GameObject musicObject;
	public AudioClip music;
	public FallingObjectSettings[] fallingObjects;
	public GameObject fallingObjectPrefab;
	public float frequency = 3;
	public float missesAllowed = 3;
	public Texture2D tick;
	public Texture2D cross;
	Shader _diffuse;
	float _worldWidth;
	float _worldHeight;
	private float _timer;
	private int _collectables = 0;
	
	// Use this for initialization
	public override void Start ()
	{
		base.Start ();
		
		_diffuse = Shader.Find ("Diffuse");
		
		if (InGameMenuGUI.music == null) {
			InGameMenuGUI.music = (GameObject)Instantiate (musicObject);
			InGameMenuGUI.music.audio.clip = music;
			InGameMenuGUI.music.audio.Play ();
			InGameMenuGUI.music.audio.loop = true;
		}
		
		Vector3 __worldSize = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width-100, Screen.height, 0));
		_worldWidth = __worldSize.x;
		_worldHeight = __worldSize.y;
		
		foreach (FallingObjectSettings settings in fallingObjects) {
			if (settings.collect)
				_collectables++;
		}
		
		SetGameState (GameState.Running);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (_collectables == 0) {
			SetGameState (GameState.Won);
		} else if (missesAllowed == 0) {
			SetGameState (GameState.Lost);
		} else {
			_timer += Time.deltaTime;
			if (_timer > frequency) {
				SpawnRandomObject ();
				_timer = 0;
			}
		}
	}
	
	void OnGUI ()
	{
		DrawCollectable ();
		DrawAvoidable ();
	}
	
	void DrawCollectable ()
	{
		Rect __pos = new Rect (Screen.width - 100, 140, 80, 80);
		float __offset = 10;
		
		foreach (FallingObjectSettings settings in fallingObjects) {
			if (settings.collect) {
				GUI.DrawTexture (__pos, settings.texture);
				if (settings.collected)
					GUI.DrawTexture (__pos, tick);
				__pos.y += __offset + __pos.height;
			}
		}
	}
	
	void DrawAvoidable ()
	{
		float __width = 50;
		float __offset = 10;
		float __halfScreen = Screen.width/2;
		float __startPos = __halfScreen - (__width + __offset) * missesAllowed / 2;
		
		Rect __pos = new Rect (__startPos, __width/2, __width, __width);
		
		for(int i = 0; i < missesAllowed; i++) {
				GUI.DrawTexture (__pos, cross);
				__pos.x += __offset + __width;
		}
	}
	
	void SpawnRandomObject ()
	{
		int id = Random.Range (0, fallingObjects.Length);
		InstantiateFallingObject (fallingObjects [id], id);
	}
	
	void InstantiateFallingObject (FallingObjectSettings settings, int id)
	{
		GameObject __obj = Instantiate (fallingObjectPrefab) as GameObject;
		
		FallingObjectScript __script = __obj.GetComponent<FallingObjectScript> ();
		__script.fallingSpeed = Random.Range (settings.minSpeed, settings.maxSpeed);
		__script.manager = this;
		__script.collect = settings.collect;
		__script.id = id;
		
		Material __mat = new Material (_diffuse);
		__mat.mainTexture = settings.texture;
		__obj.renderer.material = __mat;
		
		float __size = __obj.transform.localScale.x;
		__obj.transform.position = new Vector3 (Random.Range (__size - _worldWidth, _worldWidth - __size), _worldHeight + __size, 0);
	}
	
	public void ObjectCollected (int id, bool collect)
	{
		if (fallingObjects [id].collect && !fallingObjects [id].collected) {
			if(!fallingObjects [id].collected) {
				fallingObjects [id].collected = true;
				_collectables--;
			}
		} else {
			missesAllowed--;
		}
	}
}
