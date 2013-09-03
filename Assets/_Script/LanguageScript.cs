using UnityEngine;
using System.Collections;

public class LanguageScript : MonoBehaviour {
	
	Rect finFlag;
	Rect engFlag;
	public Texture english, finnish;
	// Use this for initialization
	void Awake () {
		int a = PlayerPrefs.GetInt("Language");
		if(a == 0)return;
		if(a != 0)
		{
			Language l = (Language)a;
			GameManager.SetLanguage(l);
			Application.LoadLevel("MainScreenScene");
		}
	}
	void Start()
	{
		float width = Screen.width;
		float height = Screen.height;
		float flagW = width / 3;
		float flagH = height / 3;
		float margin = width / 20;
		finFlag = new Rect (width / 2 - flagW - margin,height / 2 - flagH / 2, flagW, flagH);
		engFlag = new Rect (width / 2 + margin, height / 2 - flagH / 2, flagW, flagH);
	}
	
	void OnGUI()
	{
		if(GUI.Button (finFlag,finnish))
		{
			PlayerPrefs.SetInt("Language",(int)Language.Finnish);
			GameManager.SetLanguage(Language.Finnish);
			Application.LoadLevel("MainScreenScene");
		}
		if(GUI.Button (engFlag,english))
		{
			PlayerPrefs.SetInt("Language",(int)Language.English);
			GameManager.SetLanguage(Language.English);
			Application.LoadLevel("MainScreenScene");
		}
	}
}
