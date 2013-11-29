using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SoundManager))]
[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(LevelGenerator))]
public class FlipsGameManager : GameManager
{
	#region MEMBERS
	public float revealTime = 3; // The time, in seconds, for which the cards are revealed at the beginning of the level
	public InputManager inputManager;
	public LevelGenerator levelGenerator;
	public SoundManager sound;
	public GameObject musicObject;
	public AudioClip music;
	
	private Card _firstCard = null; // Handles to the two cards the player is currently flipping
	private Card _secondCard = null;
	private int _cardsTotal;
	private int _cardsGuessed = 0;
	private Camera _cam;
	private static GameObject _obj;

	private string _card = "Card";
	private string _memorySwift = "MemorySwift";
	private string _memoryFound = "MemoryFound";
	private string _memoryReturn = "MemoryReturn";
	#endregion
	
	#region UNITY_METHODS
	public override void Start ()
	{
		base.Start ();
		if (InGameMenuGUI.music == null) 
		{
			InGameMenuGUI.music = (GameObject)Instantiate (musicObject);
			InGameMenuGUI.music.audio.clip = music;
			InGameMenuGUI.music.audio.Play ();
		}
		
		_cardsTotal = levelGenerator.CardCount ();
		SetGameState (GameState.Pregame);
		_cam = Camera.main;
		
		StartCoroutine (_InitiateCountdown ());
	}
	
	void Update ()
	{
		GameState currentState = GetGameState ();
		switch (currentState) {
			#region PREGAME
		case GameState.Pregame:	
			if (inputManager.IsEscapeButtonDown ()) 
				PauseGame ();
			break;
			#endregion
			#region RUNNING
		case GameState.Running:
			if (inputManager.IsEscapeButtonDown ()) 
				PauseGame ();
			Ray ray = _cam.ScreenPointToRay (inputManager.GetCursorPosition ());
			RaycastHit hit;
			
			if (inputManager.IsButtonDown () && Physics.Raycast (ray, out hit) && !_secondCard) 
			{
				if (hit.collider.CompareTag (_card)) 
				{
					Card card = hit.collider.gameObject.transform.parent.GetComponent<Card> ();
					if (card.IsFaceDown ()) {
						StartCoroutine (card.Rotate ());
						sound.PlayAudio (_memorySwift);
						if (_firstCard == null) 
							_firstCard = card;
						else {
							_secondCard = card;
						}
					}
				}
			}
			if (_secondCard != null) 
			{
				if (_firstCard.IsFaceUp () && _secondCard.IsFaceUp ()) 
				{
					if (_firstCard.GetSuit () == _secondCard.GetSuit ()) 
					{
						_cardsGuessed += 2;
						_firstCard.Disappear ();
						_secondCard.Disappear ();
						sound.PlayAudio (_memoryFound);
						if (_cardsGuessed >= _cardsTotal) 
						{
							SetGameState (GameState.Won);
							return;
						}
					} 
					else {
						StartCoroutine (_firstCard.Rotate ());
						StartCoroutine (_secondCard.Rotate ());
						sound.PlayAudio (_memoryReturn);
					}
					_firstCard = null;
					_secondCard = null;			
				}
			}
			break;
			#endregion
			#region OVER
		case GameState.Won:
			break;
			#endregion
		}
	}
	#endregion
	
	#region METHODS
	private void _HideAllCards ()
	{
		Card card;
		GameObject [] obj = GameObject.FindGameObjectsWithTag (_card);
		foreach (GameObject cardBack in obj) {
			card = cardBack.transform.parent.GetComponent<Card> ();
			if (card.IsFaceUp ()) 
			{
				StartCoroutine (card.Rotate ());
			}
		}
	}
	
	private IEnumerator _InitiateCountdown ()
	{
		CountdownManager __cdm = GetComponent<CountdownManager> ();
		__cdm.SetSpawnPosition(new Vector3(0f, 0f, -1f));
        __cdm.StartCountdown(this);
		
		while (!__cdm.CountdownDone)
		{
			yield return null;
		}
		
		_HideAllCards ();
		base.SetGameState (GameState.Running);
		
		yield return null;
	}
	#endregion
}
