using UnityEngine;
using System.Collections;

public class PandaGameManager : GameManager
{
    public GameObject musicObject;
    public AudioClip music;

	// Use this for initialization
    public override void Start()
    {
        base.Start();
        SetGameState(GameState.Running);

        if (InGameMenuGUI.music == null)
        {
            InGameMenuGUI.music = (GameObject)Instantiate(musicObject);
            InGameMenuGUI.music.audio.clip = music;
            InGameMenuGUI.music.audio.Play();
            InGameMenuGUI.music.audio.loop = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
