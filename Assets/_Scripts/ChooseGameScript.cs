using UnityEngine;
using System.Collections;

public class ChooseGameScript : Overlay
{
	#region MEMBERS
	public AudioClip audioPress;
	public Texture2D otter, hedgehog, tree, horse, owl, dragon, bear, granma, seaDragon;
	public Texture2D blonde, brune, fox, boy;
	public GameObject camPrefab;
	
	private Camera cam;
	private GUITexture background;
	private Rect characterBoxRect;
	private Texture2D chosen;
	private GUIStyle noStyle = new GUIStyle ();
	private AudioSource audioSource;
	
	public Texture2D hugeBackground;
	public float scrollingSpeed;
	
	public MovieTexture[] buttonTextures;
	
	// Entire background will be shifted by this value to create an illusion of "centering" the camera
	private float centerPivotOffset; 
	
	// This will be define how much background is shifted from center pivot. 
	private float currentPivotOffset;
	private Rect backgroundRect;
	private Rect leftScrollRegion, rightScrollRegion;
	#endregion
	
	public class GameButton {
		public Rect rect;
		public MovieTexture texture;
		public float horizontalOffset;
		public string startSceneName;
		
		public GameButton(float x, float y, float width, float height, MovieTexture texture, string startSceneName) {
			this.rect = new Rect(x + width / 2f, y - height / 2f, width, height);
			this.texture = texture;
			this.startSceneName = startSceneName;
		}
		
		public Rect CalcRect() {
			Rect __r = new Rect(rect);
			__r.x += horizontalOffset;
			return __r;
		}
	}
	
	private GameButton[] gameButtons;
	
	#region UNITY_METHODS
	public override void Awake ()
	{
		base.Awake ();
		Object o = FindObjectOfType (typeof(Camera));
		if (o == null) {
			GameObject c = (GameObject)Instantiate (camPrefab, new Vector3 (0, 0, 0), Quaternion.identity);
			cam = c.camera;
		} else {
			cam = (Camera)o;
		}
		
		// Initialize background
		float __width = hugeBackground.width;
		float __height = hugeBackground.height;
		backgroundRect = new Rect(__width / 2f, 0f, hugeBackground.width, hugeBackground.height);
		
		// Initialize mouse scroll regions
		float __regionWidth = Screen.width / 6f;
		leftScrollRegion = new Rect(0f, 0f, __regionWidth, Screen.height);
		rightScrollRegion = new Rect(Screen.width - __regionWidth, 0f, __regionWidth, Screen.height);
		
		// Pivot offset will always be negative and one quarter of backgrounds width
		centerPivotOffset = -__width / 4f;
	}
	
	void Start ()
	{
		FadeIn ();
		background = GetComponent<GUITexture> ();
		
		/* 
		 * Positions are randomly generated and evenly spread across window.
		 * Will change later.
		 */ 
		float __buttonWidth = Screen.width / 8f;
		float __buttonHeight = Screen.height / 8f;
		float __startX = -Screen.width/2f - __buttonWidth / 2f;
		float __periodX = __buttonWidth * 1.365f;
		Vector2[] __buttonPositions = new Vector2[10];
		for (int i = 0; i < __buttonPositions.Length; i++) {
			__buttonPositions[i].x = __startX + (i+1) * __periodX;
			__buttonPositions[i].y = (Screen.height/2f) + Random.Range(-1f, 1f) * Screen.height/4f;
		}
		
		/*
		 * Construct game buttons
		 * 1. & 2. param: position x, position y
		 * 2. & 3. param: size width, size height
		 * 4. param: MovieTexture
		 * 5. param: Scene to Load
		 */ 
		gameButtons = new GameButton[10];
		gameButtons[0] = new GameButton(
			__buttonPositions[0].x, __buttonPositions[0].y, 
			__buttonWidth, __buttonHeight, 
			buttonTextures[0], "Flip_1");
		
		gameButtons[1] = new GameButton(
			__buttonPositions[1].x, __buttonPositions[1].y, 
			__buttonWidth, __buttonHeight, 
			buttonTextures[0], "Diff_1");
		
		gameButtons[2] = new GameButton(
			__buttonPositions[2].x, __buttonPositions[2].y, 
			__buttonWidth, __buttonHeight, 
			buttonTextures[0], "Coloring_1");
		
		gameButtons[3] = new GameButton(
			__buttonPositions[3].x, __buttonPositions[3].y, 
			__buttonWidth, __buttonHeight, 
			buttonTextures[0], "Horse_1");
		
		gameButtons[4] = new GameButton(
			__buttonPositions[4].x, __buttonPositions[4].y, 
			__buttonWidth, __buttonHeight, 
			buttonTextures[0], "nullscene");
		
		gameButtons[5] = new GameButton(
			__buttonPositions[5].x, __buttonPositions[5].y, 
			__buttonWidth, __buttonHeight, 
			buttonTextures[0], "nullscene");
		
		gameButtons[6] = new GameButton(
			__buttonPositions[6].x, __buttonPositions[6].y, 
			__buttonWidth, __buttonHeight, 
			buttonTextures[0], "nullscene");
		
		gameButtons[7] = new GameButton(
			__buttonPositions[7].x, __buttonPositions[7].y, 
			__buttonWidth, __buttonHeight, 
			buttonTextures[0], "nullscene");
		
		gameButtons[8] = new GameButton(
			__buttonPositions[8].x, __buttonPositions[8].y, 
			__buttonWidth, __buttonHeight, 
			buttonTextures[0], "nullscene");
		
		gameButtons[9] = new GameButton(
			__buttonPositions[9].x, __buttonPositions[9].y, 
			__buttonWidth, __buttonHeight, 
			buttonTextures[0], "nullscene");
		
		// Character widget
		characterBoxRect = new Rect (0, 0, 120, 120);
		chosen = _GetChosenCharacter ();
		
		// Sound system
		audioSource = GetComponent<AudioSource> ();
		audioSource.clip = audioPress;
		audioSource.volume = 0.5f;
		
		// This hack kills any extra music objects
		GameObject t = GameObject.Find ("MusicMemory(Clone)");
		if (t != null)
			Destroy (t);
		
		// Set movie textures to loop and start playing them
		for (int i = 0; i < buttonTextures.Length; i++) {
			buttonTextures[i].loop = true;
			buttonTextures[i].Play();
		}
	}
	
