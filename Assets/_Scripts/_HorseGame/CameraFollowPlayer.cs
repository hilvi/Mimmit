using UnityEngine;
using System.Collections;

/// <summary>
/// Camera follow player.
/// Script is attached to the camera.
/// Purpose is to follow the x position of the player only
/// </summary>
public class CameraFollowPlayer : MonoBehaviour
{
	#region MEMBERS
	private Transform playerTr;
	private Transform _transform;
	#endregion
	
	#region UNITY_METHODS
	void Start ()
	{
		_transform = GetComponent<Transform> ();
		playerTr = GameObject.Find ("Player").transform;
	}

	void Update ()
	{
		Vector3 __vec = _transform.position;
		__vec.x = playerTr.position.x;
		_transform.position = __vec;
	}
	#endregion
}
