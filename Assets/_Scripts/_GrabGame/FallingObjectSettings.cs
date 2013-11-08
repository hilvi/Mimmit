﻿using UnityEngine;

[System.Serializable]
public class FallingObjectSettings {
	public float minSpeed;
	public float maxSpeed;
	public Texture2D texture;
	public bool collect;
	
	internal int id = -1;
	internal bool collected = false;
}
