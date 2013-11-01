using UnityEngine;
using System.Collections;

public class CountdownManager : MonoBehaviour {
	
	public GameObject countdownScript;
	
	public bool CountdownDone {
		get {
			return _countdownDone;
		}
	}
	private bool _countdownDone;
	
	void Start () {
		_countdownDone = false;
		StartCoroutine(_Countdown());
	}

	void Update () {
	
	}
	
	private IEnumerator _Countdown() {
		GameObject cs = Instantiate(countdownScript) as GameObject;
		cs.GetComponent<CountdownScript>().SetText("3");
		for (int i = 2; i >= 0; i--) {
			float t = 1f;
			while (t > 0f) {
				t -= Time.deltaTime;
				yield return null;
			}
			
			if (i != 0) {
				// Numbers
				cs = Instantiate(countdownScript) as GameObject;
				cs.GetComponent<CountdownScript>().SetText(i.ToString());
			} else {
				// Go text
				cs = Instantiate(countdownScript) as GameObject;
				cs.GetComponent<CountdownScript>().SetText("GO!");
			}
		}
		
		_countdownDone = true;
		yield return null;
	}
}