	void Update () {
		// Mouse scrolling
		Vector2 __mouse = InputManager.MouseScreenToGUI();
		if (leftScrollRegion.Contains(__mouse)) {
			currentPivotOffset += Time.deltaTime * scrollingSpeed;
		}
		
		if (rightScrollRegion.Contains(__mouse)) {
			currentPivotOffset -= Time.deltaTime * scrollingSpeed;
		}
		
		currentPivotOffset = Mathf.Clamp(currentPivotOffset, -Screen.width / 2f, Screen.width / 2f);
		
		for (int i = 0; i < gameButtons.Length; i++) {
			gameButtons[i].horizontalOffset = currentPivotOffset;
		}
	}
	
	// Update is called once per frame
	void OnGUI ()
	{
		float __width = hugeBackground.width;
		float __height = hugeBackground.height;
		backgroundRect = new Rect(centerPivotOffset + currentPivotOffset, 0f, hugeBackground.width, hugeBackground.height);
		GUI.DrawTexture(backgroundRect, hugeBackground);
		
		GUI.Box(leftScrollRegion, "left");
		GUI.Box(rightScrollRegion, "right");

		// Character widget
		NavigationState currentState = Manager.GetNavigationState ();
		GUI.enabled = true;
		GUI.Box (characterBoxRect, chosen, noStyle);
		if (currentState == NavigationState.Pause) {
			GUI.enabled = false;
		}

		// Very very ugly code
		// First loop only draws buttons if they are NOT hovered, to make SURE they are
		// behind every other buttons, especially when they enlarge ...
		for (int i = 0; i < gameButtons.Length; i++) {
			if (!gameButtons[i].CalcRect().Contains(InputManager.MouseScreenToGUI())) {
				if (MGUI.HoveredButton (gameButtons[i].CalcRect(), gameButtons[i].texture, 3f)) {
					audioSource.Play ();
					StartCoroutine (_FadeOutAndLoad (gameButtons[i].startSceneName));
					
				}
			}
		}
		// ... second loop only draws one hovered button. We only draw 10 buttons,
		// so no duplicates, but lots of unnecessary calculations.
		for (int i = 0; i < gameButtons.Length; i++) {
			if (gameButtons[i].CalcRect().Contains(InputManager.MouseScreenToGUI())) {
				if (MGUI.HoveredButton (gameButtons[i].CalcRect(), gameButtons[i].texture, 3f)) {
					audioSource.Play ();
					StartCoroutine (_FadeOutAndLoad (gameButtons[i].startSceneName));
				}
			}
		}
	}
	#endregion
	
	#region METHODS
	IEnumerator _FadeOutAndLoad (string scene)
	{
		AudioSource source = cam.audio;
		LoadLevel (scene);
		while (source.volume > 0.2f || audioSource.isPlaying) {
			source.volume -= Time.deltaTime * 0.2f;
			yield return null;
		}
	}

	Texture2D _GetChosenCharacter ()
	{
		Character _character = Manager.GetCharacter ();
		switch (_character) {
		case Character.Blonde:
			return blonde;
		case Character.Brune:
			return brune;
		case Character.Boy:
			return boy;
		case Character.Fox:
			return fox;
		case Character.None:
			return blonde;
		default:
			return blonde;
		}
	}
	#endregion
}
