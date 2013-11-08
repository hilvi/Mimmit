using UnityEngine;
using System.Collections;

public class FallingObjectScript : MonoBehaviour {
	
	public float fallingSpeed;
	public GrabGameManager manager;
	public int id;
	public bool collect;
	
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
		_pos = transform.position;
		_pos.y -= fallingSpeed * Time.deltaTime;
		_pos.x = startX+Mathf.Sin(Time.time)*oscillation;
		transform.position = _pos;
	}
	
	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.name == "Player") {
			manager.ObjectCollected(id, collect);
			Destroy(gameObject);
		}
	}
	
	void OnBecameInvisible() {
		Destroy (gameObject);
	}
}
