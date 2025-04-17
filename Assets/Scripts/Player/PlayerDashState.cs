using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    private float dashDuration;
    public PlayerDashState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        dashDuration = player.dashDuration;

        /* 冲刺时使用 clone skill
         后期修改为完美冲刺时留下残影，并自主攻击
        跟下蛋似的 = = */
        player.skill.skill_clone.UseSkill();
    }

    public override void Exit()
    {
        base.Exit();
        player.dashTempCD = player.dashCD;
    }

    public override void Update()
    {
        base.Update();

        dashDuration -= Time.deltaTime;
        if (SkillManager.instance.skill_dash.CanUseSkill())
        {
            if (dashDuration > 0)
            {
                player.rigidbody2D.velocity = new Vector2(player.faceDir.x * player.dashSpeed, 0.0f);
                /* 冲刺过程中撞墙直接进入 SlideWallState */
                if (player.WallCheck())
                    stateMachine.ChangeState(player.slideWallState);
            }
            else
            {
                if (player.rigidbody2D.velocity.y != 0)
                    stateMachine.ChangeState(player.airState);
                else
                {
                    if (player.rigidbody2D.velocity.x != 0)
                        stateMachine.ChangeState(player.moveState);
                    else
                        stateMachine.ChangeState(player.idleState);
                }
            }
        }
        
    }
}
