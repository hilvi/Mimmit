using UnityEngine;
using System.Collections;

/// <summary>
/// Mud script.
/// Script is attached to an empty game object representing the mud
/// Purpose: slows down the horse and put the texture halfway in the ground 
/// </summary>
public class MudScript : MonoBehaviour {

	public HorseCharacterController horseScript;
	
	void OnTriggerEnter()
	{
		horseScript.EnterMudConfiguration();
		print ("Call");
	}
	void OnTriggerExit()
	{
		horseScript.ExitMudConfiguration();
	}
}
