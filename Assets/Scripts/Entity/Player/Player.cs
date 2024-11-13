using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Player : Entity
{
    [SerializeField] private Animator animator;
    
    private StateMachine _state;
    private EnemyManager _enemyManager;
    private PlayerEnemyChecker _playerChecker;

    public CharacterStatsHandler Stats { get; private set; }
    
    private void Awake()
    {
        InitState();
        
        PlayerManager.Instance.Player = this;
        _enemyManager = EnemyManager.Instance;
        
        Stats = GetComponent<CharacterStatsHandler>();
    }

    private void Start()
    {
        _playerChecker = TryGetComponent<PlayerEnemyChecker>(out PlayerEnemyChecker checker) ? checker : gameObject.AddComponent<PlayerEnemyChecker>(); 
    }

    private void InitState()
    {
        _state = new StateMachine();
        
        var idle = new IdleState(this, animator);
        var tracking = new TrackingState(this, animator);
        var attackingState = new AttackingState(this, animator);
        
        _state.AddTransition(idle, tracking, new FuncPredicate(() => 0 != _enemyManager.GetEnemies().Count ));
        
        _state.AddTransition(tracking, attackingState, new FuncPredicate(() => _playerChecker.IsHit ));
        
        _state.AddTransition(attackingState, idle, new FuncPredicate(() => !_playerChecker.IsHit ));
        
        _state.AddTransition(tracking, idle, new FuncPredicate(() => 0 == _enemyManager.GetEnemies().Count ));
        
        _state.SetState((idle));
    }
    private void FixedUpdate()
    {
        _state.FixedUpdate();
    }
    private void Update()
    {
        _state.Update();
    }

    public override void Release()
    {
    }
}
