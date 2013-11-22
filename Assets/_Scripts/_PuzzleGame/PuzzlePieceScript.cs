using UnityEngine;
using System.Collections;

public class PuzzlePieceScript : MonoBehaviour {
	Material _mat;
	public int x = -1;
	public int y = -1;

	// Use this for initialization
	void Awake () {
		Shader __diffuse = Shader.Find ("Diffuse");
		_mat = new Material (__diffuse);
		renderer.material = _mat;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
