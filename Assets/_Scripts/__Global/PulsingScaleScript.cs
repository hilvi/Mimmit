using UnityEngine;
using System.Collections;

public class PulsingScaleScript : MonoBehaviour {

	public float pulsingRate;
	public float pulsingSize;

	private Vector3 _defaultScale;

	void Awake() {
		_defaultScale = transform.localScale;
	}

	void Update () {
		// y = 0.5 + 0.5 * cos(t) oscillates between 0 and 1
		float __multiplier = 0.5f + 0.5f * Mathf.Cos (Time.time * pulsingRate);

		Vector3 __newScale = new Vector3();
		__newScale.x = _defaultScale.x + pulsingSize * __multiplier * _defaultScale.x;
		__newScale.y = _defaultScale.y + pulsingSize * __multiplier * _defaultScale.y;

		transform.localScale = __newScale;
	}
}
