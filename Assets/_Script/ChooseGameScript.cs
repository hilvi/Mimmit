using UnityEngine;
using System.Collections;

public class ChooseGameScript : MonoBehaviour {
	
	GUITexture background;
	
	public Texture2D otter, hedgehog, tree, horse, owl, dragon,bear,granma,seaDragon;
	Rect /*mapRect,*/ otterRect, hedgehogRect, treeRect, horseRect, owlRect, dragonRect, bearRect,granmaRect,seaDragonRect;
	Rect characterBoxRect;
	public Texture2D blonde, brune, fox, boy;
	public GameObject camPrefab;
	Texture2D chosen;
	GUIStyle noStyle = new GUIStyle();
	Camera cam;
	public AudioClip audioPress;
	AudioSource audioSource;

	Texture2D GetChosenCharacter ()
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
	void Awake()
	{
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
		background = GetComponent<GUITexture>();
		// Setting background to full screen
		float width = Screen.width;
		float height = Screen.height;
		Rect rect = new Rect(-width / 2, - height / 2, width, height);
		background.pixelInset = rect;
		
		
		float __startY = 50f;
		float __edge = 110;
		float __size = 130;
		float __margin = (__size - __edge ) / 2;
		float __startX = (960 - __size * 4f)/2f;
		owlRect = 		new Rect(__startX + 1.5f * __size + __margin, __startY + __margin,__edge,__edge);
	
		treeRect = 		new Rect(__startX + __size + __margin, __startY + __size + __margin,__edge,__edge);// 285 250
		otterRect = 	new Rect(__startX + 2f * __size + __margin, __startY + __size + __margin,__edge,__edge);
		
		hedgehogRect = 	new Rect(__startX + 0.5f  *__size+__margin, __startY + 2f * __size+ __margin,__edge,__edge);
		horseRect = 	new Rect(__startX + 1.5f  *__size + __margin, __startY + 2f * __size + __margin,__edge,__edge);
		dragonRect =	new Rect(__startX + 2.5f  *__size+ __margin, __startY + 2f * __size + __margin,__edge,__edge);
		
		bearRect =	 	new Rect(__startX + 0.5f * __size+__margin, __startY + 3f * __size+__margin,__edge,__edge);
		granmaRect = 	new Rect(__startX + 1.5f * __size+__margin, __startY + 3f * __size+__margin,__edge,__edge);
		seaDragonRect = new Rect(__startX + 2.5f * __size+__margin, __startY + 3f * __size+__margin,__edge,__edge);
		characterBoxRect = new Rect(20,20,200,200);
		chosen = GetChosenCharacter();
		audioSource = GetComponent<AudioSource>();
		audioSource.clip = audioPress;
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
			StartCoroutine(FadeOutAndLoad("Flip_1"));
		}
		if(MGUI.HoveredButton (treeRect,tree))
		{
			audioSource.Play ();
			StartCoroutine(FadeOutAndLoad("Diff_1"));
		}
		
		//Unfinished games
		GUI.enabled = false;
		if(MGUI.HoveredButton (otterRect,otter))
		{
			
		}
		if(MGUI.HoveredButton (hedgehogRect,hedgehog))
		{
			
		}
		
		if(MGUI.HoveredButton (horseRect,horse))
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
	}
	IEnumerator FadeOutAndLoad (string scene)
	{
		AudioSource source = cam.audio;
		while(source.volume > 0.2f || audioSource.isPlaying)
		{
			source.volume -= Time.deltaTime*0.2f;
			yield return null;
		}
		Application.LoadLevel(scene);
	}
}
