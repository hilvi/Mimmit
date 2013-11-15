using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public enum Pattern
{
	Random,
	LeftToRight,
	RightToLeft,
	AllAtOnce
}

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
	public int spawnLanes;
	public Pattern[] patterns;
	Shader _diffuse;
	float _worldWidth;
	float _worldHeight;
	private float _timer;
	private int _collectables = 0;
	private AudioSource _audioSource;
	private CharacterWidgetScript _characterWidget;
	private List<GameObject> _objectsOnScreen = new List<GameObject>();
	private float[] _lanes;
	private bool patternFinished = true;
	private GUIStyle _counterStyle = new GUIStyle();
	
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
		
		Vector3 __worldSize = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width - 150, Screen.height, 0));
		_worldWidth = __worldSize.x;
		_worldHeight = __worldSize.y;
		
		foreach (FallingObjectSettings settings in fallingObjects) {
			if (settings.collect) {
				_collectables += settings.numberToCollect;
			}
		}

		InitiateLanes(spawnLanes);

		_counterStyle.font = (Font)Resources.Load ("Fonts/Gretoon");
		_counterStyle.fontSize = 50;
		_counterStyle.normal.textColor = Color.white;
		_counterStyle.alignment = TextAnchor.MiddleCenter;


		SetGameState (GameState.Running);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(patternFinished)
			StartCoroutine(Spawner());
	}
	
	void OnGUI ()
	{
		DrawCollectable ();
		DrawLife ();
	}

	void DrawCollectable ()
	{
		float __width = 50;
		Rect __pos = new Rect (0, 140, __width, __width);
		float __offset = 10;

		float __counterPos = Screen.width - __width * 2 - __offset;
		float __picturePos = Screen.width - __width - __offset;

		foreach (FallingObjectSettings settings in fallingObjects) {
			if (settings.collect) {
				__pos.x = __picturePos;
				GUI.DrawTexture (__pos, settings.texture);

				if (settings.numberToCollect == 0)
					GUI.DrawTexture (__pos, tick);
				else {
					__pos.x = __counterPos;
					GUI.Label (__pos, settings.numberToCollect.ToString(), _counterStyle);
				}

				__pos.y += __offset + __pos.height;
			}
		}
	}
	
	void DrawLife ()
	{
		float __width = 50;
		float __offset = 10;

		/*float __halfScreen = Screen.width / 2;
		float __startPos = __halfScreen - (__width + __offset) * missesAllowed / 2;
		Rect __pos = new Rect (__startPos, __width / 2, __width, __width);*/

		Rect __pos = new Rect (__width/2, 150, __width, __width);
		
		for (int i = 0; i < missesAllowed; i++) {
			GUI.DrawTexture (__pos, cross);
			__pos.y += __offset + __width;
		}
	}

	IEnumerator Spawner ()
	{
		patternFinished = false;
		yield return new WaitForSeconds(frequency);
		Pattern pattern = patterns[Random.Range (0, patterns.Length)];
		switch(pattern) {
		case Pattern.Random:
			yield return StartCoroutine(SpawnRandomObject());
			break;
		case Pattern.LeftToRight:
			yield return StartCoroutine(SpawnLeftToRightPattern());
			break;
		case Pattern.RightToLeft:
			yield return StartCoroutine (SpawnRightToLeftPattern());
			break;
		case Pattern.AllAtOnce:
			yield return StartCoroutine (SpawnAllAtOnce());
			break;
		}
		patternFinished = true;
	}

	IEnumerator SpawnLeftToRightPattern ()
	{
		for(int i = 0; i < spawnLanes; i++)
		{
			InstantiateFallingObject(GetObjectId(), i);
			yield return new WaitForSeconds(frequency/2);
		}
	}

	IEnumerator SpawnRightToLeftPattern ()
	{
		for(int i = spawnLanes-1; i >= 0; i--)
		{
			InstantiateFallingObject(GetObjectId(), i);
			yield return new WaitForSeconds(frequency/2);
		}
	}

	IEnumerator SpawnRandomObject ()
	{
		//Not very efficient..
		int __lane;
		do {
			__lane = Random.Range (0, spawnLanes);
			yield return null;
		} while(!CheckIfLaneFree(__lane));

		InstantiateFallingObject (GetObjectId(), __lane);
	}

	IEnumerator SpawnAllAtOnce()
	{
		for(int i = 0; i < spawnLanes; i++)
		{
			InstantiateFallingObject(GetObjectId(), i);
			yield return null;
		}
		yield return new WaitForSeconds(frequency);
	}

	int GetObjectId()
	{
		int __id;
		do {
			__id = Random.Range (0, fallingObjects.Length);
		} while(fallingObjects[__id].numberToCollect == 0 && fallingObjects[__id].collect);

		return __id;
	}

	bool CheckIfLaneFree(int lane)
	{
		foreach(GameObject obj in _objectsOnScreen) {
			if(_lanes[lane] == obj.transform.position.x) {
				return false;
			}
		}
		return true;
	}

	void InitiateLanes(int lanes)
	{
		float __worldSize = 2 * _worldWidth;
		float __laneSize = __worldSize / (lanes-1);
		_lanes = new float[lanes];

		for(int i = 0; i < lanes; i++) {
			_lanes[i] = i * __laneSize - _worldWidth;
		}
	}
	
	void InstantiateFallingObject (int id, int lane)
	{
		if(GetGameState() != GameState.Running)
			return;
		GameObject __obj = Instantiate (fallingObjectPrefab) as GameObject;
		FallingObjectSettings settings = fallingObjects[id];
		
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
		if(spawnLanes == 0) {
			__obj.transform.position = new Vector3 (Random.Range (__size - _worldWidth, _worldWidth - __size), _worldHeight + __size, 0);
		} else {
			__obj.transform.position = new Vector3(_lanes[lane], _worldHeight + __size, 0);
		}
		
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
				if (fallingObjects [id].numberToCollect != 0) {
					fallingObjects [id].numberToCollect--;
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
	public void RemoveObject(GameObject go)
	{
		_objectsOnScreen.Remove(go);
		Destroy(go);
	}
}
