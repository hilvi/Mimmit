using UnityEngine;
using System.Collections;

public class HorseGameManager : GameManager {

	#region UNITY_METHODS
	public override void Start () 
	{
		base.Start ();
		SetGameState(GameState.Running);
		print("First"+GetGameState());
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	#endregion 
}
