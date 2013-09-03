using UnityEngine;
using System.Collections;

public enum Character{
	None, Blonde, Brune
}
public enum Language{
	None, Finnish, English
}
public static class GameManager {

	static Character _character = Character.None;
	static Language _language = Language.None;
	
	public static Character GetCharacter()
	{
		return _character;
	}
	public static void SetCharacter(Character chosenCharacter)
	{
		_character = chosenCharacter;
	}
	public static Language GetLanguage()
	{
		return _language;
	}
	public static void SetLanguage(Language language)
	{
		_language = language;
	}
}
