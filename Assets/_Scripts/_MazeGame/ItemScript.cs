using UnityEngine;
using System.Collections;

public class ItemScript : MonoBehaviour {

	public Inventory inventory;  
	public Items item;
	
	void OnTriggerEnter()
	{
		inventory.AddToInventory(item);
		Destroy (gameObject);
	}
}
