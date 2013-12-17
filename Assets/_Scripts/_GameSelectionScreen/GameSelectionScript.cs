#define UNITY_PRO
using UnityEngine;
using System.Collections;

public class GameSelectionScript : Overlay
{
	#region MEMBERS
	public AudioClip audioPress;
	public GameObject camPrefab;
	public Texture2D hugeBackground;
	public float scrollingSpeed;
	public float backScroll;
	public string[] sceneNames;
	public MovieTexture[] buttonTextures;
	public Texture2D frame;
	public Texture2D [] arrows;

	// Entire background will be shifted by this value to create an illusion of "centering" the camera
	public float centerPivotOffset; 
	// This will be define how much background is shifted from center pivot. 
	private float _currentPivotOffset;
	public Rect _backgroundRect;
	private Rect _leftScrollRegion, _rightScrollRegion;
	private Camera _localCamera;
	private AudioSource _localAudioSource;
	private NavigationGUIScript _navGUI;
	private GUIStyle _noStyle = new GUIStyle();
	private Rect _leftArrow, _rightArrow;
	private float _xBackPosition;
	private float _moveBack;
	private float _slowBackMovement;
	#endregion

	private GameSelectionButton[] _gameButtons;
	
	#region UNITY_METHODS
	public override void Awake ()
	{
		base.Awake ();
		Object o = FindObjectOfType (typeof(Camera));
		if (o == null) 
		{
			GameObject c = (GameObject)Instantiate (camPrefab, new Vector3 (0, 0, 0), Quaternion.identity);
			_localCamera = c.camera;
		} 
		else
		{
			_localCamera = (Camera)o;
		}
		
		// Initialize background
		_xBackPosition = 250f;
		_moveBack = -_xBackPosition;

		_backgroundRect = new Rect (_moveBack, 0, Screen.width + _xBackPosition * 2, Screen.height);

		_slowBackMovement = Mathf.Abs (centerPivotOffset) / _xBackPosition;

		// Initialize mouse scroll regions
		float __regionWidth = Screen.width / 3f;
		float __regionVerticalOffset = Screen.height / 8f;
		float __regionHeight = Screen.height - Screen.height / 4f;
		_leftScrollRegion = new Rect (0f, __regionVerticalOffset, __regionWidth, __regionHeight);
		_rightScrollRegion = new Rect (Screen.width - __regionWidth, __regionVerticalOffset, __regionWidth, __regionHeight);
		
		_navGUI = GetComponent<NavigationGUIScript>();
	}
	
