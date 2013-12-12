using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterWidgetScript : MonoBehaviour {
	
	#region PUBLIC
	public Texture2D blonde, brune, fox, boy; // TODO swap to animations
	#endregion
	
	#region PRIVATE
	private Dictionary<Character, Texture2D> _roster = new Dictionary<Character, Texture2D>();
    private Rect _characterBoxRect;
	#endregion
	
	#region UNITY_METHODS
	void Awake () 
	{
		_roster.Add(Character.None, blonde); // "None" maps to "Blonde", cover all cases just in case
		_roster.Add(Character.Blonde, blonde);
		_roster.Add(Character.Brune, brune);
		_roster.Add(Character.Fox, fox);
		_roster.Add(Character.Boy, boy);
		float __size = 90f;
        _characterBoxRect = new Rect(0,0,__size,__size);
	}

	void OnGUI() 
	{
		// TODO, playAnimation() etc.
		GUI.DrawTexture(_characterBoxRect, _roster[Manager.GetCharacter()]);
	}
	#endregion
	
	#region METHODS
	public void TriggerHappyEmotion()
	{
		// TODO, make this fancy
		Debug.Log(_roster[Manager.GetCharacter()].name + ": I am happy");
	}
	
	public void TriggerSadEmotion() 
	{
		// TODO, make this fancy
		Debug.Log(_roster[Manager.GetCharacter()].name + ": I am sad");
	}
	#endregion
}
