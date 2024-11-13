using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class IdleState : PlayerState
{
    private readonly int _idle = Animator.StringToHash("Idle");
    
    private readonly EnemyManager _manager = EnemyManager.Instance;
    
    public IdleState(Player player,  Animator animator) : base(player, animator) { }

    public  override void OnEnter()
    {
        Animator.CrossFade(_idle, CrossFadeDuration);
    }

    public override void FixedUpdate()
    {

    }

    public override void Update()
    {

    }
}