using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemStatModifiers", menuName = "ItemStatModifiers")]
public class ItemStatModifiers : ItemData
{
    [SerializeField] private List<ModifiersData> modifiers;
    
    public override void UseItem()
    {
        foreach (var stat in modifiers)
        {
            PlayerManager.Instance.Player.Stats.AddModifiers(stat);
        }
    }
}