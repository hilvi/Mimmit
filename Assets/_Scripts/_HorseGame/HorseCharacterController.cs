using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class HorseCharacterController : MonoBehaviour {
	
	public float gravity = 20;
	public float runningSpeed = 5;
	public float jumpSpeed = 10;
	public float mudSpeed = 2;
	public Animator2D anim;
	
	private CharacterController _controller;
	private Vector3 _movement;
	private float _currentSpeed;
	
	void Start () 
	{
		_controller = GetComponent<CharacterController>();
		_currentSpeed = runningSpeed;
		_movement.x = _currentSpeed;
	}
	
	void Update () 
	{
		if(_controller.isGrounded) 
		{
			Debug.Log(_controller.velocity.x);
			if(_controller.velocity.x != 0)
				anim.PlayAnimation("Run");
			else
				anim.StopAnimation();
			if(Input.GetButtonDown("Jump")) 
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
	}
	
	/// <summary>
	/// Exits the mud configuration.
	/// When stepping out of the mud, the horse gets initla speed and position
	/// </summary>
	public void ExitMudConfiguration()
	{
		_currentSpeed = runningSpeed;
	}
	public void SetSpeed(float speed)
	{
		_currentSpeed = speed;
	}
}
