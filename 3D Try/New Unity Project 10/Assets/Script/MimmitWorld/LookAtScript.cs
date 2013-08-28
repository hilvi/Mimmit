using UnityEngine;
using System.Collections;

public class MovementScript : MonoBehaviour {
	Vector3 targetPosition;
	GameObject player;
	
 
	void Update(){
		
			player = GameObject.Find ("Player");
			targetPosition = new Vector3(player.transform.position.x,player.transform.position.y,player.transform.position.z);
			transform.LookAt(targetPosition);
		
	}
	
}
