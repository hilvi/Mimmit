using UnityEngine;
using System.Collections;
 
public class Waypoint: MonoBehaviour {
 
	//public int index;
   bool used;
   public bool GetUsed(){
      return used;
   }
   public void SetUsed(bool b){
      used = b;
   }
   public float radius = 0.5f;
   void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, radius);
    }
}