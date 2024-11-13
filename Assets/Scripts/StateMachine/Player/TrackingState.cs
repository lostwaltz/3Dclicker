using UnityEngine;
using UnityEngine.AI;

public class TrackingState : PlayerState
{
    private readonly int _runHash = Animator.StringToHash("Run");
    private EnemyManager _manager;
    private NavMeshAgent _agent;

    public TrackingState(Player player, Animator animator) : base(player, animator)
    {
    }

    public override void OnEnter()
    {
        Animator.CrossFade(_runHash, CrossFadeDuration);
        _manager = EnemyManager.Instance;
        _agent = Player.GetComponent<NavMeshAgent>();
    }

    public override void Update()
    {
        UpdateDestination();
    }

    void UpdateDestination()
    {
        _agent.angularSpeed = 1000f;
        
        _agent.speed = Player.GetComponent<CharacterStatsHandler>().currentStats.speed;
        
        if (0 >= _manager.GetEnemies().Count) return;

        Transform nearestEnemy = null;
        var shortestDistance = Mathf.Infinity;
        Vector3 currentPosition = Player.transform.position;

        foreach (Enemy enemy in _manager.GetEnemies())
        {
            if (enemy == null)
                continue;
            
            var distance = Vector3.Distance(currentPosition, enemy.transform.position);
            
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }

        if (nearestEnemy != null)
        {
            _agent.SetDestination(nearestEnemy.position);
            Debug.DrawLine(currentPosition, nearestEnemy.position, Color.red);
        }
        else
            Debug.LogWarning("모든 적이 null 이어서 목적지를 설정할 수 없습니다.");
    }
}