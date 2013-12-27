using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public enum Pattern
{
	Random,
	LeftToRight,
	RightToLeft
}

public class GrabGameManager : GameManager
{
	public GameObject musicObject;
	public AudioClip music;
	public GameObject fallingObjectPrefab;
	float _frequency = 3;
	public float missesAllowed = 3;
	public Texture2D tick;
	public Texture2D cross;
	public AudioClip hitSound;
	public AudioClip missSound;
	public float speedMultiplier = 1;
	int _spawnLanes;
	Pattern[] _patterns;
	public GrabLevel[] levels;
	public GrabObjects objects;
	Shader _diffuse;
	float _worldWidth;
	float _worldHeight;
	FallingObjectSettings[] _fallingObjects;
	private float _timer;
	private int _collectables = 0;
	private AudioSource _audioSource;
	private CharacterWidgetScript _characterWidget;
	private List<GameObject> _objectsOnScreen = new List<GameObject>();
	private float[] _lanes;
	private bool patternFinished = true;
	private GUIStyle _counterStyle = new GUIStyle();
    private int _spawnCounter = 0;
	private HarakkaScript _harakka;
	private int _level = 0;
	private GameObject _player;

	public GameObject bird, brune, blonde, fox, boy;

    private List<FallingObjectSettings> _fallOrder;

	public override void GoToNextLevel ()
	{
		_level++;
		if(_level < levels.Length)
			StartCoroutine (FadeAndLoad());
		else
			StartCoroutine (LoadWinScene ());
	}

	public override void RestartGame ()
	{
		GameOver ();
		Time.timeScale = 1;

		StartCoroutine(FadeAndLoad());
	}

	IEnumerator LoadWinScene ()
	{
		AudioSource source = InGameMenuGUI.music.GetComponent<AudioSource>();
		Time.timeScale = 1.0f;
		if (source != null) 
		{
			while (source.volume > 0) 
			{
				source.volume -= 0.02f;	
				yield return null;
			}
		}
		LoadLevel ("WinScene");
		Time.timeScale = 1.0f;
		InGameMenuGUI.music = null;
	}

	IEnumerator FadeAndLoad() {
		yield return StartCoroutine (_fade.FadeOut());
		SetGameState(GameState.Pregame);
		InitiateLevel();
		yield return StartCoroutine (_fade.FadeIn());

		Time.timeScale = 1;
		SetGameState(GameState.Running);
	}

	void InitiateLevel() {
		levels[_level]._objects = objects;
		_fallingObjects = levels[_level].GetFallingObjects();
		_spawnLanes = levels[_level].lanes;
		_frequency = levels[_level].frequency;
		missesAllowed = levels[_level].missesAllowed;
		_patterns = levels[_level].patterns;
		
		_collectables = 0;
		for(int i = 0; i < _fallingObjects.Length; i++) {
			if (_fallingObjects[i].collect) {
				_collectables += _fallingObjects[i].numberToCollect;
			}
			_fallingObjects[i].id = i;
		}
		
		_fallOrder = new List<FallingObjectSettings>(_fallingObjects);
		
		InitiateLanes(_spawnLanes);

		ShuffleFallingObjects();

		_player.SetActive(true);
	}
	
