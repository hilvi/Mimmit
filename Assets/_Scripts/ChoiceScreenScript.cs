using UnityEngine;
using System.Collections;

public class ChoiceScreenScript : Overlay 
{
	
	public Texture2D blonde, brunette, fox, boy, map, button;
	public AudioClip audioPress;
	GUITexture background;
	// Use this for initialization
	Rect blondeRect, bruneRect, foxRect , boyRect, mapRect, buttonRect; 
	bool characterChosen = false;
	public GameObject cam;
	AudioSource audioSource;
	public override void Awake()
	{
		base.Awake();
		Object o = FindObjectOfType(typeof(Camera));
		if(o == null)
		{
			Instantiate (cam, new Vector3 (0,0,0), Quaternion.identity);
		}
	}
	void Start() {
		audioSource = GetComponent<AudioSource>();
		audioSource.clip = audioPress;
		background = GetComponent<GUITexture>();
		// Setting background to full screen
		float width = Screen.width;
		float height = Screen.height;
		Rect rect = new Rect(-width / 2, - height / 2, width, height);
		background.pixelInset = rect;
		
		float halfWidth = Screen.width / 2;
		int _y = 187; int _width = 150;
		float margin = 8;
		blondeRect 	= new Rect(halfWidth - 2 * _width - 3*margin ,_y,_width,_width);
		bruneRect 	= new Rect(halfWidth - _width - margin,_y,_width,_width);
		foxRect 	= new Rect(halfWidth+margin,_y,_width,_width);
		boyRect 	= new Rect(halfWidth+margin * 3 +  _width,_y,_width,_width);
		
  		float _ySecondButtons = 421;
		mapRect = new Rect(halfWidth -  0.5f * _width, _ySecondButtons,_width,_width);
		//buttonRect = new Rect(halfWidth + 0.5f * _width,_ySecondButtons,_width,_width);
		FadeIn ();

	}
	

	void OnGUI()
	{
		if(MGUI.HoveredButton(blondeRect,blonde))
		{
			audioSource.Play();
			Manager.SetCharacter(Character.Blonde);
			characterChosen = true;
		}
		if(MGUI.HoveredButton(bruneRect,brunette))
		{
			audioSource.Play();
			Manager.SetCharacter(Character.Brune);
			characterChosen = true;
		}
		if(MGUI.HoveredButton(foxRect,fox))
		{
			audioSource.Play();
			Manager.SetCharacter(Character.Fox);
			characterChosen = true;
		}
		if(MGUI.HoveredButton(boyRect,boy))
		{
			audioSource.Play();
			Manager.SetCharacter(Character.Boy);
			characterChosen = true;
		}
		if(characterChosen)
		{
			if(MGUI.HoveredButton(mapRect,map))
			{
				LoadLevelAndPlaySound("ChooseGameScene", audioSource);
			}
			/*if(GUI.Button(buttonRect,"ButtonScreenScene"))
			{
				Manager.SetScreenChoice(ScreenChoice.Button);
				Application.LoadLevel("ChooseGameScene");
			}*/
		}
		
	}
	IEnumerator SoundAndLoad()
	{
		audioSource.Play();
		while(audioSource.isPlaying)
		{
			yield return null;
		}
		Manager.SetScreenChoice(ScreenChoice.Button);
		LoadLevel("ChooseGameScene");
	}
}
