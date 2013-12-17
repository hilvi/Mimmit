using UnityEngine;
using System.Collections;

public class HarakkaItemScript : MonoBehaviour {
	public GameObject fallingObject;
	public bool moving = false;
	public float velocity = 200;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(moving) {
			transform.position += new Vector3(0, velocity*Time.deltaTime, 0);
			velocity -= 9.81f;
		}
	}

	void OnBecameInvisible() {
		if(fallingObject != null)
			fallingObject.GetComponent<FallingObjectScript>().falling = true;
		Destroy(gameObject);
	}
}