	// Use this for initialization
	public override void Start ()
	{
		base.Start ();
		
		_diffuse = Shader.Find ("Transparent/Diffuse");
		_audioSource = GetComponent<AudioSource> ();
		_characterWidget = GameObject.Find("CharacterWidget").GetComponent<CharacterWidgetScript>();

		_harakka = GameObject.Find ("Harakka").GetComponent<HarakkaScript>();

		switch(Manager.GetCharacter()) {
		default:
			_player = Instantiate(bird) as GameObject;
			break;
		}
		
		if (InGameMenuGUI.music == null) {
			InGameMenuGUI.music = (GameObject)Instantiate (musicObject);
			InGameMenuGUI.music.audio.clip = music;
			InGameMenuGUI.music.audio.Play ();
			InGameMenuGUI.music.audio.loop = true;
		}
		
		Vector3 __worldSize = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width - 150, Screen.height, 0));
		_worldWidth = __worldSize.x;
		_worldHeight = __worldSize.y;



		_counterStyle.font = (Font)Resources.Load ("Fonts/Gretoon");
		_counterStyle.fontSize = 50;
		_counterStyle.normal.textColor = Color.white;
		_counterStyle.alignment = TextAnchor.MiddleCenter;

		_level = currentLevel - 1;
		InitiateLevel();

		SetGameState(GameState.Running);
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

		foreach (FallingObjectSettings settings in _fallingObjects) {
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
		yield return new WaitForSeconds(_frequency);
		Pattern pattern = _patterns[Random.Range (0, _patterns.Length)];
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
		}
		patternFinished = true;
	}

	IEnumerator SpawnLeftToRightPattern ()
	{
		for(int i = 0; i < _spawnLanes; i++)
		{
			StartCoroutine(InstantiateFallingObject(GetObjectId(), i));
			yield return new WaitForSeconds(_frequency/2);
		}
	}

	IEnumerator SpawnRightToLeftPattern ()
	{
		for(int i = _spawnLanes-1; i >= 0; i--)
		{
			StartCoroutine(InstantiateFallingObject(GetObjectId(), i));
			yield return new WaitForSeconds(_frequency/2);
		}
	}

	IEnumerator SpawnRandomObject ()
	{
		//Not very efficient..
		int __lane;
		do {
			__lane = Random.Range (0, _spawnLanes);
			yield return null;
		} while(!CheckIfLaneFree(__lane));

		StartCoroutine(InstantiateFallingObject (GetObjectId(), __lane));
	}

	IEnumerator SpawnAllAtOnce()
	{
		for(int i = 0; i < _spawnLanes; i++)
		{
			StartCoroutine(InstantiateFallingObject(GetObjectId(), i));
			yield return null;
		}
		yield return new WaitForSeconds(_frequency);
	}

	int GetObjectId()
	{
        do
        {
            _spawnCounter++;
            if (_spawnCounter >= _fallOrder.Count)
            {
                _spawnCounter = 0;
                ShuffleFallingObjects();
            }
        } while (_fallOrder[_spawnCounter].numberToCollect == 0 && _fallOrder[_spawnCounter].collect);

        return _fallOrder[_spawnCounter].id;
	}

    void ShuffleFallingObjects()
    {
        for (int i = 0; i < _fallOrder.Count; i++)
        {
            FallingObjectSettings tmp;
            int newPos = Random.Range(0, _fallOrder.Count);

            tmp = _fallOrder[newPos];
            _fallOrder[newPos] = _fallOrder[i];
            _fallOrder[i] = tmp;
        }
    }

	bool CheckIfLaneFree(int lane)
	{
		foreach(GameObject obj in _objectsOnScreen) {
			FallingObjectScript script;
			if((script = obj.GetComponent<FallingObjectScript>()) != null) {
				if(script.lane == lane)
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
	
	IEnumerator InstantiateFallingObject (int id, int lane)
	{
		if(GetGameState() != GameState.Running)
			yield break;

		FallingObjectSettings settings = _fallingObjects[id];


		GameObject __obj = Instantiate (fallingObjectPrefab) as GameObject;
		FallingObjectScript __script = __obj.GetComponent<FallingObjectScript> ();
		__script.fallingSpeed = Random.Range (settings.minSpeed, settings.maxSpeed)*speedMultiplier;
		__script.oscillation = Random.Range (settings.minOscillationAmplitude, settings.maxOscillationAmplitude);
		__script.manager = this;
		__script.collect = settings.collect;
        __script.id = id;
		__script.lane = lane;

		
		Material __mat = new Material (_diffuse);
		__mat.mainTexture = settings.texture;
		__obj.renderer.material = __mat;

		float __size = __obj.transform.localScale.x*2;
		if(_spawnLanes == 0) {
			__obj.transform.position = new Vector3 (Random.Range (__size - _worldWidth, _worldWidth - __size), _worldHeight + __size, 0);
		} else {
			__obj.transform.position = new Vector3(_lanes[lane], _worldHeight + __size, 0);
		}

		while(_harakka.moving)
			yield return null;

		_harakka.StartCoroutine("MoveToPosAndThrow", __obj);

		_objectsOnScreen.Add(__obj);
	}
	
	void GameOver()
	{
		_frequency = 0;
		foreach(GameObject obj in _objectsOnScreen)
		{
			Destroy(obj);
		}
		_objectsOnScreen.Clear();
		_harakka.End();
		_player.SetActive(false);
	}
	
	public void ObjectCollected (int id, bool collect)
	{
		if (GetGameState () == GameState.Running) {
			if (_fallingObjects [id].collect) {
				if (_fallingObjects [id].numberToCollect != 0) {
					_fallingObjects [id].numberToCollect--;
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
