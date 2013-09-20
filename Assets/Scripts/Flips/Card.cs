using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour {
	
	public float randomPosition = 0.01f;
	public float randomRotation = 3;
	public bool randRotation = false;
	enum CardState {FaceDown, RotatingRight, RotatingLeft, FaceUp, Disappearing};
	CardState state = CardState.FaceUp;
	
	Quaternion currentRotation;
	Quaternion nextRotation;
	Quaternion buf;
	
	/*AudioClip flipClick;
	AudioClip allRotateSound;
	AudioClip cardGoing;*/

	
	string suit = null;
	float sizeReduction;

	// Use this for initialization
	void Start () 
	{
		if(randRotation)
		{
			transform.rotation = Quaternion.Euler(0,0,Random.Range(-randomRotation, randomRotation));
		}
		transform.position += Vector3.up*Random.Range(-randomPosition, randomPosition);
		transform.position += Vector3.right*Random.Range(-randomPosition, randomPosition);
		currentRotation = transform.rotation;
		transform.localScale = new Vector3(1,0.7f,1);
		nextRotation = Quaternion.Euler (0, 180, 0)*transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (state == CardState.Disappearing) { // This makes the card grow smaller and destroys the object when the card is less than 1% of it's original size
			sizeReduction += Time.deltaTime / 2;
			transform.localScale -= new Vector3(sizeReduction, sizeReduction, sizeReduction);
			if (transform.localScale.x < 0.01f) {
				Destroy(gameObject);
			}
			
		}
	}
	
	public IEnumerator Rotate() 
	{
		//Camera.main.audio.PlayOneShot(flipClick, 0.5f);
		float timer = 0;
		while(timer < 1)
		{
			timer += Time.deltaTime;
			transform.rotation = Quaternion.Lerp (transform.rotation, nextRotation, timer);	
			yield return null;
		}
		if (state == CardState.RotatingLeft) state = CardState.FaceUp;
		if (state == CardState.RotatingRight) state = CardState.FaceDown;
		buf = currentRotation;
		currentRotation = nextRotation;
		nextRotation = buf;
		timer = 0;
		if(state == CardState.FaceDown)
			state = CardState.FaceUp;
		else
			state = CardState.FaceDown;
	}
	
	public bool IsFaceUp() {
		return state == CardState.FaceUp;
	}
	
	public bool IsFaceDown() {
		return state == CardState.FaceDown;
	}	
	
	public void SetSuit(string suit) {
		this.suit = suit;
	}
	
	public string GetSuit() {
		return suit;
	}
	
	public void Disappear() {
		state = CardState.Disappearing;
	}
}
