using UnityEngine;
using System.Collections;

[System.Flags]
public enum Items{
	None = 0x00,
	PickAxe = 0x01,
	Axe = 0x02,
	Wood = 0x04
}
public class Inventory : MonoBehaviour {

	#region MEMBERS
	Items _items;
	#endregion

	#region UNITY_METHODS
	void Start () 
	{
		_items = Items.None;
	}
	#endregion
	#region METHODS
	/// <summary>
	/// Adds item to the inventory, keeps already existing items
	/// </summary>
	/// <param name="items">Items.</param>
	public void AddToInventory(Items items)
	{
		_items |= items;
	}
	/// <summary>
	///  Clears all items off the inventory
	/// </summary>
	public void ClearInventory()
	{
		_items = Items.None;
	}

	/// <summary>
	/// Sets the inventory with the single item.
	/// </summary>
	/// <param name="item">Item.</param>
	public void SetInventory(Items item)
	{
		_items = item; 
	}

	/// <summary>
	/// Checks if inventory contains the item 
	/// </summary>
	/// <returns><c>true</c>, if inventory contains the item <c>false</c> otherwise.</returns>
	/// <param name="item">Item.</param>
	public bool InventoryContains(Items item)
	{
		if((_items & item) == item)
		{
			return true;
		}
		return false;
	}
	#endregion
}
