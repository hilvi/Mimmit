using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class HorseCharacterController : MonoBehaviour {
	
	public float gravity = 20;
	public float speed = 5;
	public float jumpSpeed = 10;
	
	private CharacterController _controller;
	private Vector3 _movement;
	
	void Start () {
		_controller = GetComponent<CharacterController>();
		_movement.x = speed;
	}
	
	// Update is called once per frame
	void Update () {
		if(_controller.isGrounded) 
		{
			if(Input.GetButtonDown("Jump")) {
				_movement.y = jumpSpeed;
			}
		}else 
		_movement.y -= gravity * Time.deltaTime;
		_controller.Move(_movement*Time.deltaTime);
	}
}
