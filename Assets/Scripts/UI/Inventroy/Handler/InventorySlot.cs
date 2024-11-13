

using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InventorySlot
{
    public InventorySlot(ItemData itemData, Inventory inventory)
    {
        data = itemData;
        _inventory = inventory;
    }
    
    public ItemData data;
    private Inventory _inventory; 

    public void UseItem()
    {
        data?.UseItem();
        _inventory.RemoveItem(this);
    }
}
