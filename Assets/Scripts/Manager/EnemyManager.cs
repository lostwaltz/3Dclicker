using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    private readonly List<Enemy> _enemies = new List<Enemy>();

    public void CreateEnemy(Vector3 position)
    {
        GameObject go = ResourceManager.Instance.Instantiate(PrefabType.Enemy, position);
        _enemies.Add(go.GetComponent<Enemy>());
    }

    public void ReleaseEnemy(Enemy enemy)
    {
        if (0 >= _enemies.Count) return;
        if (true != _enemies.Contains(enemy)) return;
        
        enemy.Release();
        _enemies.Remove(enemy);
    }

    public List<Enemy> GetEnemies()
    {
        return _enemies;
    }
}
