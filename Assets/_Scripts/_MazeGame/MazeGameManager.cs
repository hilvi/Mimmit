using UnityEngine;
using System.Collections;

public class MazeGameManager : GameManager 
{
	#region MEMBERS
	Character _character;
	#endregion
	#region UNITY_METHODS
	public override void Start () 
	{
		base.Start ();
		SetGameState (GameState.Pregame);
		StartCoroutine(_IntroAnim());
		//_character = Manager.GetCharacter();
		//TODO
		// Get appropriate texture based on chosen character
	}

	void Update () 
	{
	
	}
	#endregion
	IEnumerator _IntroAnim()
	{
		float __timer = 0;
		while(__timer < 1)
		{ 
			__timer += Time.deltaTime;
			yield return null;
		}
		SetGameState(GameState.Running);
	}
}
