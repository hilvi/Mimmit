using UnityEngine;
using System.Collections;

public class HarakkaScript : MonoBehaviour {
	private Animator2D _anim;
	public GameObject item;
	public float speed = 10;
	public bool moving = false;
	// Use this for initialization
	void Start () {
		_anim = GetComponent<Animator2D>();
		_anim.PlayAnimation("idle");
	}
	
	// Update is called once per frame
	void Update () {

	}

	public IEnumerator MoveToPosAndThrow(float position, GameObject fallingObject) {
		moving = true;
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
		StartCoroutine(Throw(fallingObject));
	}

	public IEnumerator Throw(GameObject fallingObject) {
		_anim.PlayAnimation("throw");

		GameObject obj = Instantiate(item) as GameObject;
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
}
