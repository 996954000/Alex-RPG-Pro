using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private bool successFlag;
    public PlayerCounterAttackState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        successFlag = false;
        /* 进行 enemy 是否处于 CBCAWindow (CanBeCounterAttackWindow)
         * 反击范围检查为第二段攻击的范围 */
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(player.groundAttackTrans.position, player.groundAttackRadius[1]);
        foreach (var hit in collider2Ds)
        {
            /* 若敌人中存在可被晕眩的单位，则晕眩其，并播放反击成功动画 */
            if (hit.GetComponent<Enemy>() != null && hit.GetComponent<Enemy>().CanBeCounterAttackWindowCheck())
            {
                hit.GetComponent<Enemy>().BeStunned();
                successFlag = true;
            }
        }

        if (successFlag)
            player.animator.SetBool("SuccessCounterAttack", true);
    }

    public override void Exit()
    {
        base.Exit();
        player.animator.SetBool("SuccessCounterAttack", false);
    }

    public override void Update()
    {
        base.Update();
        if (!successFlag && stateTimer > 0.3f)
            stateMachine.ChangeState(player.idleState);
        else
        {
            if (animationTrigger)
                stateMachine.ChangeState(player.idleState);
        }

    }
}
