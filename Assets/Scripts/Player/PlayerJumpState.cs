using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Jump");
        /* 起跳时施加力，因为相比于非状态机控制时只会施加 1 帧的力，所以 JumpForce 要比以前大许多才行 */
        player.rigidbody2D.AddForce(new Vector2(player.rigidbody2D.velocity.x,
            player.jumpForce));
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (player.rigidbody2D.velocity.y < 0)
        {
            stateMachine.ChangeState(player.airState);
            return;
        }

        player.rigidbody2D.velocity = new Vector2(xInput * player.moveSpeed, player.rigidbody2D.velocity.y);
    }
}
