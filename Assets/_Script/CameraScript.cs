using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	Transform girl;
	public MapWorldScript map;
	void Start () {
		GameObject g = map.GetGirl();
		girl = g.transform;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 vec = transform.position;
		vec.x = girl.position.x;
		vec.z = girl.position.z;
		transform.position = vec;
	}
}
