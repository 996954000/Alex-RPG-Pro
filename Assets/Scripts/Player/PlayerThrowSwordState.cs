using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowSwordState : PlayerState
{
    public PlayerThrowSwordState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        
        if (animationTrigger)
        {
            if (player.skill.skill_throwSword.CanUseSkill())
                player.skill.skill_throwSword.UseSkill();
            stateMachine.ChangeState(player.idleState);
        }
    }
}
