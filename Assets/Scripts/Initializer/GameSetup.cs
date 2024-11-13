using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameSetup : MonoBehaviour
{
    [SerializeField] private DungeonGenerator.DungeonGenerator dungeonGenerator;
    [SerializeField] private GameObject monsterPrefab;
    
    private List<RectInt> _dungeonPoints;


    private void Awake()
    {
        dungeonGenerator.Init();
        
        _dungeonPoints = dungeonGenerator.DungeonData;

        foreach (var dungeonRectInt in _dungeonPoints)
        {
            var spawnCount = Random.Range(1, 3);

            for (var i = 0; i < spawnCount; i++)
            {
                Vector2 point = dungeonRectInt.center;
                
                point.x = Random.Range(point.x - (dungeonRectInt.width / 2.2f), point.x + (dungeonRectInt.width / 2.2f));
                point.y = Random.Range(point.y - (dungeonRectInt.height / 2.2f), point.y + (dungeonRectInt.height / 2.2f));
                
                EnemyManager.Instance.CreateEnemy(new Vector3(point.x, 0, point.y));
            }
        }
        
        GameObject go = ResourceManager.Instance.Instantiate(PrefabType.Player);
        go.GetComponent<NavMeshAgent>().Warp(new Vector3(_dungeonPoints[^1].center.x, 0, _dungeonPoints[^1].center.y));
    }
}