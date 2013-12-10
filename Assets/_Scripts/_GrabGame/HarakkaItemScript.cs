using UnityEngine;
using System.Collections;

public class HarakkaItemScript : MonoBehaviour {
	public GameObject fallingObject;
	public bool moving = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(moving)
			transform.position += new Vector3(0, 200*Time.deltaTime, 0);
	}

	void OnBecameInvisible() {
		fallingObject.GetComponent<FallingObjectScript>().falling = true;
		Destroy(gameObject);
	}
}
