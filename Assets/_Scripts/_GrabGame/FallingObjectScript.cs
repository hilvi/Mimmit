﻿using UnityEngine;
using System.Collections;

public class FallingObjectScript : MonoBehaviour {
	
	public float fallingSpeed;
	public GrabGameManager manager;
	public int id;
	public bool collect;
	public bool falling = false;
	
	public float oscillation;
	
	private Vector3 _pos;
	private float startX;
	
	// Use this for initialization
	void Start () {
		startX = transform.position.x;
	}
	
	public void SetTexture(Texture2D texture) {
		renderer.material.mainTexture = texture;
	}
	
	// Update is called once per frame
	void Update () {
		if(falling) {
			_pos = transform.position;
			_pos.y -= fallingSpeed * Time.deltaTime;
			_pos.x = startX+Mathf.Sin(Time.time * 3)*oscillation;
			transform.position = _pos;
		}
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.name == "Player") {
			manager.ObjectCollected(id, collect);
			manager.RemoveObject(gameObject);
		}
	}
	
	void OnBecameInvisible() {
		manager.RemoveObject(gameObject);
	}
}
