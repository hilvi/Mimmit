using UnityEngine;
using System.Collections;

/// <summary>
/// In game menu GUI.
/// Attached to the game manager object
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class InGameMenuGUI : MonoBehaviour {
	
	public Texture gameTitleTexture;
	public float gamePreviewWidthToScreenWidthRatio = 0.75f;
	public float barHeightToScreenHeightRatio = 0.25f;
	public float gamePreviewArrowHeightRation = 0.2f;//  height ration of the white speach arrow pointing to character to total height of preview screen
	public static int currentLevel = 1;
	public static string selectedGameName;
	public string[] gameList;
	
	GameManager gameManager;
	Texture Restart, PlayButton, MainMenuButton, PauseButton, pauseTexture;
	Texture soundON, soundOff;
	
	int gamesNumber;
	
	AudioSource audioSource;
	bool callOnce = true;
	
	string isSounON;
	Texture[] previewTextures;	
	Rect creditsRect;
	
	
	
	// Use this for initialization
	void Start () {
		
		
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		PlayButton = (Texture)Resources.Load("MenuCommon/play_" + MainMenuGUI.selectedGameName);
		MainMenuButton = (Texture)Resources.Load("MenuCommon/home_" + MainMenuGUI.selectedGameName);
		Restart = (Texture)Resources.Load("PauseMenu/replay_" + MainMenuGUI.selectedGameName);
		PauseButton = (Texture)Resources.Load("PauseMenu/pause_" + MainMenuGUI.selectedGameName);
		pauseTexture = (Texture)Resources.Load("TutorialMenu/Previews/" + MainMenuGUI.selectedGameName);
		soundON =(Texture)Resources.Load("MainMenu/Buttons/soundon");
		soundOff =(Texture)Resources.Load("MainMenu/Buttons/soundoff");
		
		previewTextures = new Texture[1];
		previewTextures[0] = (Texture)Resources.Load("MainMenu/Previews/brick");
			
		
		audio.clip = (AudioClip)Resources.Load("Music/Medal/MedalScreen");
		audio.volume = 0;
		audio.loop = true;
		audio.playOnAwake = false;
		
		audioSource = Camera.main.GetComponent<AudioSource>();
	
		currentLevel = 1;
		creditsRect = new Rect(Screen.width - MGUI.menuButtonWidth, MGUI.menuButtonWidth*1/3, MGUI.menuButtonWidth*2/3, MGUI.menuButtonWidth*2/3);
		

	}
	
	void OnGUI() {		
		float screenUnitW = Screen.width/100;
		
		// While the game is in progress, only display the pause button
		if ((gameManager.GetGameState() == GameManager.GameState.Running)||(gameManager.GetGameState() == GameManager.GameState.Pregame)) {
			if (GUI.Button(new Rect(Screen.width - screenUnitW*10, 0, (Screen.width/10), (Screen.width/10)), PauseButton, MGUI.NoStyle)) {	
				gameManager.PauseGame();
			}
		}
		else {
				// define the medal and show the corresponding texture
				switch (gameManager.GetGameState()) {
					case GameManager.GameState.Paused: 
						GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), pauseTexture);
						ShowBottomMenu();
						break;
					case GameManager.GameState.Over:
						if(callOnce){
							audio.volume = 0;
							StartCoroutine(FadeOutMusic(audioSource));
							audio.Play();
							StartCoroutine(FadeInMusic(audio));
							callOnce = false;
						}
					break;
			}
		}	
	}

	
	IEnumerator LoadMainMenu(AudioSource source){
		if (source != null)
		while(source.volume > 0){
			source.volume -= 0.02f;	
			yield return null;
		}
		Time.timeScale = 1.0f;
		Application.LoadLevel("MainMenu");
	}
	IEnumerator FadeInMusic(AudioSource source){
		if (source != null)
		while(source.volume < 1){
			source.volume += 0.02f;	
			yield return null;
		}
	}
	IEnumerator FadeOutMusic(AudioSource source){
		if (source != null)
		while(source.volume > 0){
			source.volume -= 0.02f;	
			yield return null;
		}
	}
	IEnumerator WaitAndLoadNext(){
		yield return StartCoroutine(FadeOutMusic(audio));
		gameManager.GoToNextLevel();
	}
	
	void ShowBottomMenu(){
		// Left button
		if (MGUI.HoveredButton(new Rect(MGUI.Margin*3, Screen.height - (Screen.width/6), Screen.width/7, Screen.width/7), MainMenuButton)) {
			switch(gameManager.GetGameState()){
				case GameManager.GameState.Paused: 
					StartCoroutine(LoadMainMenu(audioSource));
					break;
				case GameManager.GameState.Over:
					StartCoroutine(LoadMainMenu(audio));
					break;
			}
		}
		
		// Middle button
		if (MGUI.HoveredButton(new Rect(Screen.width -(Screen.width/2 + Screen.width/14),Screen.height - (Screen.width/6), Screen.width/7, Screen.width/7), Restart)) {
			gameManager.RestartGame();
		}
		
		// Right button
		if (gameManager.GetGameState() == GameManager.GameState.Over)
			 GUI.enabled = false; // Resume button is grayed out on the loss screen
		
		if (MGUI.HoveredButton(new Rect(Screen.width - (Screen.width/3 - Screen.width/7), Screen.height - (Screen.width/6), Screen.width/7, Screen.width/7), PlayButton)) {
			if (gameManager.GetGameState()== GameManager.GameState.Paused){
				gameManager.UnpauseGame();
				
			}
			if ((gameManager.GetGameState() == GameManager.GameState.Over)){
				StartCoroutine(WaitAndLoadNext());
			}
		}
		GUI.enabled = true;
		
		if(gameManager.GetGameState() == GameManager.GameState.Paused){
			if(isSounON == "true"){
				
				if (MGUI.HoveredButton(creditsRect, soundON)){
					
					PlayerPrefs.SetString("sound", "false");
					isSounON = "false";
					EnableSound();
					
				}
			}
			else{
				
				if (MGUI.HoveredButton(creditsRect, soundOff)){
					
					PlayerPrefs.SetString("sound", "true");
					isSounON = "true";
					EnableSound();
				}
			}
		}
		
	}
	void EnableSound(){	
		if(AudioListener.volume == 0){
			
			AudioListener.volume = 1;
		}
			
		else{
				AudioListener.volume = 0;
		} 			
	}
}