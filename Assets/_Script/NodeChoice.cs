using UnityEngine;
using System.Collections;
public enum GamePosition{
	None, Left, Right, Up ,Down
}
public class NodeChoice : INode {
	
	public Transform[] left;
	public Transform[] right;
	public Transform[] up;
	public Transform[] down;
	public GamePosition gamePosition;
	bool[] _bool = new bool[4];
	
	
  	
	Movement girlMove;
	ArrowScript arrowScript;
	bool isGirlOn;
	
	// Use this for initialization
	void Awake () 
	{
		isGirlOn = false;
		GameObject g = GameObject.Find("Girl");
		if(g != null)
			girlMove = g.GetComponent<Movement>();
		arrowScript = Camera.main.GetComponent<ArrowScript>();
		for(int i = 0; i < _bool.Length ;i++)_bool[i] = false;
		if(up.Length != 0 )_bool[0] = true; 
		if(down.Length != 0 )_bool[1] = true; 
		if(left.Length != 0 )_bool[2] = true; 
		if(right.Length != 0 )_bool[3] = true;
	}
	void Start()
	{
		if(arrowScript == null) Camera.main.GetComponent<ArrowScript>();
		if(girlMove == null)girlMove = GameObject.Find("Girl").GetComponent<Movement>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!isGirlOn)return;
		
		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			if(up.Length == 0 )return;
			AssignPath(Direction.Up);
			arrowScript.RemoveArrow();
		}
		else if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			if(down.Length == 0 )return;
			AssignPath(Direction.Down);
			arrowScript.RemoveArrow();
		}
		else if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			if(left.Length == 0)return;
			AssignPath(Direction.Left);
			arrowScript.RemoveArrow();
		}
		else if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			if(right.Length == 0)return;
			AssignPath(Direction.Right);
			arrowScript.RemoveArrow();
		}
	
	}
	public void AssignPath(Direction direction)
	{
		switch(direction)
		{
		case Direction.Up:
			girlMove.SetPath(up);
			break;
		case Direction.Down:
			girlMove.SetPath(down);
			break;
		case Direction.Left:
			girlMove.SetPath(left);
			break;
		case Direction.Right:
			girlMove.SetPath(right);
			break;
		}
		isGirlOn = false;
	}
	public override void SetGirlOn()
	{
		isGirlOn = true;
		if(arrowScript == null)Debug.Log ("Nope");
		arrowScript.SetArrow(_bool, this,gamePosition);
	}
}
