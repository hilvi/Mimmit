using UnityEngine;
using System.Collections;

public class NavigationGUIScript : Overlay {
	public Texture PlayButton,homeButton,PauseButton;
	
	void OnGUI() 
	{		
		float screenUnitW = Screen.width/100;
		NavigationState currentState = Manager.GetNavigationState();
		// While the game is in progress, only display the pause button
		if (currentState == NavigationState.Running) 
		{
			if (GUI.Button(new Rect(Screen.width - screenUnitW*10, 0, (Screen.width/10), (Screen.width/10)), PauseButton, MGUI.noStyle)) 
			{	
				Manager.SetNavigationState(NavigationState.Pause);
			}
		}
		else if(currentState == NavigationState.Pause)
		{
			ShowBottomMenu();
		}	
	}
	
	void ShowBottomMenu()
	{
		// Left button
		if (MGUI.HoveredButton(new Rect(MGUI.margin*3, Screen.height - (Screen.width/6), Screen.width/7, Screen.width/7), homeButton)) 
		{
			LoadLevel("ChoiceScene");
			Time.timeScale = 1.0f;
			Manager.SetNavigationState(NavigationState.Running);
			
		}
		// Right button
		if (MGUI.HoveredButton(new Rect(Screen.width - (Screen.width/3 - Screen.width/7), Screen.height - (Screen.width/6), Screen.width/7, Screen.width/7), PlayButton)) {
			Manager.SetNavigationState(NavigationState.Running);
		}
	}
}
