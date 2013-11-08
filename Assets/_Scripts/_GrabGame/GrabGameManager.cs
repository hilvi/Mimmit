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
	public AudioClip hitSound;
	public AudioClip missSound;
	public float speedMultiplier = 2;
	Shader _diffuse;
	float _worldWidth;
	float _worldHeight;
	private float _timer;
	private int _collectables = 0;
	private AudioSource _audioSource;
	private CharacterWidgetScript _characterWidget;
	private List<GameObject> _objectsOnScreen = new List<GameObject>();
	
	// Use this for initialization
	public override void Start ()
	{
		base.Start ();
		
		_diffuse = Shader.Find ("Diffuse");
		_audioSource = GetComponent<AudioSource> ();
		_characterWidget = GetComponent<CharacterWidgetScript>();
		
		if (InGameMenuGUI.music == null) {
			InGameMenuGUI.music = (GameObject)Instantiate (musicObject);
			InGameMenuGUI.music.audio.clip = music;
			InGameMenuGUI.music.audio.Play ();
			InGameMenuGUI.music.audio.loop = true;
		}
		
		Vector3 __worldSize = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width - 120, Screen.height, 0));
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
		_timer += Time.deltaTime;
		if (_timer > frequency && frequency != 0) {
			SpawnRandomObject ();
			_timer = 0;
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
		float __halfScreen = Screen.width / 2;
		float __startPos = __halfScreen - (__width + __offset) * missesAllowed / 2;
		
		Rect __pos = new Rect (__startPos, __width / 2, __width, __width);
		
		for (int i = 0; i < missesAllowed; i++) {
			GUI.DrawTexture (__pos, cross);
			__pos.x += __offset + __width;
		}
	}
	
	void SpawnRandomObject ()
	{
		//Not very efficient..
		int id;
		do {
			id = Random.Range (0, fallingObjects.Length);
		} while(fallingObjects[id].collected);
			
		InstantiateFallingObject (fallingObjects [id], id);
	}
	
	void InstantiateFallingObject (FallingObjectSettings settings, int id)
	{
		GameObject __obj = Instantiate (fallingObjectPrefab) as GameObject;
		
		FallingObjectScript __script = __obj.GetComponent<FallingObjectScript> ();
		__script.fallingSpeed = Random.Range (settings.minSpeed, settings.maxSpeed)*speedMultiplier;
		__script.oscillation = Random.Range (settings.minOscillationAmplitude, settings.maxOscillationAmplitude);
		__script.manager = this;
		__script.collect = settings.collect;
		__script.id = id;
		
		Material __mat = new Material (_diffuse);
		__mat.mainTexture = settings.texture;
		__obj.renderer.material = __mat;
		
		float __size = __obj.transform.localScale.x;
		__obj.transform.position = new Vector3 (Random.Range (__size - _worldWidth, _worldWidth - __size), _worldHeight + __size, 0);
		
		_objectsOnScreen.Add(__obj);
	}
	
	void GameOver()
	{
		frequency = 0;
		foreach(GameObject obj in _objectsOnScreen)
		{
			Destroy(obj);
		}
		_objectsOnScreen.Clear();
		GameObject.Find("Player").SetActive(false);
	}
	
	public void ObjectCollected (int id, bool collect)
	{
		if (GetGameState () == GameState.Running) {
			if (fallingObjects [id].collect) {
				if (!fallingObjects [id].collected) {
					fallingObjects [id].collected = true;
					_collectables--;
				}
				if (hitSound != null) {
					_audioSource.clip = hitSound;
					_audioSource.Play ();
				}
				_characterWidget.TriggerHappyEmotion();
			} else {
				missesAllowed--;
				if (missSound != null) {
					_audioSource.clip = missSound;
					_audioSource.Play ();
				}
				_characterWidget.TriggerSadEmotion();
			}
		}
		
		if (_collectables == 0) {
			SetGameState (GameState.Won);
			GameOver();
		} else if (missesAllowed == 0) {

			SetGameState (GameState.Lost);
			GameOver();
		}
	}
}
