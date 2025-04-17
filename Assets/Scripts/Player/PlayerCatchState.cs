using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class PlayerCatchState : PlayerState
{
    public PlayerCatchState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        /* 本来准备写在这里的转身接剑逻辑为了避免 sword 已经被 destroy 无法获取位置，所以写在 destroy 的前面了 */
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
            stateMachine.ChangeState(player.idleState);
            player.catchSword = false;
        }
    }
}
