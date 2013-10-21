using UnityEngine;
using System.Collections;

public class WaypointGizmo : MonoBehaviour {

	 public float radius = 0.5f;
   		void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
