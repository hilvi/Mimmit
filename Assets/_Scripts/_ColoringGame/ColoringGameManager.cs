﻿//#define DEVELOPER_MODE

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColoringGameManager : GameManager {
	
	#region PUBLIC
	//public Rect chosenCharRegion;		// 20,20,160,160
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
	public Texture2D tickTexture;
	public Texture2D upArrowTexture;
	public Texture2D downArrowTexture;
	public Texture2D resetTexture;
	public Texture2D saveTexture;
	public AudioClip clickColor;
	public AudioClip clickErase;
	public AudioClip clickButton;
	public Texture2D cursor;
	#endregion
	
	#region PRIVATE
	private PictureSelector _pictureSelector;
	private PaintFrame _frame;
	private PaintToolbar _toolbar;

	private CharacterWidgetScript _characterWidget;
	
	private int _currentPictureIndex = 0;
	private Vector2 _hotSpot = Vector2.zero;
	private CursorMode _cursorMode = CursorMode.Auto;
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
		
		_pictureSelector = new PictureSelector(this, pictureSelectRegion, cachedPictures,
			upArrowTexture, downArrowTexture);
		
		_toolbar = new PaintToolbar(this, toolbarRegion, 
			new Vector2(740f,180f), new Vector2(5f, 5f),
			paintBrushTextures, eraserTexture, 
			tickTexture, resetTexture,
			saveTexture);
		
		_characterWidget = GameObject.Find("CharacterWidget").GetComponent<CharacterWidgetScript>();
		
		_frame = new PaintFrame(this, pictureRegion, cachedPictures[0]);
	}
	
	void Update () 
	{
		if (GetGameState() != GameState.Running)
			return;
		
		if (Input.GetMouseButtonDown(0)) 
		{
			print (Input.mousePosition);
			_HandleMouseClick();
			_characterWidget.TriggerHappyEmotion();
		}
		if(pictureSelectRegion.Contains(InputManager.MouseScreenToGUI()))
		{
			float __mouse = Input.GetAxis("Mouse ScrollWheel");
			if(__mouse > 0)
			{
				_pictureSelector.HandleMouseWheel(1);
			}
			else if(__mouse < 0)
			{
				_pictureSelector.HandleMouseWheel(-1);
			}
		}

	}
	
	void OnGUI () 
	{
		_pictureSelector.OnGUI();
		_frame.OnGUI();
		_toolbar.OnGUI();

		if(pictureRegion.Contains(Event.current.mousePosition))
		{
			Cursor.SetCursor(null,_hotSpot,_cursorMode);
		}
		else
		{
			Cursor.SetCursor(cursor,_hotSpot,_cursorMode);
		}
	}
	#endregion
	
	#region METHODS
	public void ResetPictureToOriginal() 
	{
		#if DEVELOPER_MODE
		_frame.VolatilePicture = _CreateDebugGridTexture(560, 560, 40, 40);
		#else
		_frame.VolatilePicture = cachedPictures[_currentPictureIndex];
		#endif
	}
	
	public Texture2D GetPictureFromFrame() 
	{
		return _frame.VolatilePicture;
	}
	
	public void LoadPictureByIndex(int index) 
	{
		Debug.Log("Loading picture index: " + index.ToString());
		
		_frame.VolatilePicture = cachedPictures[index];
		_currentPictureIndex = index;
	}
	
	private void _HandleMouseClick() {
		Vector3 __p = InputManager.MouseScreenToGUI();

		// Click on the preview section
		if (pictureSelectRegion.Contains(__p)) 
		{
			audio.clip = clickButton;
			audio.volume = 0.5f;
			audio.Play ();
			_pictureSelector.HandleMouse(__p);
		} 
		// Click on the drawing
		else if (pictureRegion.Contains(__p)) 
		{
			if(_toolbar.CurrentBrush.eraser)
			{
				audio.clip = clickErase;
			}
			else
			{
				audio.clip = clickColor;
			}
			audio.volume = 0.5f;
			audio.Play ();		
			_frame.Paint(__p, _toolbar.CurrentBrush.color);
		} 
		// Click the toolbar
		else if (toolbarRegion.Contains(__p)) 
		{
			audio.clip = clickButton;
			audio.volume = 0.5f;
			audio.Play ();
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
