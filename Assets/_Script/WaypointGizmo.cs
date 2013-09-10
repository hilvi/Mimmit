using UnityEngine;
using System.Collections;

public class SeeWayPoint : MonoBehaviour {

	 public float radius = 0.5f;
   void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
