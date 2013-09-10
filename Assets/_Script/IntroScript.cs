using UnityEngine;
using System.Collections;
using System;

public class IntroScript : MonoBehaviour {
	
	public float speedSliding = 2;
	public float soundSpeed = 1f;
	public float speedAnim = 1;
	public Texture2D blonde, brune,banner;
	public SoundManager soundManager;
	
	float uv;
	bool front = true, characterSelection;
	bool coroutineRunning = false;
	
	float speedMovement;
	Rect bannerRect;
	Rect buttonPlay, buttonQuit, blondeRect, bruneRect,blTex, brTex, buttonBl, buttonBr;
	Rect chooseBlRect,chooseBrRect, backButton;
#if UNITY_STANDALONE
	bool quitting = false;
	Rect quitWindow, yesButton, noButton;
#endif
	
	GUIStyle noStyle = new GUIStyle();
	Action<string>_del;
	
	void Start () {
		speedMovement = Screen.width;
		float width = 	Screen.width;
		float height = 	Screen.height;
		float ratio = Screen.width / Screen.height;
		
#if UNITY_WEBPLAYER	
		float measureW = width / 22;
		float xPosLeftSide = measureW * 2f;
		float xPosBr = 	measureW * 16f;
		float measureH = height / 20;
		float yPos = 	measureH * 10;
		float girlW = 	measureW * 4;
		float girlH = 	measureH * 10;
		
		bannerRect = new Rect(xPosLeftSide, 0, measureW * 18, measureW * 6 * ratio );

		blondeRect = new Rect(xPosLeftSide,yPos,girlW,girlH );
		bruneRect = new Rect(xPosBr,yPos,girlW,girlH );
		
		
		buttonPlay = new Rect(measureW * 7, yPos, measureW * 8, girlH / 2);
		buttonQuit =  new Rect(measureW * 7,yPos + girlH / 2,  measureW * 8, girlH / 2 );
		
		blTex = new Rect(0.66666f, 0 ,-0.3333f , 1f);
		buttonBl = blTex;	
		brTex = new Rect(0.33333f ,0, 0.3333f,1f);
		buttonBr = brTex;
		
		float xPosCharacterA = measureW * 7 + width;
		chooseBlRect = new Rect(xPosCharacterA,yPos, girlW, girlH);
		chooseBrRect = new Rect(xPosCharacterA + girlW,yPos, girlW, girlH);
		backButton = new Rect(width, yPos, girlW, girlH / 2);
#endif
#if UNITY_STANDALONE		
		float measureW = width / 22;
		float xPosLeftSide = measureW * 2f;
		float xPosBr = 	measureW * 16f;
		float measureH = height / 20;
		float yPos = 	measureH * 8;
		float girlW = 	measureW * 4;
		float girlH = 	measureH * 10;
		
		bannerRect = new Rect(xPosLeftSide, 0, measureW * 18, measureW * 6 * ratio );

		blondeRect = new Rect(xPosLeftSide,yPos,girlW,girlH );
		bruneRect = new Rect(xPosBr,yPos,girlW,girlH );
		
		
		buttonPlay = new Rect(measureW * 7, yPos, measureW * 8, girlH / 2);
		buttonQuit =  new Rect(measureW * 7,yPos + girlH / 2,  measureW * 8, girlH / 2 );
		
		blTex = new Rect(0.66666f, 0 ,-0.3333f , 1f);
		buttonBl = blTex;	
		brTex = new Rect(0.33333f ,0, 0.3333f,1f);
		buttonBr = brTex;
		
		float xPosCharacterA = measureW * 7 + width;
		chooseBlRect = new Rect(xPosCharacterA,yPos, girlW, girlH);
		chooseBrRect = new Rect(xPosCharacterA + girlW,yPos, girlW, girlH);
		backButton = new Rect(width, yPos, girlW, girlH / 2);
		
		float sizeBoxW = width / 3;
		float sizeBoxH = height / 3;
		quitWindow = new Rect(width / 3 , yPos,sizeBoxW, sizeBoxH);
		yesButton = new Rect(width / 3, yPos + sizeBoxH / 2, sizeBoxW / 2, sizeBoxH / 2 );
		noButton = new Rect(width / 3 + sizeBoxW / 2, yPos + sizeBoxH / 2, sizeBoxW / 2, sizeBoxH / 2 );
#endif
		_del = soundManager.PlayAudio;
		StartCoroutine(Wait(1.0f,_del,"Hello"));
	}
	
