using UnityEngine;
using System.Collections;

public class CarrotScript : MonoBehaviour {
	public HorseCharacterController _controller;
	
	void Start() {
		_controller = GameObject.Find ("Player").GetComponent<HorseCharacterController>();
	}
	
	IEnumerator OnTriggerEnter() {
		GetComponentInChildren<Renderer>().enabled = false;
		yield return StartCoroutine(_controller.PowerUp());
		Destroy (gameObject);
	}
}
