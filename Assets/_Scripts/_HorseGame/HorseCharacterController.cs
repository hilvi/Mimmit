﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class HorseCharacterController : MonoBehaviour {
	
	public float gravity = 20;
	public float runningSpeed = 5;
	public float jumpSpeed = 10;
	public float mudSpeed = 2;
	public Vector3 sideStep = new Vector3(0, -0.3f, -0.5f);
	public Animator2D anim;
	
	private CharacterController _controller;
	private Vector3 _movement;
	private float _currentSpeed;
	private bool _sideStepping = false;
	Transform _plane;
	
	void Start () 
	{
		_controller = GetComponent<CharacterController>();
		_plane = transform.Find("Plane");
		_currentSpeed = runningSpeed;
		_movement.x = _currentSpeed;
	}
	
	void Update () 
	{
		if(Input.GetButtonDown("Fire1"))
			SideStep();
		if(Input.GetButtonUp ("Fire1"))
			SideStepReturn();
		
		if(_controller.isGrounded) 
		{ 
			if(_controller.velocity.x != 0)
				anim.PlayAnimation("Run");
			else
				anim.PlayAnimation("Idle");
			if(Input.GetButtonDown("Jump") && !_sideStepping) 
			{	
				_movement.y = jumpSpeed;
			}
			
		}else
		{
			anim.PlayAnimation("Jump");
			_movement.y -= gravity * Time.deltaTime;
		}
		
		_movement.x = _currentSpeed;
		_controller.Move(_movement*Time.deltaTime);
	}
	
	/// <summary>
	/// Enters the mud configuration.
	/// When stepping in mud, this is called and set the horse as mud configuration.
	/// Speed is slowed down and the horse is set halfway into the ground
	/// </summary>
	public void EnterMudConfiguration()
	{
		_currentSpeed = mudSpeed;
		Animation2D __anim =  anim.GetCurrentAnimation();
		__anim.frameRate /= 2;
	}
	
	/// <summary>
	/// Exits the mud configuration.
	/// When stepping out of the mud, the horse gets initla speed and position
	/// </summary>
	public void ExitMudConfiguration()
	{
		_currentSpeed = runningSpeed;
		Animation2D __anim =  anim.GetCurrentAnimation();
		__anim.frameRate *= 2;
	}
	public void SetSpeed(float speed)
	{
		_currentSpeed = speed;
	}
	public void SideStep() {
		if(!_sideStepping) // && _controller.isGrounded
			_SideStep(1);
	}
	public void SideStepReturn() {
		if(_sideStepping) {
			_SideStep(-1);
		}
	}
	private void _SideStep(int dir) {
			Vector3 __tmp = _plane.position;
			__tmp.y += dir*sideStep.y;
			_plane.position = __tmp;
	
			__tmp = transform.position;
			__tmp.z += dir*sideStep.z;
			transform.position = __tmp;
			_sideStepping = !_sideStepping;
	}
}
