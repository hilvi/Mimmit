using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
public class PathScript : MonoBehaviour {
 
   public GameObject[]start;
   Dictionary<string,List<Transform>>path = new Dictionary<string, List<Transform>>();
 
   void Awake () {
      string str = null;
      for(int i = 0;i<start.Length;i++){
         int index=0;
         str = start[i].gameObject.tag;
         GameObject [] gos = GameObject.FindGameObjectsWithTag(str);
 
         List<Transform> waypoints = new List<Transform>();
         waypoints.Add (start[i].transform);
         start[i].gameObject.GetComponent<Waypoint>().SetUsed(true);
         while(true){
            Transform closest = FindClosest(waypoints[index],gos);
            if(closest != null)
               waypoints.Add (closest);
            if(++index >= gos.Length)break;
         }
         path.Add (str,waypoints);
     }
   }
 
   Transform FindClosest(Transform start, GameObject[] obj){
      int layerMask = 1 << 2;
		Debug.Log(layerMask);
      layerMask = ~layerMask;
      Transform closest = null;float distance = Mathf.Infinity;
      foreach(GameObject go in obj){
         Waypoint sc = go.GetComponent<Waypoint>();
			Debug.Log(sc);
			Debug.Log(go);
			if (sc)
         if(start!=go.transform && !sc.GetUsed()){
            if(!Physics.Linecast (start.position,go.transform.position,layerMask)){
               float dist = (start.position - go.transform.position).magnitude;
               if(dist < distance){
                  distance = dist;
                  closest = go.transform;
               }
            }
         }
      }
      if(closest != null){
         closest.gameObject.GetComponent<Waypoint>().SetUsed (true);
      }
      return closest;
   }
 
   public List<Transform> GetPath(string str){
      return path[str];
   }
}