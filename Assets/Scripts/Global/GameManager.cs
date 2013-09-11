using UnityEngine;
using System.Collections;

public enum GameState { Pregame, Running, Paused, Over };
public class GameManager : MonoBehaviour {
	
	public bool isLastLevel = true;
	public int currentLevel = 1;
	public string gameName;
	
	
	public static GameState gameState;
	public static GameState prevGameState;
	
	
	virtual protected void Awake() {		
		MainMenuGUI.selectedGameName = gameName;
		MainMenuGUI.currentLevel = currentLevel;
	}
	
	
	// Use this for initialization
	public virtual void Start () {
		SetGameState(GameState.Pregame); //reset the game state set by previous game, TODO  why do we need static gameState?
		Time.timeScale = 1;
	}
	
	public void SetGameState(GameState s) {
		GameManager.gameState = s;
	}
	
	public GameState GetGameState() {
		return GameManager.gameState;
	}
	
	public void PauseGame() {
		GameManager.prevGameState = GetGameState();
		GameManager.gameState = GameState.Paused;
		Time.timeScale = 0;
	}
	
	public void UnpauseGame() {
		GameManager.gameState = GameManager.prevGameState;
		Time.timeScale = 1;
	}
	
	public void ResumeGame() {
		GameManager.gameState = GameState.Running;
		Time.timeScale = 1;
	}
	
	public void RestartGame() {
		//Reset global time scale
		Time.timeScale = 1;
		Application.LoadLevel(Application.loadedLevel);
	}
	
	public bool IsGameRunning() {
		if (GetGameState() == GameState.Running) return true;
		return false;
	}
	
	public void GoToNextLevel() {
		//Reset global time scale
		Time.timeScale = 1;
		if (!isLastLevel)
			Application.LoadLevel(MainMenuGUI.selectedGameName + "_level_" + (MainMenuGUI.currentLevel+1).ToString());
		else
			Application.LoadLevel("CreditsScreen");	

	}
	
	public void EndGame() {
		PauseGame();
		SetGameState(GameState.Over);
		Time.timeScale = 0;
	}
	protected void GameOverScreen(Texture2D texture)
	{
		
	}
}