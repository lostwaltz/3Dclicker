using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[System.Serializable]
public class ModifiersData
{
    public bool infinity;
    public float duration;
        
    public CharacterStats stats;
}

public class CharacterStatsHandler : MonoBehaviour
{
    [SerializeField] private CharacterStats baseStats;
    public CharacterStats currentStats;
    public List<ModifiersData> statsModifiers = new List<ModifiersData>();

    private void Awake()
    {
        currentStats = new CharacterStats
        {
            maxHealth = baseStats.maxHealth,
            damage = baseStats.damage,
            speed = baseStats.speed,
            health = baseStats.health
            
        };


        UpdateCurrentStats();
    }

    private void Update()
    {
        
    }

    public void AddModifiers(ModifiersData data)
    {
        statsModifiers.Add(data);
        
        UpdateCurrentStats();
    }

    public void RemoveModifiers(ModifiersData data)
    {
        statsModifiers.Remove(data);
        
        UpdateCurrentStats();
    }
    
    
    private void UpdateCurrentStats()
    {
        currentStats.maxHealth = baseStats.maxHealth;
        currentStats.damage = baseStats.damage;
        currentStats.speed = baseStats.speed;
        
        foreach (var data in statsModifiers)
        {
            currentStats.maxHealth += data.stats.maxHealth;
            currentStats.damage += data.stats.damage;
            currentStats.speed += data.stats.speed;
        }
    }
}
