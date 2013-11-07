using UnityEngine;
using System.Collections;

public class GrabGameManager : GameManager
{
	public GameObject musicObject;
	public AudioClip music;
	public FallingObjectSettings[] fallingObjects;
	public GameObject fallingObjectPrefab;
	public float frequency = 3;
	
	Shader _diffuse;
	float _worldWidth;
	float _worldHeight;
	private float timer;
	
	// Use this for initialization
	public override void Start ()
	{
		base.Start ();
		
		_diffuse = Shader.Find ("Diffuse");
		
		if (InGameMenuGUI.music == null) {
			InGameMenuGUI.music = (GameObject)Instantiate (musicObject);
			InGameMenuGUI.music.audio.clip = music;
			InGameMenuGUI.music.audio.Play ();
			InGameMenuGUI.music.audio.loop = true;
		}
		
		Vector3 __worldSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height,0));
		_worldWidth = __worldSize.x;
		_worldHeight = __worldSize.y;
	}
	
	// Update is called once per frame
	void Update ()
	{
		timer += Time.deltaTime;
		if(timer > frequency) {
			SpawnRandomObject();
			timer = 0;
		}
	}
	
	void SpawnRandomObject()
	{
		InstantiateFallingObject(fallingObjects[Random.Range(0, fallingObjects.Length)]);
	}
	
	void InstantiateFallingObject(FallingObjectSettings settings)
	{
		GameObject __obj = Instantiate(fallingObjectPrefab) as GameObject;
		__obj.GetComponent<FallingObjectScript>().fallingSpeed = Random.Range(settings.minSpeed, settings.maxSpeed);
		
		Material __mat = new Material (_diffuse);
		__mat.mainTexture = settings.texture;
		__obj.renderer.material = __mat;
		
		float __size = __obj.transform.localScale.x;
		__obj.transform.position = new Vector3(Random.Range(__size-_worldWidth, _worldWidth-__size), _worldHeight+__size, 0);
	}
}
