using UnityEngine;
using System.Collections;

public class CarrotScript : MonoBehaviour
{
	#region MEMBERS
	public HorseCharacterController _controller;
	private Renderer _renderer;
	private Transform _particle;
	private Transform _transform;
	#endregion
	
	#region UNITY_METHODS
	void Start ()
	{
		_controller = GameObject.FindGameObjectWithTag ("Player").GetComponent<HorseCharacterController> ();
		_renderer = GetComponentInChildren<Renderer> ();
		_particle = transform.Find ("Particle");
		_transform = GetComponent<Transform>();
	}

	void Update()
	{
		float __min = 1.0f;
		float __amplitude = 0.5f;
		float __frequency = 2.0f;
		Vector3 __scale = _transform.localScale;
		float __value = __amplitude * Mathf.Cos (Time.time * __frequency);
		__scale.x = __scale.y = Mathf.Abs (__value) + __min;
		_transform.localScale = __scale;
	}
	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.tag == "Player") {
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
