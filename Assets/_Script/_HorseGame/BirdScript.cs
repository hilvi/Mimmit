using UnityEngine;
using System.Collections;

public class BirdScript : MonoBehaviour {

	#region MEMBERS
	public float speed;
	private Transform _transform;
	private Vector3 _translation;
	#endregion
	
	#region UNITY_METHODS
	
	void Start () 
	{
		_transform = GetComponent<Transform>();
		_translation = new Vector3(speed,0,0);
	}

	void Update () 
	{	
		_transform.Translate(_translation * Time.deltaTime);
		
	}
	void OnTriggerEnter(Collider col)
	{
		
	}
	#endregion
	
	#region METHODS
	public void SetSpeed(float speed)
	{
		_translation.x = speed;
	}
	#endregion
}
