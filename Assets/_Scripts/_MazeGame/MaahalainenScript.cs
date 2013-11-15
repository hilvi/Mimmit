﻿using UnityEngine;
using System.Collections;

public class MaahalainenScript : MonoBehaviour {

	#region MEMBERS
	private bool _showGUI = false;
	private Rect _guiRect;
	#endregion

	#region UNITY_METHODS
	void Start()
	{
		float __width = 960 / 3;
		float __height = 600/ 3;
		_guiRect = new Rect(480 - __width / 2, 300 - __height / 2 , __width, __height);
	}

	void OnTriggerEnter()
	{
		print ("Here");
		_showGUI = true;
	}

	void OnGUI()
	{
		if(_showGUI)
		{
			GUI.Box (_guiRect, "Get the pickaxe");
		}
	}
	#endregion
}
