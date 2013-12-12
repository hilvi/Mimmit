using UnityEngine;
using System.Collections;

public class NavigationGUIScript : Overlay {
	#region MEMBERS
	public Texture PlayButton,homeButton,PauseButton;
	private Rect _pauseButtonRegion;
	#endregion
	
	#region UNITY_METHODS
	public override void Awake () {
		base.Awake();
		float __width = Screen.width;
		float __sizeButton = __width / 15;
		
		float __margin = 5f;
		float __offset = 85f;
		_pauseButtonRegion = new Rect(__margin + __offset, __margin, __sizeButton, __sizeButton);
	}
	#endregion
	
	#region METHODS
	public void Draw() 
	{		
		NavigationState currentState = Manager.GetNavigationState();
		// While the game is in progress, only display the pause button
		if (currentState == NavigationState.Running) 
		{
			if (GUI.Button(_pauseButtonRegion, PauseButton, MGUI.noStyle)) 
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
