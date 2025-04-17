using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
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
        /* 跳跃 */
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.jumpState);
        }

        /* 空中状态 */
        if (player.rigidbody2D.velocity.y < 0 && !isGrounded)
            stateMachine.ChangeState(player.airState);

        /* 攻击 */
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            stateMachine.ChangeState(player.primaryAttackState);
        }

        /* 反击 */
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            stateMachine.ChangeState(player.counterAttackState);
        }

        /* 瞄准 */
        /* 
           添加标志位表明是否有剑可掷或者有剑可收 */
        /* 进行了 CD 和 是否有剑的检查 */
        if (Input.GetKeyDown(KeyCode.Z) && player.skill.skill_throwSword.CanUseSkill())
        {
            if (player.flyingSword == null)
                stateMachine.ChangeState(player.aimState);
            else
            {
                player.skill.skill_throwSword.UseSkill();
            }
        }
        if (player.catchSword)
            stateMachine.ChangeState(player.catchSwordState);
    }
}
