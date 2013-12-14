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
    Rect _invRectA, _invRectB;
    Texture _invTextureA, _invTextureB;
    float _inYPos;
    float _outYPos;
    int _itemAmount = 0;
	#endregion

	#region UNITY_METHODS
	void Start () 
	{
		_items = Items.None;
        float __sizeButton = Screen.width / 15;
        float __margin = 10f;
        _inYPos = __margin;
        _outYPos = -(__sizeButton + __margin);
        _invRectA = new Rect(Screen.width - __sizeButton - __margin, _outYPos,__sizeButton,__sizeButton);
        _invRectB = new Rect(Screen.width - 2 * (__sizeButton + __margin), _outYPos, __sizeButton, __sizeButton);
	}

    void OnGUI() 
    {
        if (_itemAmount == 1)
        {
            _invRectA.y = Mathf.MoveTowards(_invRectA.y,_inYPos,0.2f*Time.deltaTime);
            GUI.Box(_invRectA,_invTextureA);
        }
        if (_itemAmount == 2)
        {
            float __ratio = 0.2f * Time.deltaTime;
            _invRectA.y = Mathf.MoveTowards(_invRectA.y, _inYPos, __ratio);
            GUI.Box(_invRectA, _invTextureA);
            _invRectB.y = Mathf.MoveTowards(_invRectB.y, _inYPos, __ratio);
            GUI.Box(_invRectB, _invTextureB);
        }
    }
	#endregion
	#region METHODS
	/// <summary>
	/// Adds item to the inventory, keeps already existing items
	/// </summary>
	/// <param name="items">Items.</param>
	public void AddToInventory(Items items, Texture texture)
	{
        _itemAmount++;
        if (_itemAmount == 1) _invTextureA = texture;
        else _invTextureB = texture;
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