	void OnGUI () {
		GUI.Box(bannerRect,banner,noStyle);
		if(front)
		{
			GUI.DrawTextureWithTexCoords(blondeRect,blonde,blTex);
			GUI.DrawTextureWithTexCoords(bruneRect,brune,brTex);
			if(GUI.Button (buttonPlay,"Play"))
			{
				if(!coroutineRunning)
				{
					StartCoroutine(ChangeScreen (-1));
					characterSelection = true;
				}
			}
			#if UNITY_STANDALONE
			if(GUI.Button(buttonQuit,"Quit"))
			{
				quitting = true;
				front = false;
			}
			
			#endif
		}
		#if UNITY_STANDALONE
		if(quitting)
		{
			GUI.Box(quitWindow,"Really");
			if(GUI.Button (yesButton,"YES"))
			{
				if(!coroutineRunning){
					StartCoroutine (Leaving());
				}
			}
			if(GUI.Button (noButton, "NO"))
			{
				quitting = false;
				front = true;
			}
		}
		#endif
		if(characterSelection)
		{
			GUI.DrawTextureWithTexCoords(chooseBlRect,blonde,buttonBl);
			GUI.DrawTextureWithTexCoords(chooseBrRect,brune, buttonBr);
			if(GUI.Button(chooseBlRect,"",noStyle))
			{
				print ("Blonde");
				Manager.SetCharacter(Character.Blonde);
				StartCoroutine(LaunchGame());
			}
			if(GUI.Button(chooseBrRect,"",noStyle))
			{
				print ("Brune");
				Manager.SetCharacter(Character.Brune);
				StartCoroutine(LaunchGame());
			}
			if(GUI.Button (backButton,"Back"))
			{
				if(!coroutineRunning)
				{
					StartCoroutine(ChangeScreen(1));
					front = true;
				}
			}
		}
	}
	
	IEnumerator ChangeScreen(float sign)
	{
		coroutineRunning = true;
		
		float movement = 0;
		float width = Screen.width;
		float blOrigin = blondeRect.x;
		float brOrigin = bruneRect.x;
		float playOrigin = buttonPlay.x;
		float quitOrigin = 	buttonQuit.x;
		float charAOrigin =	chooseBlRect.x;
		float charBOrigin = chooseBrRect.x;
		float backOrigin = backButton.x;
		float frame = 0;
		
		soundManager.PlayAudio("Sweep");
		
		
		while(Mathf.Abs(movement) < width)
		{
			
			if(sign < 0){
				frame+=Time.deltaTime*speedAnim;
				float numerator = (int)frame % 3;
			
				brTex = new Rect(numerator / 3 ,0, 0.3333f,1f);
				blTex = new Rect(numerator / 3 ,0, 0.3333f,1f);
			}else{
				frame-=Time.deltaTime*speedAnim;
				float numerator = (int)frame % 3;
				
				brTex = new Rect(numerator / 3 + 1 ,0, -0.3333f,1f);
				blTex = new Rect(numerator / 3 + 1 ,0, -0.3333f,1f);
			}
			
			blondeRect.x = blOrigin + movement;
			bruneRect.x = brOrigin + movement;
			buttonPlay.x = playOrigin + movement;
			buttonQuit.x = quitOrigin + movement;
			chooseBlRect.x = charAOrigin + movement;
			chooseBrRect.x = charBOrigin + movement;
			backButton.x = backOrigin + movement;
			movement += Time.deltaTime * speedMovement * speedSliding * sign;
			
			yield return null;
		}
		if(sign > 0)
		{
			characterSelection = false;
			//soundManager.PlayAudio("Hello");
			blTex = buttonBl;
			brTex = buttonBr;
		}else{
			front = false;
			soundManager.PlayAudio("Choose");
		}
		coroutineRunning = false;
	}
	
	IEnumerator LaunchGame()
	{
		coroutineRunning = true;
		float timer = 0;
		soundManager.PlayAudio("Play");
		while(timer < soundManager.GetLength("Play"))
		{
			timer += Time.deltaTime;
			yield return null;
		}
		while(audio.volume > 0.2f)
		{
			audio.volume -= Time.deltaTime * soundSpeed;
			yield return null;
		}
		coroutineRunning = false;
		Application.LoadLevel("MapWorld");
	}
	
	IEnumerator Leaving()
	{
		coroutineRunning = true;
		float timer = 0;
		soundManager.PlayAudio("Bye");
		while(timer < soundManager.GetLength("Bye"))
		{
			timer += Time.deltaTime;
			yield return null;
		}
		coroutineRunning = false;
		Application.Quit();
	}
	
	IEnumerator Wait(float period, Action<string> action, string str)
	{
		float timer = 0;
		while(timer < period)
		{
			timer += Time.deltaTime;
			yield return null;
		}
		if(action != null)
		{
			action(str);
		}
	}
}
