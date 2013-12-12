using UnityEngine;
using System.Collections;

public class CountdownManager : MonoBehaviour {
	
	#region MEMBERS
	public GameObject countdownScript;

	[Range(2,10)]
	public int counter = 3;
	public bool CountdownDone {
		get {
			return _countdownDone;
		}
	}
	private bool _countdownDone;
	private Vector3 _spawnPosition;
	#endregion
	
	#region UNITY_METHODS

	void Update () 
	{
	
	}
	#endregion
	
	#region MEMBERS
	public void SetSpawnPosition(Vector3 position) 
	{
		_spawnPosition = position;
	}
	
    public void StartCountdown(GameManager manager)
    {
        _countdownDone = false;
        StartCoroutine(_Countdown(manager));
    }

	private IEnumerator _Countdown(GameManager manager) 
	{
		GameObject cs = null;
		for (int i = counter; i >= 0; i--) 
		{
			float t = 1f;
			while (t > 0f) {
				if(manager.GetGameState() == GameState.Tutorial)
				{
					yield return null;
				}
				else
				{
					t -= Time.deltaTime;
					yield return null;
				}
			}
			
			if (i != 0) 
			{
				// Numbers
				cs = Instantiate(countdownScript, _spawnPosition, Quaternion.identity) as GameObject;
				cs.GetComponent<CountdownScript>().SetText(i.ToString());
			} 
			else {
				// Go text
				cs = Instantiate(countdownScript, _spawnPosition, Quaternion.identity) as GameObject;
				cs.GetComponent<CountdownScript>().SetText("GO!");
			}
		}
		
		_countdownDone = true;
		this.enabled = false;
		yield return null;
	}
	#endregion
}
