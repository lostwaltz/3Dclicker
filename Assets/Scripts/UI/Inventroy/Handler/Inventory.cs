using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<InventorySlot> slotList = new List<InventorySlot>();
    
    public ItemData[] itemData;

    private void Awake()
    {
        UIManager.Instance.CreateUI<UI_Inventory>(PrefabType.UI_Inventory);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
            AddItem(itemData[0]);
        if(Input.GetKeyDown(KeyCode.Q))
            AddItem(itemData[1]);
    }

    public void AddItem(ItemData data)
    {
        slotList.Add(new InventorySlot(data, this));

        UpdateUI();
    }

    public void RemoveItem(InventorySlot slot)
    {
        slotList.Remove(slot);
        
        UpdateUI();
    }
    

    public void UpdateUI()
    {
        UIManager.Instance.GetUI<UI_Inventory>().UpdateUI(slotList);
    }
}
