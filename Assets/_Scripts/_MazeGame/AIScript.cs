using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]

public class AIScript : MonoBehaviour {
	CharacterController _controller;
	Transform _transform;
	float speed = 5f;
	float gravity =20f;
	Vector3 moveDirection;

	// Use this for initialization
	void Start () {
		_controller = GetComponent<CharacterController>();
		_transform = GetComponent<Transform>();
		float speed =5f;
		float gravity = 20f;
		Vector3 moveDirection;
	
	}
	
	// Update is called once per frame
	void Update () {
		moveDirection = _transform.forward;
		moveDirection *= speed;
		moveDirection.y -= gravity;
		_controller.Move(moveDirection *Time.deltaTime);
	}
}
