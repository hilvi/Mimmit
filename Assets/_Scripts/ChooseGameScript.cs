using UnityEngine;
using System.Collections;

public class ChooseGameScript : Overlay 
{
	#region MEMBERS
	GUITexture background;
	
	public Texture2D otter, hedgehog, tree, horse, owl, dragon,bear,granma,seaDragon;
	Rect /*mapRect,*/ otterRect, treeRect, horseRect, owlRect/*, hedgehogRect,  dragonRect, bearRect,granmaRect,seaDragonRect*/;
	Rect characterBoxRect;
	public Texture2D blonde, brune, fox, boy;
	public GameObject camPrefab;
	Texture2D chosen;
	GUIStyle noStyle = new GUIStyle();
	Camera cam;
	public AudioClip audioPress;
	AudioSource audioSource;
	#endregion
	
	#region UNITY_METHODS
	public override void Awake()
	{
		base.Awake();
		Object o = FindObjectOfType(typeof(Camera));
		if(o == null)
		{
			GameObject c = (GameObject)Instantiate (camPrefab, new Vector3 (0,0,0), Quaternion.identity);
			cam = c.camera;
		}else
		{
			cam = (Camera)o;
		}
	}
	void Start () 
	{
		FadeIn ();
		background = GetComponent<GUITexture>();
		// Setting background to full screen
		float width = Screen.width;
		float height = Screen.height;
		Rect rect = new Rect(-width / 2, - height / 2, width, height);
		background.pixelInset = rect;
		
		// Here for some reasons, you may have a little problem to see the underscore in front of the variables...
		// They do not show up for me even though they are there
		float __startY = 50f;
		float __edge = 110;
		float __size = 130;
		float __margin = (__size - __edge ) / 2;
		float __startX = (960 - __size * 4f)/2f;
		owlRect = 		new Rect(__startX + 1.5f * __size + __margin, __startY + __margin,__edge,__edge);
	
		treeRect = 		new Rect(__startX + __size + __margin, __startY + __size + __margin,__edge,__edge);// 285 250
		otterRect = 	new Rect(__startX + 2f * __size + __margin, __startY + __size + __margin,__edge,__edge);
		
		//hedgehogRect = 	new Rect(__startX + 0.5f  *__size+__margin, __startY + 2f * __size+ __margin,__edge,__edge);
		horseRect = 	new Rect(__startX + 1.5f  *__size + __margin, __startY + 2f * __size + __margin,__edge,__edge);
		//dragonRect =	new Rect(__startX + 2.5f  *__size+ __margin, __startY + 2f * __size + __margin,__edge,__edge);
		
		//bearRect =	 	new Rect(__startX + 0.5f * __size+__margin, __startY + 3f * __size+__margin,__edge,__edge);
		//granmaRect = 	new Rect(__startX + 1.5f * __size+__margin, __startY + 3f * __size+__margin,__edge,__edge);
		//seaDragonRect = new Rect(__startX + 2.5f * __size+__margin, __startY + 3f * __size+__margin,__edge,__edge);
		characterBoxRect = new Rect(20,20,200,200);
		chosen = _GetChosenCharacter();
		audioSource = GetComponent<AudioSource>();
		audioSource.clip = audioPress;
		audioSource.volume = 0.5f;
		
		// This hack kills any extra music objects
		GameObject t = GameObject.Find("MusicMemory(Clone)");
		if (t != null)
			Destroy (t);
	}

	// Update is called once per frame
	void OnGUI ()
	{
		NavigationState currentState = Manager.GetNavigationState();
		GUI.enabled = true;
		GUI.Box (characterBoxRect,chosen,noStyle);
		if(currentState == NavigationState.Pause)
		{
			GUI.enabled = false;
		}
		
		/*if(GUI.Button (mapRect,"Map"))
		{
			Application.LoadLevel("MapWorld");
		}*/
		if(MGUI.HoveredButton (owlRect,owl))
		{
			audioSource.Play ();
			StartCoroutine(_FadeOutAndLoad("Flip_1"));
		}
		if(MGUI.HoveredButton (treeRect,tree))
		{
			audioSource.Play ();
			StartCoroutine(_FadeOutAndLoad("Diff_1"));
		}
		if(MGUI.HoveredButton (otterRect,otter))
		{
			audioSource.Play();
			StartCoroutine(_FadeOutAndLoad("Coloring_1"));
		}
		if(MGUI.HoveredButton (horseRect,horse))
		{
			audioSource.Play();
			StartCoroutine(_FadeOutAndLoad("Horse_1"));
		}
		
		/*
		//Unfinished games
		GUI.enabled = false;
		if(MGUI.HoveredButton (hedgehogRect,hedgehog))
		{
			
		}
		if(MGUI.HoveredButton (dragonRect,dragon))
		{
			
		}
		if(MGUI.HoveredButton (bearRect,bear))
		{
			
		}
		if(MGUI.HoveredButton (granmaRect,granma))
		{
			
		}
		if(MGUI.HoveredButton (seaDragonRect,seaDragon))
		{
			
		}
		GUI.enabled = true;
		*/
	}
	#endregion
	
	#region METHODS
	IEnumerator _FadeOutAndLoad (string scene)
	{
		AudioSource source = cam.audio;
		LoadLevel(scene);
		while(source.volume > 0.2f || audioSource.isPlaying)
		{
			source.volume -= Time.deltaTime*0.2f;
			yield return null;
		}
	}
	Texture2D _GetChosenCharacter ()
	{
		Character _character = Manager.GetCharacter();
		switch(_character)
		{
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
