//#define UNITY_PRO
using UnityEngine;
using System.Collections;

public class GameSelectionScript : Overlay
{
	#region MEMBERS
	public AudioClip audioPress;
	public GameObject camPrefab;
	public Texture2D hugeBackground;
	public float scrollingSpeed;
	public string[] sceneNames;
	public MovieTexture[] buttonTextures;

	// Entire background will be shifted by this value to create an illusion of "centering" the camera
	public float centerPivotOffset; 
	// This will be define how much background is shifted from center pivot. 
	private float currentPivotOffset;
	private Rect backgroundRect;
	private Rect leftScrollRegion, rightScrollRegion;
	private Camera _localCamera;
	private AudioSource _localAudioSource;
	private NavigationGUIScript navGUI;
	#endregion

	private GameSelectionButton[] gameButtons;
	
	#region UNITY_METHODS
	public override void Awake ()
	{
		base.Awake ();
		Object o = FindObjectOfType (typeof(Camera));
		if (o == null) {
			GameObject c = (GameObject)Instantiate (camPrefab, new Vector3 (0, 0, 0), Quaternion.identity);
			_localCamera = c.camera;
		} else {
			_localCamera = (Camera)o;
		}
		
		// Initialize background
		backgroundRect = new Rect (0f, 0f, hugeBackground.width, hugeBackground.height);

		// Initialize mouse scroll regions
		float __regionWidth = Screen.width / 3f;
		float __regionVerticalOffset = Screen.height / 8f;
		float __regionHeight = Screen.height - Screen.height / 4f;
		leftScrollRegion = new Rect (0f, __regionVerticalOffset, __regionWidth, __regionHeight);
		rightScrollRegion = new Rect (Screen.width - __regionWidth, __regionVerticalOffset, __regionWidth, __regionHeight);
		
		navGUI = GetComponent<NavigationGUIScript>();
	}
	
	void Start ()
	{
		FadeIn ();
		
		// Some hardcore magic number wizardry, dont try this at home.
		// Manual placement will replace this in future.
		float __buttonWidth = Screen.width / 8f;
		float __buttonHeight = Screen.height / 8f;
		float __startX = 0f;
		Vector2[] __buttonPositions = new Vector2[10];
		for (int i = 0; i < __buttonPositions.Length; i++) {
			if (i < 5) {
				__buttonPositions [i].x = __startX + i * __buttonWidth + i * 60f;
				__buttonPositions [i].y = (Screen.height / 2f) + Random.Range (-1f, -0.5f) * Screen.height / 3.5f;
			} else {
				__buttonPositions [i].x = __startX + (i - 5) * __buttonWidth + (i - 5) * 60f;
				__buttonPositions [i].y = (Screen.height / 2f) + Random.Range (0.5f, 1f) * Screen.height / 3.5f;
			}
		}
		
		// Construct game buttons
		gameButtons = new GameSelectionButton[10];
		for (int i = 0; i < gameButtons.Length; i++) {
			gameButtons [i] = new GameSelectionButton (
				__buttonPositions [i].x, // Position x
				__buttonPositions [i].y, // Position y
				__buttonWidth, // Size width
				__buttonHeight, // Size height
				buttonTextures [i], // Movie Texture
				sceneNames [i],  // Scene name
				10f // Border width
			);			
		}

		// Sound system
		_localAudioSource = GetComponent<AudioSource> ();
		_localAudioSource.clip = audioPress;
		_localAudioSource.volume = 0.5f;
		
		// This hack kills any extra music objects
		GameObject t = GameObject.Find ("MusicMemory(Clone)");
		if (t != null)
			Destroy (t);
		
		#if UNITY_PRO
		// Set movie textures to loop and start playing them
		for (int i = 0; i < buttonTextures.Length; i++) {
			((MovieTexture)buttonTextures[i]).loop = true;
			((MovieTexture)buttonTextures[i]).Play();
		}
		#endif
	}
	
	void Update ()
	{
		// Mouse scrolling
		Vector2 __mouse = InputManager.MouseScreenToGUI ();
		
		// Check if player is hovering any button
		bool __buttonHovering = false;
		for (int i = 0; i < gameButtons.Length; i++) {
			bool __contains = (gameButtons [i].CalcStaticRect ().Contains (__mouse));
			
			if  (__contains)
				__buttonHovering = true;
			
			// Mouse input
			if (Input.GetMouseButtonDown (0)) {
				if (__contains && gameButtons [i].startSceneName != "") {
					_localAudioSource.Play ();
					StartCoroutine (_FadeOutAndLoad (gameButtons [i].startSceneName));
					break;
				}
			}
		}

		if (leftScrollRegion.Contains (__mouse) && !__buttonHovering) {
			float __force = 1f - __mouse.x / leftScrollRegion.width;
			currentPivotOffset += Time.deltaTime * scrollingSpeed * __force;
		}
		
		if (rightScrollRegion.Contains (__mouse) && !__buttonHovering) {
			float __force = 1f - (Screen.width-__mouse.x) / rightScrollRegion.width;
			currentPivotOffset -= Time.deltaTime * scrollingSpeed * __force;
		}
		
		currentPivotOffset = Mathf.Clamp (currentPivotOffset, 
			-Mathf.Abs(centerPivotOffset), Mathf.Abs(centerPivotOffset));
		
		for (int i = 0; i < gameButtons.Length; i++) {
			gameButtons [i].horizontalOffset = currentPivotOffset;
			// If hovering, float button
			if (gameButtons [i].CalcStaticRect().Contains(__mouse)) {
				gameButtons[i].FloatUp();
			} else {
				gameButtons[i].FloatBack();
			}
		}
	}
	
	// Update is called once per frame
	void OnGUI ()
	{
		// Draw background
		backgroundRect = new Rect (centerPivotOffset + currentPivotOffset, 0f, hugeBackground.width, hugeBackground.height);
		GUI.DrawTexture (backgroundRect, hugeBackground);
		
		#if UNITY_EDITOR
		// Draw mouse scroll regions
		//GUI.Box(leftScrollRegion, "left");
		//GUI.Box(rightScrollRegion, "right");
		#endif

		// Draw buttons
		for (int i = 0; i < gameButtons.Length; i++) {
			GUI.Box(gameButtons[i].CalcBGRect(), "");
			
			#if UNITY_PRO
			GUI.DrawTexture(gameButtons[i].CalcRect(), gameButtons[i].texture);
			#else
			GUI.Box (gameButtons [i].CalcRect (), gameButtons [i].startSceneName);
			#endif
		}
		
		navGUI.Draw();
	}
	#endregion
	
	#region METHODS
	IEnumerator _FadeOutAndLoad (string scene)
	{
		AudioSource source = _localCamera.audio;
		LoadLevel (scene);
		while (source.volume > 0.2f || _localAudioSource.isPlaying) {
			source.volume -= Time.deltaTime * 0.2f;
			yield return null;
		}
	}
	#endregion
}
