using UnityEngine;
using System.Collections.Generic;

public enum Direction
{
	Up,Down, Left, Right	
}
public class Movement : MonoBehaviour {

	Transform[] path;
	public Transform start;
	int index = 0;
	public float speed = 2.0f;
	bool walking;
	Animation2D anim;
	
	void Start () {
		walking = false;
		transform.position = start.position;
		NodeChoice nodeScript = start.GetComponent<NodeChoice>();
		nodeScript.SetGirlOn();
		anim = GetComponent<Animation2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(!walking)
		{
			anim.Stop();
			return;
		}
		
		Vector3 move = Vector3.zero;
		move.z += -1f;
		Vector3 target = path[index].position + move;
		float dist = Vector3.Distance(transform.position, target);
		if( dist > 0.6f)
		{
			anim.Play();
			
			transform.Translate((target - transform.position).normalized * Time.deltaTime*speed);
		}
		else if(++index == path.Length ){
			walking = false;
			INode nodeScript = path[index - 1].GetComponent<INode>();
			nodeScript.SetGirlOn();
		}
	}
	public void SetPath(Transform [] tr)
	{
		path = tr;
		index = 0;
		walking = true;
	}
}
