//#define DEVELOPER_MODE

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColoringGameManager : GameManager {
	
	#region PUBLIC
	public Rect chosenCharRegion;		// 20,20,160,160
	public Rect pictureSelectRegion;	// 20,200,160,380
	public Rect pictureRegion;			// 200,20,560,560
	public Rect toolbarRegion;			// 780,100,160,480
	
	public GameObject musicObject;
	public AudioClip music;
	
	/*
	 * These pictures will be read/write unlocked, so we always 
	 * have to make a copy before modifying any of them.
	 */ 
	public Texture2D[] cachedPictures; 
	public Texture2D[] paintBrushTextures;
	public Texture2D eraserTexture;
	#endregion
	
	#region PRIVATE
	private PictureSelector _pictureSelector;
	private PaintFrame _frame;
	private PaintToolbar _toolbar;

	private CharacterWidgetScript _characterWidget;
	#endregion

	#region UNITY_METHODS
	public override void Start () 
	{
		base.Start();
		SetGameState(GameState.Running);
		
		if(InGameMenuGUI.music == null)
		{
		  	InGameMenuGUI.music = (GameObject)Instantiate(musicObject);
			InGameMenuGUI.music.audio.clip = music;
			InGameMenuGUI.music.audio.Play();
		}
		
		_pictureSelector = new PictureSelector(this, pictureSelectRegion);
		_toolbar = new PaintToolbar(this, toolbarRegion, 
			new Vector2(800f, 325f), new Vector2(10f, 10f),
			paintBrushTextures, eraserTexture);
		
		_characterWidget = GameObject.Find("CharacterWidget").GetComponent<CharacterWidgetScript>();
		
		#if DEVELOPER_MODE
		pictureRegion = new Rect(200, 20, 560, 560);
		_frame = new PaintFrame(this, pictureRegion, _CreateDebugGridTexture(560, 560, 40, 40));
		#else
		_frame = new PaintFrame(this, pictureRegion, cachedPictures[0]);
		#endif
	}
	
	void Update () 
	{
		if (Input.GetMouseButtonDown(0)) 
		{
			_HandleMouseClick();
			_characterWidget.TriggerHappyEmotion();
		}
	}
	
	void OnGUI () 
	{
		#if UNITY_EDITOR
		GUI.Box(chosenCharRegion, "chosenChar");
		#endif
		
		_pictureSelector.OnGui();
		_frame.OnGUI();
		_toolbar.OnGUI();
	}
	#endregion
	
	#region METHODS
	public void ResetPictureToOriginal() 
	{
		#if DEVELOPER_MODE
		_frame.VolatilePicture = _CreateDebugGridTexture(560, 560, 40, 40);
		#else
		_frame.VolatilePicture = cachedPictures[0];
		#endif
	}
	
	public Texture2D GetPictureFromFrame() 
	{
		return _frame.VolatilePicture;
	}
	
	public void LoadPictureByIndex(int index) {
		Debug.Log("Loading picture index: " + index.ToString());
		
		_frame.VolatilePicture = cachedPictures[index];
	}
	
	private void _HandleMouseClick() {
		Vector2 __p = Input.mousePosition;
		__p.y = Screen.height - __p.y; // y-axis flips
		
		if (chosenCharRegion.Contains(__p)) {
			_HandleChosenCharClick(__p);
		} else if (pictureSelectRegion.Contains(__p)) {
			_pictureSelector.HandleMouse(__p);
		} else if (pictureRegion.Contains(__p)) {
			_frame.Paint(__p, _toolbar.CurrentBrush.color);
		} else if (toolbarRegion.Contains(__p)) {
			_toolbar.HandleMouse(__p);
		}
	}
	
	private void _HandleChosenCharClick(Vector2 position) {
		//TODO
		Debug.Log("clicked on chosen char");
	}

	private Texture2D _CreateDebugGridTexture(int width, int height, int gridWidth, int gridHeight) 
	{
		Texture2D __picture = new Texture2D(width, height);
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (x % gridWidth == 0 || y % gridHeight == 0)
					__picture.SetPixel(x, y, Color.black);
				else
					__picture.SetPixel(x, y, Color.white);
			}
		}
		__picture.Apply();
		return __picture;
	}
	#endregion
}
