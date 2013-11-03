using UnityEngine;
using System.Collections;

public class BirdScript : MonoBehaviour
{
	#region MEMBERS
	public float speed;
	private Animator2D _anim;
	private Transform _transform;
	private Vector3 _translation;
	#endregion
	
	#region UNITY_METHODS
	void Start ()
	{
		_transform = GetComponent<Transform> ();
		_translation = new Vector3 (speed, 0, 0);
		_anim = GetComponentInChildren<Animator2D> ();
	}

	void Update ()
	{	
		_transform.Translate (_translation * Time.deltaTime);
		_anim.PlayAnimation ("Bird");
	}
	#endregion
	
	#region METHODS
	public void SetSpeed (float speed)
	{
		_translation.x = speed;
	}
	#endregion
}
