using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class HarakkaScript : MonoBehaviour {
	private Animator2D _anim;
	public GameObject item;
	public float speed = 10;
	public bool moving = false;

	private GameObject _currentObject;

	// Use this for initialization
	void Start () {
		_anim = GetComponent<Animator2D>();
		_anim.PlayAnimation("idle");
	}
	
	// Update is called once per frame
	void Update () {

	}

	public IEnumerator MoveToPosAndThrow(GameObject fallingObject) {
		moving = true;
		float position = fallingObject.transform.position.x;
		if (transform.position.x - position > 0)
			_anim.PlayAnimation ("left");
		else
			_anim.PlayAnimation ("right");

		Vector2 targetPos = transform.position;
		targetPos.x = position;

		float time = 0;

		while(Vector2.Distance(targetPos, transform.position) > 0.05f) {
			transform.position = Vector2.MoveTowards(transform.position, targetPos, 5*time);
			time += Time.deltaTime;
			yield return null;
		}
		transform.position = targetPos;


		_anim.PlayAnimation("throw");
		
		GameObject obj = _currentObject = Instantiate(item) as GameObject;
		obj.transform.position = transform.position;
		obj.transform.position += new Vector3(3, 0, -1);
		obj.renderer.material.mainTexture = fallingObject.renderer.material.mainTexture;
		
		HarakkaItemScript script = obj.GetComponent<HarakkaItemScript>();
		script.fallingObject = fallingObject;
		
		while(_anim.IsPlaying()) {
			yield return null;
		}
		
		_anim.PlayAnimation("idle");
		
		script.moving = true;
		moving = false;
	}

	public void End() {
		StopCoroutine("MoveToPosAndThrow");
		moving = false;
		Destroy(_currentObject);
		_anim.PlayAnimation("idle");
	}
}
