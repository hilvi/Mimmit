using UnityEngine;
using System.Collections;

//This script is for the movement done solely by the computer keys to allow looking and moving forward

public class MazeWalk : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.UpArrow))transform.Translate(Vector3.forward*Time.deltaTime);
		if(Input.GetKey(KeyCode.DownArrow))transform.Translate(-Vector3.forward*Time.deltaTime);
		if(Input.GetKey(KeyCode.RightArrow))transform.Rotate(0,1,0);
		if(Input.GetKey(KeyCode.LeftArrow))transform.Rotate(0,-1,0);
	}
}
