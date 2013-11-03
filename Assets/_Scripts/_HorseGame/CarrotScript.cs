using UnityEngine;
using System.Collections;

public class CarrotScript : MonoBehaviour
{
	#region MEMBERS
	public HorseCharacterController _controller;
	private Renderer _renderer;
	private Transform _particle;
	#endregion
	
	#region UNITY_METHODS
	void Start ()
	{
		_controller = GameObject.Find ("Player").GetComponent<HorseCharacterController> ();
		_renderer = GetComponentInChildren<Renderer> ();
		_particle = transform.Find ("Particle");
	}
	
	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.name == "Player") {
			StartCoroutine (DoubleSpeed (2f));
			_renderer.enabled = false;
			_particle.gameObject.SetActive (false);
			_controller.particle.gameObject.SetActive (true);
		}
	}
	#endregion
	
	#region METHODS
	public IEnumerator DoubleSpeed (float addSpeed)
	{
		_controller.SetSpeed (_controller.GetSpeed () + 2f);
		while (_controller.GetSpeed() > _controller.runningSpeed) {
			_controller.SetSpeed (_controller.GetSpeed () - Time.deltaTime);
			yield return null;
		}
		_controller.SetSpeed (_controller.runningSpeed);
		_controller.particle.gameObject.SetActive (false);
	}
	#endregion
}
