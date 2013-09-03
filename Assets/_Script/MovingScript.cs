using UnityEngine;
using System.Collections;

public class MovingScript : MonoBehaviour {
	Vector3 targetPosition;
	GameObject player;
	
 
	void Update(){
		
			player = GameObject.Find ("Player");
			targetPosition = new Vector3(player.transform.position.x,this.transform.position.y,player.transform.position.z);
			transform.LookAt(targetPosition);
		
	}
	
}
