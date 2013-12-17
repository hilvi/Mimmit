using UnityEngine;
using System.Collections;

public class PuzzleMenuManager : GameManager {

	public GameObject musicObject;
	public AudioClip music;

	public IEnumerator LoadGame (string game)
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
		LoadLevel (game);
		Time.timeScale = 1.0f;
		InGameMenuGUI.music = null;
	}

	// Use this for initialization
	public override void Start () {
		base.Start();

		if (InGameMenuGUI.music == null) {
			InGameMenuGUI.music = (GameObject)Instantiate (musicObject);
			InGameMenuGUI.music.audio.clip = music;
			InGameMenuGUI.music.audio.Play ();
			InGameMenuGUI.music.audio.loop = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
