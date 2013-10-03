using UnityEngine;
using System.Collections;

public class NavigationGUIScript : MonoBehaviour {
	public Texture PlayButton,homeButton,PauseButton;
	
	AudioSource audioSource;
	
	void Awake()
	{
		/*Camera[] cams = (Camera[])FindObjectsOfType(typeof(Camera));
		foreach(Camera c in cams)
		{
			if(c.gameObject != this.gameObject)
			{
				
				Destroy (c.gameObject);
			}
		}*/
	}
	// Use this for initialization
	void Start () 
	{
		//audioSource = cam.GetComponent<AudioSource>();
	}
	
	void OnGUI() 
	{		
		float screenUnitW = Screen.width/100;
		NavigationState currentState = Manager.GetNavigationState();
		// While the game is in progress, only display the pause button
		if (currentState == NavigationState.Running) 
		{
			if (GUI.Button(new Rect(Screen.width - screenUnitW*10, 0, (Screen.width/10), (Screen.width/10)), PauseButton, MGUI.NoStyle)) 
			{	
				Manager.SetNavigationState(NavigationState.Pause);
			}
		}
		else if(currentState == NavigationState.Pause)
		{
			ShowBottomMenu();
		}	
	}

	
	/*IEnumerator LoadMainMenu(AudioSource source){
		if (audioSource != null)
		{
			while(audioSource.volume > 0){
				audioSource.volume -= 0.02f;	
				yield return null;
			}
		}
		
		Manager.SetNavigationState(NavigationState.Running);
		Application.LoadLevel("ChoiceScene");
	}*/
	
	void ShowBottomMenu()
	{
		// Left button
		if (MGUI.HoveredButton(new Rect(MGUI.Margin*3, Screen.height - (Screen.width/6), Screen.width/7, Screen.width/7), homeButton)) 
		{
			//StartCoroutine(LoadMainMenu(audioSource));
			
			Application.LoadLevel("ChoiceScene");
			Time.timeScale = 1.0f;
			Manager.SetNavigationState(NavigationState.Running);
			
		}
		// Right button
		if (MGUI.HoveredButton(new Rect(Screen.width - (Screen.width/3 - Screen.width/7), Screen.height - (Screen.width/6), Screen.width/7, Screen.width/7), PlayButton)) {
			Manager.SetNavigationState(NavigationState.Running);
		}
	}
}
