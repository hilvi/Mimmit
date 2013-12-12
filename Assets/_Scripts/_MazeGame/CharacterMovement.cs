using UnityEngine;
using System.Collections;


[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour 
{
	private enum AnimState{
		WalkLeft, WalkRight
	}
	#region MEMBERS
	private Vector3 _moveDirection = Vector3.zero;
	private CharacterController _controller;
	private Animator2D _anim;
	private AnimState _animState = AnimState.WalkRight;
	private MazeGameManager _manager;

	public float speed = 6.0F;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
	#endregion

	#region UNITY_METHODS
	void Start () 
	{
		_anim = GetComponentInChildren<Animator2D>();
		_controller = GetComponent<CharacterController>();
		_manager = GameObject.Find("GameManager").GetComponent<MazeGameManager>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(_manager.GetGameState() != GameState.Running)return;

		if (_controller.isGrounded) 
		{
			float __hor = Input.GetAxis("Horizontal");
			float __ver = Input.GetAxis("Vertical");
			_moveDirection = new Vector3(__hor, 0, __ver);
			_moveDirection = transform.TransformDirection(_moveDirection);
			_moveDirection *= speed;	
			_PlayAnim(__hor, __ver);
		}
		_moveDirection.y -= gravity * Time.deltaTime;
		_controller.Move(_moveDirection * Time.deltaTime);
	}
	#endregion

	#region METHODS

	void _PlayAnim(float hor, float ver)
	{
		if( hor > 0)
		{
			_animState = AnimState.WalkRight;
			_anim.PlayAnimation("WalkRight");
			return;
		}
		else if( hor < 0)
		{
			_animState = AnimState.WalkLeft;
			_anim.PlayAnimation("WalkLeft");
			return;
		}
		if(ver != 0)
		{
			_anim.PlayAnimation(_animState.ToString());
		}
	}
	#endregion
}
