using UnityEngine;
using System.Collections;

public class ChoiceScreenScript : Overlay
{
	#region MEMBERS
	public GameObject cam;
	public Texture2D blonde, brunette, fox, boy, map, button;
	public AudioClip audioPress;
	private AudioSource _audioSource;
	private GUITexture _background;
	private Rect _backgroundRect, _blondeRect, _bruneRect, _foxRect, _boyRect, _mapRect;
	private Vector2 _mapBeginPos, _mapEndPos;
	private bool _characterChosen = false;
	private bool _buttonIsSliding = false;
	private bool _buttonSlideIsDone = false;
	#endregion
	
	#region UNITY_METHODS
	public override void Awake ()
	{
		base.Awake ();
		Object o = FindObjectOfType (typeof(Camera));
		if (o == null) 
		{
			Instantiate (cam, new Vector3 (0, 0, 0), Quaternion.identity);
		}
	}

	void Start ()
	{
		_audioSource = GetComponent<AudioSource> ();
		_audioSource.clip = audioPress;
		_background = GetComponent<GUITexture> ();
		// Setting background to full screen
		float width = Screen.width;
		float height = Screen.height;
		float __halfWidth = Screen.width / 2;
		
		_backgroundRect = new Rect (-width / 2, - height / 2, width, height);
		_background.pixelInset = _backgroundRect;
		
		const float __yUpper = 187f;
		const float __yBottom = 421f;
		const float __margin = 8f;
		const float __buttonWidth = 150f;
		
		// Upper row
		_blondeRect = new Rect (__halfWidth - 2 * __buttonWidth - 3 * __margin, __yUpper, __buttonWidth, __buttonWidth);
		_bruneRect = new Rect (__halfWidth - __buttonWidth - __margin, __yUpper, __buttonWidth, __buttonWidth);
		_foxRect = new Rect (__halfWidth + __margin, __yUpper, __buttonWidth, __buttonWidth);
		_boyRect = new Rect (__halfWidth + __margin * 3 + __buttonWidth, __yUpper, __buttonWidth, __buttonWidth);
		
		// Bottom row
		_mapEndPos = new Vector2 (Screen.width + __buttonWidth, __yBottom);
		_mapBeginPos = new Vector2 (__halfWidth - 0.5f * __buttonWidth, __yBottom);
		_mapRect = new Rect (_mapBeginPos.x, _mapBeginPos.y, __buttonWidth, __buttonWidth);

		FadeIn ();
	}

	void OnGUI ()
	{
		if (MGUI.HoveredButton (_blondeRect, blonde)) 
		{
			_audioSource.Play ();
			Manager.SetCharacter (Character.Blonde);
			_characterChosen = true;
		}
		if (MGUI.HoveredButton (_bruneRect, brunette)) 
		{
			_audioSource.Play ();
			Manager.SetCharacter (Character.Brune);
			_characterChosen = true;
		}
		if (MGUI.HoveredButton (_foxRect, fox)) 
		{
			_audioSource.Play ();
			Manager.SetCharacter (Character.Fox);
			_characterChosen = true;
		}
		if (MGUI.HoveredButton (_boyRect, boy)) 
		{
			_audioSource.Play ();
			Manager.SetCharacter (Character.Boy);
			_characterChosen = true;
		}
		if (_characterChosen) 
		{
			if (!_buttonIsSliding && !_buttonSlideIsDone)
				StartCoroutine (SlideButton ());
			
			if (MGUI.HoveredButton (_mapRect, map) && _buttonSlideIsDone) 
			{
				LoadLevelAndPlaySound ("GameSelectionScene", _audioSource);
			}
		}
	}

	private IEnumerator SoundAndLoad ()
	{
		_audioSource.Play ();
		while (_audioSource.isPlaying) {
			yield return null;
		}
		Manager.SetScreenChoice (ScreenChoice.Button);
		if(Application.CanStreamedLevelBeLoaded("GameSelectionScene"))
		{
			Application.LoadLevel ("GameSelectionScene");
		}
	}
	
	private IEnumerator SlideButton ()
	{
		_buttonIsSliding = true;
		
		float __t = 1f;
		while (__t > 0f) {
			__t -= Time.deltaTime * 2f;
			_mapRect.x = Mathf.Lerp (_mapBeginPos.x, _mapEndPos.x, __t);
			yield return null;
		}
		
		_buttonSlideIsDone = true;
		_buttonIsSliding = false;
		yield return null;
	}
	#endregion
}
