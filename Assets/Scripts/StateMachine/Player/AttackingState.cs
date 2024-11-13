using UnityEngine;
using UnityEngine.AI;

public class AttackingState : PlayerState
{
    private NavMeshAgent _agent;
    private readonly int _attack = Animator.StringToHash("Attack");

    
    public AttackingState(Player player, Animator animator) : base(player, animator) { }

    
    public override void OnEnter()
    {
       Animator.CrossFade(_attack, CrossFadeDuration);
       _agent = Player.GetComponent<NavMeshAgent>();

       _agent.isStopped = true;
    }

    public override void Update()
    {
        
        
    }

    public override void OnExit()
    {
        _agent.isStopped = false;
    }
}
