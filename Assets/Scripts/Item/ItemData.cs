using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    public Sprite icon;
    public string itemName;
    public string itemDescription;

    public abstract void UseItem();
}