using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SoundManager))]
[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(LevelGenerator))]
public class FlipsGameManager : GameManager {
	
	public float revealTime = 3; // The time, in seconds, for which the cards are revealed at the beginning of the level
	
	public InputManager inputManager;
	public LevelGenerator levelGenerator;
	public GUIText statusLine;
	public SoundManager sound;

	Card firstCard = null; // Handles to the two cards the player is currently flipping
	Card secondCard = null;
	
	int cardsTotal;
	int cardsGuessed = 0;
	Camera cam;
	public GameObject musicObject;
	static GameObject obj;
	
	// Use this for initialization
	public override void Start () 
	{
		base.Start ();
		//obj = null;
		//obj = GameObject.FindGameObjectWithTag("SoundCam");
		if(InGameMenuGUI.music == null)
		{
		  	InGameMenuGUI.music = (GameObject)Instantiate(musicObject);
		}
		cardsTotal = levelGenerator.CardCount();
		SetGameState(GameState.Pregame);
		statusLine.pixelOffset = new Vector2(Screen.width/2, -Screen.height /2 );
		cam = Camera.main;
		StartCoroutine(UpdateStatus());
	}
	
	// Update is called once per frame
	void Update () 
	{
		GameState currentState = GetGameState();
		switch(currentState)
		{
			#region PREGAME
		 	case GameState.Pregame:	
				if (inputManager.IsEscapeButtonDown()) 
					PauseGame();
				break;
			#endregion
			#region RUNNING
			case GameState.Running:
				if (inputManager.IsEscapeButtonDown()) 
					PauseGame();
				Ray ray = cam.ScreenPointToRay(inputManager.GetCursorPosition());
		   		RaycastHit hit;
			
		        if (inputManager.IsButtonDown() && Physics.Raycast(ray, out hit) && !secondCard)
				{
					if (hit.collider.CompareTag("Card")) 
					{
						Card card = hit.collider.gameObject.transform.parent.GetComponent<Card>();
						if (card.IsFaceDown()) 
						{
							StartCoroutine(card.Rotate());
							sound.PlayAudio("MemorySwift");
							if (firstCard==null) 
								firstCard = card;
							else {
								secondCard = card;
							}
						}
					}
				}
				if (secondCard != null) 
				{
					if (firstCard.IsFaceUp() && secondCard.IsFaceUp()) 
					{
						if (firstCard.GetSuit() == secondCard.GetSuit()) 
						{
							cardsGuessed += 2;
							firstCard.Disappear();
							secondCard.Disappear();
							sound.PlayAudio("MemoryFound");
							if (cardsGuessed >= cardsTotal) 
							{
								SetGameState(GameState.Over);
								return;
							}
						}else
						{
							StartCoroutine(firstCard.Rotate());
							StartCoroutine(secondCard.Rotate ());
							sound.PlayAudio("MemoryReturn");
						}
						
						firstCard = null;
						secondCard = null;			
					}
				}
				break;
			#endregion
			#region OVER
			case GameState.Over:
				break;
			#endregion
		}
	}
	
	
	void HideAllCards() 
	{
		Card card;
		GameObject [] obj = GameObject.FindGameObjectsWithTag("Card");
		foreach (GameObject cardBack in obj) 
		{
			card = cardBack.transform.parent.GetComponent<Card>();
			if (card.IsFaceUp()) 
			{
				StartCoroutine(card.Rotate());
			}
		}
	}

	IEnumerator UpdateStatus() 
	{
		float goTimer = 1;
		while (revealTime > 0) 
		{
			revealTime -= Time.deltaTime;
			statusLine.guiText.material.color = new Color(1f,1f,1f,1.5f - (Mathf.Ceil(revealTime) - revealTime));
			statusLine.text = (Mathf.Ceil(revealTime)).ToString();
			statusLine.fontSize = (int)(80 + 100*(Mathf.Ceil(revealTime) - revealTime));
			yield return null;
		}
		
		HideAllCards();
		
		base.SetGameState(GameState.Running);
		while (goTimer > 0) 
		{
			statusLine.text = "Go!";
			statusLine.guiText.material.color = new Color(1f,1f,1f,goTimer);
			statusLine.fontSize = (int)(80 + 100*(Mathf.Ceil(goTimer) - goTimer));
			goTimer -= Time.deltaTime;
			yield return null;
		}
		statusLine.gameObject.SetActive(false);	
	}
}
