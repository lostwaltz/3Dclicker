using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_InventorySlot : MonoBehaviour
{
    [SerializeField] private Button useButton;
    
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemDesc;

    public UI_Inventory uiInventory;
    
    private void Awake()
    {
        AddListener(OnDestroy);
    }

    public void AddListener(UnityAction callback)
    {
        useButton.onClick.RemoveListener(callback);
        useButton.onClick.AddListener(callback);
    }

    public void UpdateUI(InventorySlot slot)
    {
        icon.sprite = slot.data.icon;
        itemName.text = slot.data.itemName;
        itemDesc.text = slot.data.itemDescription;
    }

    private void OnDestroy()
    {
        uiInventory.RemoveUI(this);
        Destroy(gameObject);
    }
}
