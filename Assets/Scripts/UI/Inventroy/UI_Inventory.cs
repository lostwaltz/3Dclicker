using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : UI_Base
{
    [SerializeField] private UI_InventorySlot inventorySlotPrefab;
    [SerializeField] private Transform content;
    
    private readonly List<UI_InventorySlot> _slotList = new List<UI_InventorySlot>();
    

    public void UpdateUI(List<InventorySlot> dataList)
    {
        for (var i = 0; i < dataList.Count; i++)
        {
            if (_slotList.Count <= i)
            {
                UI_InventorySlot slot = Instantiate(inventorySlotPrefab, content.transform);
                _slotList.Add(slot);
                slot.uiInventory = this;
            }

            _slotList[i].AddListener(dataList[i].UseItem);
            
            _slotList[i].UpdateUI(dataList[i]); 
        }
    }

    public void RemoveUI(UI_InventorySlot slot)
    {
        _slotList.Remove(slot);
    }
    
    public override void Init()
    {
    }

    public override void Release()
    {
    }
}
