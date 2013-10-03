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
		
		//mapRect = new Rect(500,200,300,250);
		float _edge = 100;
		otterRect = new Rect(40,330,_edge,_edge);//400, 350
		hedgehogRect = new Rect(200,330,_edge,_edge);//385 330
		treeRect = new Rect(120, 240,_edge,_edge);// 285 250
		horseRect = new Rect(300,240,_edge,_edge); // 285 250
		owlRect = new Rect(210, 160,_edge,_edge);// 185 150
		dragonRect = new Rect(325, 105,_edge,_edge);
		
		float bearX = 450;
		bearRect = new Rect(bearX, 75,_edge,_edge);
		granmaRect = new Rect(bearX + 1.3f * _edge,50,_edge,_edge);
		seaDragonRect = new Rect(bearX + 2.6f *_edge ,50,_edge,_edge);
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
		if(MGUI.HoveredButton (otterRect,otter))
		{
			
		}
		if(MGUI.HoveredButton (hedgehogRect,hedgehog))
		{
			
		}
		if(MGUI.HoveredButton (treeRect,tree))
		{
			
		}
		if(MGUI.HoveredButton (horseRect,horse))
		{
			
		}
		if(MGUI.HoveredButton (owlRect,owl))
		{
			audioSource.Play ();
			StartCoroutine(FadeOutAndLoad("Flip_1"));
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
