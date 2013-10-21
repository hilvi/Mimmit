using UnityEngine;
using System.Collections;
/// <summary>
/// Map world script.
/// Defines which character will be used on the map scene based on the choice made previously.
/// The value is taken from the Manager class.
/// A new character object is created and placed on the map.
/// </summary>
public class MapWorldScript : MonoBehaviour {
	
	public GameObject blonde, brune;
	public Transform startPosition;
	GameObject girl;
	void Awake () 
	{
		if(Manager.GetCharacter() == Character.Blonde)
		{
			girl = (GameObject)Instantiate (blonde);	
		}
		else if(Manager.GetCharacter() == Character.Brune)
		{
			girl = (GameObject)Instantiate(brune);
		}else{
			#if UNITY_EDITOR
			girl = (GameObject)Instantiate(brune);
			#endif
			//Debug.LogError("No girl chosen in MapWorld");
		}
		girl.name = "Girl";
		girl.transform.position = startPosition.position;
	}
	public GameObject GetGirl()
	{
		return girl;
	}
}
