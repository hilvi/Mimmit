using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FoodGameManager : GameManager
{
    #region MEMBERS
    // References
    public GameObject musicObject;
    public AudioClip music;
    public GameObject recipe;
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

        recipe = new GameObject();
        recipe.AddComponent<AppleRecipe>().Build();
    }

    void OnEnable()
    {
        FoodRecipe.OnRecipeDone += _EndGame;
    }

    void OnDisable()
    {
        FoodRecipe.OnRecipeDone -= _EndGame;
    }
    #endregion

    #region METHODS
    private void _EndGame()
    {
        SetGameState(GameState.Won);
    }
    #endregion
}