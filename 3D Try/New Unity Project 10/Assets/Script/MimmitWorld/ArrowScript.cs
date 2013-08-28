using UnityEngine;
using System.Collections;

public class ArrowScript : MonoBehaviour {
	
	Rect leftButton, rightButton, upButton, downButton;
	public Texture2D up,down,left,right;
	bool upOn, downOn, leftOn ,rightOn;
	bool isGirlOn;
	NodeChoice node = null;
	
	void Start () {
		float midW = Screen.width / 2;
		float midH = Screen.height / 2;
		float buttonW = Screen.width / 6;
		float buttonH = Screen.height / 8;
		float margin = buttonW / 2;
		upButton = new Rect(midW - 0.5f * buttonH , midH - buttonW-margin, buttonH, buttonW);
		downButton = new Rect(midW - 0.5f * buttonH , midH + margin, buttonH, buttonW);
		leftButton = new Rect(midW - buttonW - margin, midH - 0.5f * buttonH,buttonW, buttonH);
		rightButton = new Rect(midW + margin, midH - 0.5f * buttonH,buttonW, buttonH);
	}
	
	void OnGUI()
	{
		if(!isGirlOn) return;
		
		if(upOn){
			if(GUI.Button(upButton,up))
			{
				node.AssignPath(Direction.Up);
				RemoveArrow ();
			}
		}
		if(downOn){
			if(GUI.Button(downButton,down))
			{
				node.AssignPath(Direction.Down);
				RemoveArrow();
			}
		}
		if(leftOn){
			if(GUI.Button(leftButton,left))
			{
				node.AssignPath(Direction.Left);
				RemoveArrow();
			}
		}
		if(rightOn){
			if(GUI.Button(rightButton,right))
			{
				node.AssignPath(Direction.Right);
				RemoveArrow();
			}
		}
	}
	public void SetArrow(bool[] array, NodeChoice node)
	{
		this.upOn = array[0];
		this.downOn = array[1];
		this.leftOn = array[2];
		this.rightOn = array[3];
		isGirlOn = true;
		this.node = node;
	}
	public void RemoveArrow()
	{
		isGirlOn = false;
		node = null;
	}
}
