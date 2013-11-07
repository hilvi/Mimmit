using UnityEngine;
using System.Collections;

public class FallingObjectScript : MonoBehaviour {
	
	public float fallingSpeed;
	
	private Vector3 _pos;
	
	// Use this for initialization
	void Start () {
	
	}
	
	public void SetTexture(Texture2D texture) {
		renderer.material.mainTexture = texture;
	}
	
	// Update is called once per frame
	void Update () {
		_pos = transform.position;
		_pos.y -= fallingSpeed * Time.deltaTime;
		transform.position = _pos;
	}
	
	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.name == "Player") {
			Destroy(gameObject);
		}
	}
	
	void OnBecameInvisible() {
		Destroy (gameObject);
	}
}
