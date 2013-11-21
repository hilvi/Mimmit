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
	public ParticleEmitter particleEmit;
	
	private HorseGameManager _gameManager;
	private CharacterController _controller;
	private Vector3 _movement;
	private float _currentSpeed;
	private bool _sideStepping = false;
	private Transform _plane;
	private string _runAnim;
	private string _jumpAnim;
	internal string _idleAnim;
	
	#endregion
	
	#region UNITY_METHODS
    void Awake()
    {
        switch (Manager.GetCharacter())
        {
            case Character.Blonde:
                _runAnim = "RunBlonde";
                _jumpAnim = "JumpBlonde";
                _idleAnim = "IdleBlonde";
                break;
            case Character.Brune:
                _runAnim = "RunBrune";
                _jumpAnim = "JumpBrune";
                _idleAnim = "IdleBrune";
                break;
            default:
                _runAnim = "RunBlonde";
                _jumpAnim = "JumpBlonde";
                _idleAnim = "IdleBlonde";
                break;
        }
    }

	void Start ()
	{
		_controller = GetComponent<CharacterController> ();
		particle = transform.Find ("Particle");
		particle.gameObject.SetActive (false);
		
		_plane = transform.Find ("Plane");
		_currentSpeed = runningSpeed;
		_movement.x = _currentSpeed;
		_gameManager = GameObject.Find("GameManager").GetComponent<HorseGameManager>();
		
		anim.PlayAnimation (_idleAnim);
	}
	
	void Update ()
	{			
		GameState __state = _gameManager.GetGameState();
		
		if (__state == GameState.Pregame || __state == GameState.Lost || __state == GameState.Won || __state == GameState.Tutorial) 
		{
			particleEmit.emit = false;
			anim.PlayAnimation (_idleAnim);
			return;
		}
			
		if (_controller.isGrounded) 
		{
			if (Input.GetButtonDown ("Fire1"))
			SideStep ();
			if (Input.GetButtonUp ("Fire1"))
				SideStepReturn ();
			
			if (_controller.velocity.x != 0)
			{
				anim.PlayAnimation (_runAnim);
				if(particleEmit.emit == false)
					particleEmit.emit = true;
			}
			else
			{
				anim.PlayAnimation (_idleAnim);
				if(particleEmit.emit == true)
					particleEmit.emit = false;
			}
			if (Input.GetButtonDown ("Jump") && !_sideStepping) 
			{	
				_movement.y = jumpSpeed;
			}
			
		} 
		else 
		{
			anim.PlayAnimation (_jumpAnim);
			if(particleEmit.emit == true)
					particleEmit.emit = false;
			_movement.y -= gravity * Time.deltaTime;
		}
		
		_movement.x = _currentSpeed;
		_controller.Move (_movement * Time.deltaTime);
	}
	#endregion
	
	#region METHODS
	/// <summary>
	/// Enters the mud configuration.
	/// When stepping in mud, this is called and set the horse as mud configuration.
	/// Speed is slowed down and the horse is set halfway into the ground
	/// </summary>
	public void EnterMudConfiguration ()
	{
		if (_currentSpeed > runningSpeed)
			return;
			//_currentSpeed = (mudSpeed + powerUpSpeed) / 2;
		else
			_currentSpeed = mudSpeed;

		Animator2D __anim = GetComponentInChildren<Animator2D>();
		__anim.speed /= 2;
	}
	
	/// <summary>
	/// Exits the mud configuration.
	/// When stepping out of the mud, the horse gets initla speed and position
	/// </summary>
	public void ExitMudConfiguration ()
	{
		if (_currentSpeed > runningSpeed)
			return;
		else
			_currentSpeed = runningSpeed;
		
		Animator2D __anim = GetComponentInChildren<Animator2D>();
		__anim.speed *= 2;
	}
	
	public void SetSpeed(float speed)
	{
		_currentSpeed = speed;
	}

	public float GetSpeed ()
	{
		return _currentSpeed;
	}

	public void SideStep ()
	{
		if (!_sideStepping)
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
