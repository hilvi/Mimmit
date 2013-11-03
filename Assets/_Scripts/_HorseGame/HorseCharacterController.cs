using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class HorseCharacterController : MonoBehaviour
{
	#region MEMBERS
	public float gravity = 20;
	public float runningSpeed = 5;
	public float jumpSpeed = 10;
	public float mudSpeed = 2;
	public float powerUpSpeed = 7;
	public float powerUpTime = 2;
	public Vector3 sideStep = new Vector3 (0, -0.3f, -0.5f);
	public Animator2D anim;
	public Transform particle;
	
	private HorseGameManager _gameManager;
	private CharacterController _controller;
	private Vector3 _movement;
	private float _currentSpeed;
	private bool _sideStepping = false;
	private Transform _plane;
	#endregion
	
	#region UNITY_METHODS
	void Start ()
	{
		_controller = GetComponent<CharacterController> ();
		particle = transform.Find ("Particle");
		particle.gameObject.SetActive (false);
		
		_plane = transform.Find ("Plane");
		_currentSpeed = runningSpeed;
		_movement.x = _currentSpeed;
		_gameManager = GameObject.Find("GameManager").GetComponent<HorseGameManager>();
		anim.PlayAnimation ("Idle");
	}
	
	void Update ()
	{		
		if (_gameManager.GetGameState() != GameState.Running &&
			_gameManager.GetGameState() != GameState.Paused) {
			// Always idle if game is not running or paused
			anim.PlayAnimation ("Idle");
			return;
		}
		
		if (_gameManager.GetGameState() != GameState.Paused) {			
			if (Input.GetButtonDown ("Fire1"))
				SideStep ();
			if (Input.GetButtonUp ("Fire1"))
				SideStepReturn ();
		}
		
		if (_controller.isGrounded) { 
			if (_controller.velocity.x != 0)
				anim.PlayAnimation ("Run");
			else
				anim.PlayAnimation ("Idle");
			if (Input.GetButtonDown ("Jump") && !_sideStepping) {	
				_movement.y = jumpSpeed;
			}
			
		} else {
			anim.PlayAnimation ("Jump");
			_movement.y -= gravity * Time.deltaTime;
		}
		
		_movement.x = _currentSpeed;
		_controller.Move (_movement * Time.deltaTime);
	}
	#endregion
	
	#region METHODS
<<<<<<< HEAD

	
=======
>>>>>>> 4dfc115e09bd115d765c6f697dc1814b34cd5b2d
	/// <summary>
	/// Enters the mud configuration.
	/// When stepping in mud, this is called and set the horse as mud configuration.
	/// Speed is slowed down and the horse is set halfway into the ground
	/// </summary>
	public void EnterMudConfiguration ()
	{
		if (_currentSpeed == powerUpSpeed)
			_currentSpeed = (mudSpeed + powerUpSpeed) / 2;
		else
			_currentSpeed = mudSpeed;

		Animation2D __anim = anim.GetCurrentAnimation ();
		__anim.frameRate /= 2;
		print ("In");
	}
	
	/// <summary>
	/// Exits the mud configuration.
	/// When stepping out of the mud, the horse gets initla speed and position
	/// </summary>
	public void ExitMudConfiguration ()
	{
		if (_currentSpeed == mudSpeed)
			_currentSpeed = runningSpeed;
		else
			_currentSpeed = powerUpSpeed;
		
		Animation2D __anim = anim.GetCurrentAnimation ();
		__anim.frameRate *= 2;
		print ("Out");
		print(__anim.frameRate);
	}
<<<<<<< HEAD
	
	public void SetSpeed(float speed)
=======

	public void SetSpeed (float speed)
>>>>>>> 4dfc115e09bd115d765c6f697dc1814b34cd5b2d
	{
		_currentSpeed = speed;
	}

	public float GetSpeed ()
	{
		return _currentSpeed;
	}

	public void SideStep ()
	{
		if (!_sideStepping) // && _controller.isGrounded
			_SideStep (1);
	}

	public void SideStepReturn ()
	{
		if (_sideStepping) {
			_SideStep (-1);
		}
	}

	private void _SideStep (int dir)
	{
		Vector3 __tmp = _plane.position;
		__tmp.y += dir * sideStep.y;
		_plane.position = __tmp;
	
		__tmp = transform.position;
		__tmp.z += dir * sideStep.z;
		transform.position = __tmp;
		_sideStepping = !_sideStepping;
	}
	#endregion
}
