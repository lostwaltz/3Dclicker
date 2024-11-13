using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : BaseState
{
    protected readonly Player Player;
    
    protected const float CrossFadeDuration = 0.1f;

    protected readonly Animator Animator;

    protected PlayerState(Player player, Animator animator)
    {
        Player = player;
        Animator = animator;
    }
}
