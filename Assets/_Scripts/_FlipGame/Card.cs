using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour
{
	enum CardState
	{
		FaceDown,
		RotatingRight,
		RotatingLeft,
		FaceUp,
		Disappearing
	};
	
	#region MEMBERS
	public float randomPosition = 0.01f;
	public float randomRotation = 3;
	public bool randRotation = false;
	
	private CardState _state = CardState.FaceUp;
	private Quaternion _currentRotation;
	private Quaternion _nextRotation;
	private Quaternion _buf;
	//private AudioClip flipClick;
	//private AudioClip allRotateSound;
	//private AudioClip cardGoing;
	private string _suit = null;
	private float _sizeReduction;
	#endregion

	#region UNITY_METHODS
	void Start ()
	{
		if (randRotation) {
			transform.rotation = Quaternion.Euler (0, 0, Random.Range (-randomRotation, randomRotation));
		}
		
		transform.position += Vector3.up * Random.Range (-randomPosition, randomPosition);
		transform.position += Vector3.right * Random.Range (-randomPosition, randomPosition);
		_currentRotation = transform.rotation;
		transform.localScale = new Vector3 (1, 0.7f, 1);
		_nextRotation = Quaternion.Euler (0, 180, 0) * transform.rotation;
	}

	void Update ()
	{
		if (_state == CardState.Disappearing) { 
			// This makes the card grow smaller and destroys the object when the card is 
			// less than 1% of it's original size
			_sizeReduction += Time.deltaTime / 2;
			transform.localScale -= new Vector3 (_sizeReduction, _sizeReduction, _sizeReduction);
			if (transform.localScale.x < 0.01f) {
				Destroy (gameObject);
			}
		}
	}
	#endregion
	
	#region METHODS
	public IEnumerator Rotate ()
	{
		//Camera.main.audio.PlayOneShot(flipClick, 0.5f);
		float timer = 0;
		while (timer < 1) {
			timer += Time.deltaTime;
			transform.rotation = Quaternion.Lerp (transform.rotation, _nextRotation, timer);	
			yield return null;
		}
		if (_state == CardState.RotatingLeft)
			_state = CardState.FaceUp;
		if (_state == CardState.RotatingRight)
			_state = CardState.FaceDown;
		
		_buf = _currentRotation;
		_currentRotation = _nextRotation;
		_nextRotation = _buf;
		timer = 0;
		
		if (_state == CardState.FaceDown)
			_state = CardState.FaceUp;
		else
			_state = CardState.FaceDown;
	}
	
	public bool IsFaceUp ()
	{
		return _state == CardState.FaceUp;
	}
	
	public bool IsFaceDown ()
	{
		return _state == CardState.FaceDown;
	}
	
	public void SetSuit (string suit)
	{
		this._suit = suit;
	}
	
	public string GetSuit ()
	{
		return _suit;
	}
	
	public void Disappear ()
	{
		_state = CardState.Disappearing;
	}
	#endregion
}
