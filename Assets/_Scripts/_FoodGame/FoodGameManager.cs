using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FoodGameManager : GameManager
{
    #region MEMBERS
    public GameObject musicObject;
    public AudioClip music;

    #endregion

    #region UNITY_METHODS
    public override void Start()
    {
        // Boilerplate
        base.Start();
        SetGameState(GameState.Running);

        if (InGameMenuGUI.music == null)
        {
            InGameMenuGUI.music = (GameObject)Instantiate(musicObject);
            InGameMenuGUI.music.audio.clip = music;
            InGameMenuGUI.music.audio.Play();
        }
    }

    #endregion
}