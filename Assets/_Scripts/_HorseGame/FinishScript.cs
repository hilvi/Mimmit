using UnityEngine;
using System.Collections;

public class FinishScript : MonoBehaviour {
	#region MEMBERS
	public HorseGameManager manager;
	public BirdScript birdScript;
	#endregion
	
	#region UNITY_METHODS

	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.name == "Player")
		{
			manager.PlayerFinish();
		}
		if(col.gameObject.name == "Bird")
		{
			manager.BirdFinish();
		}
	}

	#endregion
	
	#region METHODS
	#endregion
}
