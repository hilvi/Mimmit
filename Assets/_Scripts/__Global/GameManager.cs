using UnityEngine;
using System.Collections;

public enum GameState 
{ 
	Tutorial,Pregame, Running, Paused, Won, Lost 
}

public class GameManager : Overlay 
{
	#region MEMBERS
	public bool isLastLevel = true;
	public int currentLevel = 1;
	public string gameName;
	
	public static GameState gameState;
	public static GameState prevGameState;
	#endregion
	
	#region UNITY_METHODS
	public override void Awake () 
	{
		base.Awake ();
		if(currentLevel == 1)
            SetGameState(GameState.Tutorial);
		else
            SetGameState(GameState.Pregame); //reset the game state set by previous game, TODO  why do we need static gameState?
		Time.timeScale = 1;
		
		Camera[] cams = (Camera[])FindObjectsOfType(typeof(Camera));
		foreach(Camera c in cams)
		{
			if(c.gameObject.CompareTag("SoundCam"))
			{
				Destroy (c.gameObject);
			}
		}
	}
	public virtual void Start(){}
	#endregion
	
	#region METHODS
	public void SetGameState(GameState s) 
	{
		GameManager.gameState = s;
	}
	
	public GameState GetGameState() 
	{
		return GameManager.gameState;
	}
	
	public void PauseGame() 
	{
		GameManager.prevGameState = GetGameState();
		GameManager.gameState = GameState.Paused;
		Time.timeScale = 0;
	}
	
	public void UnpauseGame() 
	{
		GameManager.gameState = GameManager.prevGameState;
		Time.timeScale = 1;
	}
	
	public void ResumeGame() 
	{
		GameManager.gameState = GameState.Running;
		Time.timeScale = 1;
	}
	
	public void RestartGame() 
	{
		//Reset global time scale
		Time.timeScale = 1;
		LoadLevel(Application.loadedLevelName);
	}
	
	public bool IsGameRunning() 
	{
		if (GetGameState() == GameState.Running) return true;
		return false;
	}
	
	public void GoToNextLevel() 
	{
		//Reset global time scale
		Time.timeScale = 1;
		int i = currentLevel + 1;
		LoadLevel(gameName + i.ToString());
	}
	
	public void EndGame() 
	{
		PauseGame();
		SetGameState(GameState.Won);
		Time.timeScale = 0;
	}
	#endregion
}
