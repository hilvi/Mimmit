using UnityEngine;
using System.Collections;

public class CountdownManager : MonoBehaviour {
	
	#region MEMBERS
	public GameObject countdownScript;
	
	public bool CountdownDone {
		get {
			return _countdownDone;
		}
	}
	private bool _countdownDone;
	private Vector3 _spawnPosition;
	#endregion
	
	#region UNITY_METHODS
	void Start () {
		_countdownDone = false;
		StartCoroutine(_Countdown());
	}

	void Update () {
	
	}
	#endregion
	
	#region MEMBERS
	public void SetSpawnPosition(Vector3 position) {
		_spawnPosition = position;
	}
	
	private IEnumerator _Countdown() {
		GameObject cs = null;
		for (int i = 3; i >= 0; i--) {
			float t = 1f;
			while (t > 0f) {
				t -= Time.deltaTime;
				yield return null;
			}
			
			if (i != 0) {
				// Numbers
				cs = Instantiate(countdownScript, _spawnPosition, Quaternion.identity) as GameObject;
				cs.GetComponent<CountdownScript>().SetText(i.ToString());
			} else {
				// Go text
				cs = Instantiate(countdownScript, _spawnPosition, Quaternion.identity) as GameObject;
				cs.GetComponent<CountdownScript>().SetText("GO!");
			}
		}
		
		_countdownDone = true;
		yield return null;
	}
	#endregion
}
