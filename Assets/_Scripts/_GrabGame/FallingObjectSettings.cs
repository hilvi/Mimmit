using UnityEngine;

[System.Serializable]
public class FallingObjectSettings {
	public float minSpeed;
	public float maxSpeed;
	public Texture2D texture;
	public bool collect;
	public int numberToCollect;
	public float minOscillationAmplitude;
	public float maxOscillationAmplitude;
	
	internal int id = -1;
}
