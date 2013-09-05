using UnityEngine;
using System.Collections;

public class MapWorldScript : MonoBehaviour {

	public GameObject blonde, brune;
	public Transform startPosition;
	GameObject girl;
	void Awake () {
		if(Manager.GetCharacter() == Character.Blonde)
		{
			girl = (GameObject)Instantiate (blonde);	
		}
		else if(Manager.GetCharacter() == Character.Brune)
		{
			girl = (GameObject)Instantiate(brune);
		}else{
			Debug.LogError("No girl chosen in MapWorld");
		}
		girl.name = "Girl";
		girl.transform.position = startPosition.position;
	}
	public GameObject GetGirl()
	{
		return girl;
	}
}
