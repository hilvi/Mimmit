using UnityEngine;
using System.Collections;

public class BirdGameManager : GameManager
{
	public GameObject musicObject;
	public AudioClip music;
	public FallingObjectSettings[] fallingObjects;
	
	private ObjectSpawner _spawner;
	
	// Use this for initialization
	public override void Start ()
	{
		base.Start ();
		
		if (InGameMenuGUI.music == null) {
			InGameMenuGUI.music = (GameObject)Instantiate (musicObject);
			InGameMenuGUI.music.audio.clip = music;
			InGameMenuGUI.music.audio.Play ();
			InGameMenuGUI.music.audio.loop = true;
		}
		
		_spawner = GetComponent<ObjectSpawner>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
