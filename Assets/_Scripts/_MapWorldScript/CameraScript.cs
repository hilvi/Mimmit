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
		vec.x = Mathf.Clamp(girl.position.x , -38.0f, 38.0f);
		vec.z = Mathf.Clamp(girl.position.z , -18.0f, 17.0f);
		transform.position = vec;
	}
}
