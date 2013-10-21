using UnityEngine;
using System.Collections;

public enum Character{
	None, Blonde, Brune, Fox, Boy
}
public enum ScreenChoice{
	None, Map, Button
}
public enum NavigationState{
	Running, Pause
}
public static class Manager {

	static Character _character = Character.None;
	static ScreenChoice _choice = ScreenChoice.None;
	static NavigationState _navState = NavigationState.Running;
	
	public static Character GetCharacter()
	{
		return _character;
	}
	public static void SetCharacter(Character chosenCharacter)
	{
		_character = chosenCharacter;
	}
	public static ScreenChoice GetScreenChoice()
	{
		return _choice;
	}
	public static void SetScreenChoice(ScreenChoice chosenScreenMode)
	{
		_choice = chosenScreenMode;
	}
	public static NavigationState GetNavigationState()
	{
		return _navState;
	}
	public static void SetNavigationState(NavigationState navigationState)
	{
		_navState = navigationState;
	}
}

