using UnityEngine;
using System.Collections;

public class BallonScript : Overlay {

	public Texture2D[] texture;
	GUITexture guiT;
	public float speed;
	float timer;
	int index = 0;
	bool _playing = true;
	public int changeSpeed = 0;
	bool once = true;
	bool onceOut = true;
	AudioSource audioSource;

	void Start () 
	{
		guiT = GetComponent<GUITexture>();
		guiT.texture = texture[index];
		audioSource = GetComponent<AudioSource>();
		audioSource.volume = 0;
		StartCoroutine(_FadeIn());
		FadeIn ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		timer += Time.deltaTime;
		if(timer > speed && _playing)
		{
			if(++index < texture.Length)
				guiT.texture = texture[index];
			else
				_playing = false;
			timer = 0;
		}
		if(index > changeSpeed && once)
		{
			speed = speed * 4f; 
			once = false;
		}
		if(index == texture.Length && Input.anyKeyDown && onceOut)
		{
			onceOut = false;
			StartCoroutine(_FadeOut ());
		}
	}
	IEnumerator _FadeIn()
	{
		while(audioSource.volume < 1)
		{
			audioSource.volume += Time.deltaTime;
			yield return null;
		}
	}
	IEnumerator _FadeOut()
	{
		while(audioSource.volume > 0)
		{
			audioSource.volume -= Time.deltaTime;
			yield return null;
		}
		LoadLevel("GameSelectionScene");
	}
}
