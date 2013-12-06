using UnityEngine;
using System.Collections;

public class NavigationGUIScript : Overlay {
	#region MEMBERS
	public Texture PlayButton,homeButton,PauseButton;
	private Rect _buttonRect;
	#endregion
	
	#region UNITY_METHODS
	public override void Awake () {
		base.Awake();
		float screenUnitW = Screen.width/10;
		_buttonRect = new Rect(Screen.width - screenUnitW, 0, (Screen.width/10), (Screen.width/10));
	}
	#endregion
	
	#region METHODS
	public void Draw() 
	{		
		NavigationState currentState = Manager.GetNavigationState();
		// While the game is in progress, only display the pause button
		if (currentState == NavigationState.Running) 
		{
			if (GUI.Button(_buttonRect, PauseButton, MGUI.noStyle)) 
			{	
				Manager.SetNavigationState(NavigationState.Pause);
			}
		}
		else if(currentState == NavigationState.Pause)
		{
			ShowBottomMenu();
		}	
	}

	private void ShowBottomMenu()
	{
		// Left button
		if (MGUI.HoveredButton(new Rect(MGUI.margin*3, Screen.height - (Screen.width/6), Screen.width/7, Screen.width/7), homeButton)) 
		{
			if(Application.CanStreamedLevelBeLoaded("ChoiceScene"))
			{
				Application.LoadLevel("ChoiceScene");
			}
			Time.timeScale = 1.0f;
			Manager.SetNavigationState(NavigationState.Running);
			
		}
		// Right button
		if (MGUI.HoveredButton(new Rect(Screen.width - (Screen.width/3 - Screen.width/7), Screen.height - (Screen.width/6), Screen.width/7, Screen.width/7), PlayButton)) {
			Manager.SetNavigationState(NavigationState.Running);
		}
	}
	#endregion
}
