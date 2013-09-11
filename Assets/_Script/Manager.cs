using UnityEngine;
using System.Collections;

public enum Character{
	None, Blonde, Brune
}

public static class Manager {

	static Character _character = Character.None;
	
	public static Character GetCharacter()
	{
		return _character;
	}
	public static void SetCharacter(Character chosenCharacter)
	{
		_character = chosenCharacter;
	}
}
