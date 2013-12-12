using UnityEngine;
using System.Collections;

public class BirdScript : MonoBehaviour
{
	#region MEMBERS
	public float speed;
	private HorseGameManager _gameManager;
	private Animator2D _anim;
	private Transform _transform;
	private Vector3 _translation;
	#endregion
	
	#region UNITY_METHODS
	void Awake () {
		_transform = GetComponent<Transform> ();
		_translation = new Vector3 (speed, 0, 0);
		_anim = GetComponentInChildren<Animator2D> ();
		_gameManager = GameObject.Find("GameManager").GetComponent<HorseGameManager>();
	}

	void Update ()
	{	
		// Allow bird to flap its wings even when stationary,
		// because birds can't hover.
		GameState __currentState = _gameManager.GetGameState();
		if(__currentState == GameState.Tutorial)
			return;
		_anim.PlayAnimation ("Bird");
		if (__currentState != GameState.Running)
			return;
		_transform.Translate (_translation * Time.deltaTime);
	}
	#endregion
	
	#region METHODS
	public void SetSpeed (float speed)
	{
		_translation.x = speed;
	}
	#endregion
}
