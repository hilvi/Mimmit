using UnityEngine;
using System.Collections;

public class MaahalainenScript : MonoBehaviour {

	#region MEMBERS
	private bool _showGUI = false;
	private Rect _guiRect;
	public Inventory inventory;
	public GameObject stonePile;
	public Texture2D itemTexture;
	public Items item;
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
		if(inventory.InventoryContains(item))
		{
			Destroy (stonePile,1.0f);
			return;
		}
		_showGUI = true;
	}
	void OnTriggerExit()
	{
		_showGUI = false;
	}

	void OnGUI()
	{
		if(_showGUI)
		{
			GUI.Box (_guiRect, itemTexture);
		}
	}
	#endregion
}
