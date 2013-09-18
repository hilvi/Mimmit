using UnityEngine;
using System.Collections.Generic;

public enum Direction
{
	Up,Down, Left, Right	
}
public class Movement : MonoBehaviour {

	Transform[] path;
	//public Transform start;
	int index = 0;
	public float speed = 2.0f;
	bool walking;
	//Animation2D anim;
	public Animator2D animator;
	MapWorldScript map;
	void Start () 
	{
		walking = false;
		map = GameObject.Find ("MapWorld").GetComponent<MapWorldScript>();
		NodeChoice nodeScript = map.startPosition.GetComponent<NodeChoice>();
		nodeScript.SetGirlOn();
		animator = GetComponent<Animator2D>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		NavigationState currentState = Manager.GetNavigationState();
		if(currentState == NavigationState.Pause)
		{
			animator.PauseAnimation();
			return;
		}
		if(!walking)
		{
			animator.PlayAnimation("idle");
			return;
		}
		
		Vector3 move = Vector3.zero;
		move.z += -1f;
		Vector3 target = path[index].position + move;
		float dist = Vector3.Distance(transform.position, target);
		Vector3 dir = (target - transform.position).normalized;
		if( dist > 0.6f)
		{
			Vector3 cross = Vector3.Cross(dir, Vector3.forward);
			if(cross.y < 0) 
				animator.PlayAnimation("walkLeft");
			else
				animator.PlayAnimation("walkRight");
			
			transform.Translate( dir * Time.deltaTime*speed);
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
