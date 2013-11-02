using UnityEngine;
using System.Collections;

/// <summary>
/// Mud script.
/// Script is attached to an empty game object representing the mud
/// Purpose: slows down the horse and put the texture halfway in the ground 
/// </summary>
public class MudScript : MonoBehaviour {

	HorseCharacterController _horseScript;
	void Start()
	{
		_horseScript = GameObject.Find ("Player").GetComponent<HorseCharacterController>();
	}
	void OnTriggerEnter()
	{
		_horseScript.EnterMudConfiguration();
	}
	void OnTriggerExit()
	{
		_horseScript.ExitMudConfiguration();
	}
}