	void Start ()
	{
		FadeIn ();
	
		float __buttonWidth = Screen.width / 3.5f;
		float __buttonHeight = Screen.height / 3.5f;

		float __margin = 100f;
		Vector2[] __buttonPositions = new Vector2[sceneNames.Length];
		for (int i = 0; i < __buttonPositions.Length; i++) 
		{
			int __offset = (int)Mathf.Ceil ((float)__buttonPositions.Length / 2f);
			if(i < __offset)
			{
				float __startX =  -200;
				__buttonPositions [i].x = __startX + i * __buttonWidth + i * __margin;
				__buttonPositions [i].y = 230;
			}
			else 
			{
				float __startX = -200 - __buttonWidth / 2f;
				
				__buttonPositions [i].x = __startX + (i -__offset) * __buttonWidth + i * __margin;
				__buttonPositions [i].y = 450;
			}
		}
		/*
		__buttonPositions [0].y = (Screen.height / 2f) + 0.4f * Screen.height / 3.5f;
		__buttonPositions [1].y = (Screen.height / 2f) - 0.15f * Screen.height / 3.5f;
		__buttonPositions [2].y = (Screen.height / 2f) + 0f * Screen.height / 3.5f;
		__buttonPositions [3].y = (Screen.height / 2f) - 0.27f * Screen.height / 3.5f;
		*/
		// Construct game buttons
		_gameButtons = new GameSelectionButton[sceneNames.Length];
		for (int i = 0; i < _gameButtons.Length; i++) 
		{
			_gameButtons [i] = new GameSelectionButton (
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
		for (int i = 0; i < buttonTextures.Length; i++) 
		{
			if (buttonTextures[i] != null) 
			{
				((MovieTexture)buttonTextures[i]).loop = true;
				((MovieTexture)buttonTextures[i]).Play();
			}
		}
		#endif
		float __marginArrow = 0;
		float __arrWidth = 75;
		float __arrHeight = 100;
		_leftArrow = new Rect(__marginArrow, Screen.height / 2 - __arrHeight, __arrWidth, __arrHeight);
		_rightArrow = new Rect(Screen.width - __arrWidth - __marginArrow,Screen.height / 2 - __arrHeight,  __arrWidth, __arrHeight);
	}
	
	void Update ()
	{
		NavigationState __state = Manager.GetNavigationState();
		if(__state == NavigationState.Pause)return;
		// Mouse scrolling
		Vector2 __mouse = InputManager.MouseScreenToGUI ();
		
		// Check if player is hovering any button
		for (int i = 0; i < _gameButtons.Length; i++) 
		{
			bool __contains = (_gameButtons [i].CalcStaticRect ().Contains (__mouse));
			
			// Mouse input
			if (Input.GetMouseButtonDown (0)) 
			{
				if (__contains && _gameButtons [i].startSceneName != "") 
				{
					_localAudioSource.Play ();
					StartCoroutine (_FadeOutAndLoad (_gameButtons [i].startSceneName));
					break;
				}
			}
		}

		if (_leftScrollRegion.Contains (__mouse)) 
		{
			float __force = 1f - __mouse.x / _leftScrollRegion.width;
			float __movement = Time.deltaTime * scrollingSpeed * __force;
			_currentPivotOffset += __movement;
			_moveBack += __movement / _slowBackMovement;
		}
		
		if (_rightScrollRegion.Contains (__mouse) ) 
		{
			float __force = 1f - (Screen.width-__mouse.x) / _rightScrollRegion.width;
			float __movement = Time.deltaTime * scrollingSpeed * __force;
			_currentPivotOffset -= __movement;
			_moveBack -= __movement / _slowBackMovement;
		}
		
		_currentPivotOffset = Mathf.Clamp (_currentPivotOffset, 
			-Mathf.Abs(centerPivotOffset), Mathf.Abs(centerPivotOffset));
		_moveBack = Mathf.Clamp (_moveBack,-500f,0f);
		for (int i = 0; i < _gameButtons.Length; i++) 
		{
			_gameButtons [i].horizontalOffset = _currentPivotOffset;
			// If hovering, float button
			if (_gameButtons [i].CalcStaticRect().Contains(__mouse)) 
			{
				_gameButtons[i].FloatUp();
			} 
			else 
			{
				_gameButtons[i].FloatBack();
			}
		}
	}
	
	// Update is called once per frame
	void OnGUI ()
	{
		// Draw background
		_backgroundRect.x = _moveBack;
		GUI.DrawTexture (_backgroundRect, hugeBackground);
		
		#if UNITY_EDITOR
		// Draw mouse scroll regions
		//GUI.Box(leftScrollRegion, "left");
		//GUI.Box(rightScrollRegion, "right");
		#endif

		// Draw buttons
		for (int i = 0; i < _gameButtons.Length; i++) 
		{
			//GUI.Box(_gameButtons[i].CalcBGRect(), "");
			Rect __frameRect = _gameButtons[i].CalcRect();
			__frameRect.x -= 25;
			__frameRect.y -= 15;
			__frameRect.width += 45;
			__frameRect.height += 25;
			#if UNITY_PRO
			if (_gameButtons[i].texture != null) 
			{
				GUI.Box(_gameButtons[i].CalcRect(),_gameButtons[i].texture,_noStyle);
			} 
			else 
			{
				GUI.Box(_gameButtons[i].CalcRect(), _gameButtons[i].startSceneName);
			}
			#else
			GUI.Box (gameButtons [i].CalcRect (), gameButtons [i].startSceneName);
			#endif
			GUI.DrawTexture(__frameRect,frame);
			GUI.DrawTexture(_leftArrow, arrows[0]);
			GUI.DrawTexture(_rightArrow, arrows[1]);

		}
		
		_navGUI.Draw();
	}
	#endregion
	
	#region METHODS
	IEnumerator _FadeOutAndLoad (string scene)
	{
		AudioSource source = _localCamera.audio;
		LoadLevel(scene);
		while (source.volume > 0.2f || _localAudioSource.isPlaying) 
		{
			source.volume -= Time.deltaTime * 0.2f;
			yield return null;
		}
	}
	#endregion
}
