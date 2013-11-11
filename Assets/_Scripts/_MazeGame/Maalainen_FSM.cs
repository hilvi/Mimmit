using UnityEngine;
using System.Collections;

public class Maalainen_FSM : WayPointScript{
	public enum maa_States {
		Wander,
		Guide,
		Idle,
		Return,
	}
	public Maalainen_FSM curState;
	public float curSpeed;
	public float curRotSpeed;
	bool hungry;
	// Use this for initialization
	void Start () {
	curState=Wander;
	hungry=true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
