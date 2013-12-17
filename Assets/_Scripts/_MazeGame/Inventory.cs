using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Flags]
public enum Items{
	None = 0x00,
	PickAxe = 0x01,
	Axe = 0x02,
	Wood = 0x04
}
public class Inventory : MonoBehaviour {

	#region MEMBERS
    Rect _invRectA, _invRectB;
    Texture _invTextureA, _invTextureB;
    float _inYPos;
    float _outYPos;
    float _xPos,_xOffset, _sizeButton;
    List<InventoryItem> inventoryItem = new List<InventoryItem>();
    public class InventoryItem
    {
        public Items item;
        public Texture texture;
        public Rect rect = new Rect();

        public InventoryItem(Items items, Texture texture, Rect rect)
        {
            this.item = items;
            this.texture = texture;
            this.rect = rect;
        }
    }
	#endregion

	#region UNITY_METHODS
	void Start () 
	{
        _sizeButton = Screen.width / 15;
        float __margin = 10f;
        _inYPos = __margin;
        _outYPos = -(_sizeButton + __margin);
        _xPos = Screen.width - _sizeButton - __margin;
        _xOffset = _sizeButton + __margin;
	}

    void OnGUI() 
    {
        for (int i = 0; i < inventoryItem.Count; i++) 
        {
            GUI.Box(inventoryItem[i].rect, inventoryItem[i].texture);
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
        float __x = _xPos - inventoryItem.Count * _xOffset;
        Rect __rect = new Rect(__x, _outYPos,_sizeButton,_sizeButton);
        inventoryItem.Add( new InventoryItem(items, texture,__rect));
        StartCoroutine(_MoveItemDown(inventoryItem[inventoryItem.Count-1]));
	}


	/// <summary>
	/// Checks if inventory contains the item 
	/// </summary>
	/// <returns><c>true</c>, if inventory contains the item <c>false</c> otherwise.</returns>
	/// <param name="item">Item.</param>
	public bool InventoryContains(Items item)
	{
        for (int i = 0; i < inventoryItem.Count; i++) 
        {
            if (inventoryItem[i].item == item) return true;
        }
        return false;
	}
	#endregion

    public void RemoveItem(Items item)
    {
        int i = 0;
        for (; i < inventoryItem.Count; i++)
        {
            if (inventoryItem[i].item == item) break;
        }
        InventoryItem __item = inventoryItem[i];
        inventoryItem.RemoveAt(i);
        __item = null;
        StartCoroutine(_RemoveItem(i));
    }
    IEnumerator _RemoveItem(int i)
    {
        float __movement = 0;
        float __sliding = 100f * Time.deltaTime;
        float __fullMovement = 0f;
        while (__fullMovement < _xOffset )
        {
            int j = i;
            __movement = Mathf.MoveTowards(__movement, _xOffset, __sliding);
            __fullMovement += __movement;
            for (; j < inventoryItem.Count; j++)
            {        
                inventoryItem[j].rect.x += __movement;
            }
            yield return null;
        }
    }
    IEnumerator _MoveItemDown(InventoryItem item)
    {
        float __sliding = 100f;
        while (item.rect.y != _inYPos)
        {
            item.rect.y = Mathf.MoveTowards(item.rect.y, _inYPos, __sliding * Time.deltaTime);
            yield return null;
        }
    }
}

