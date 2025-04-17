using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    private float wallJumpTimer;
    public PlayerWallJumpState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.rigidbody2D.AddForce(new Vector2(-player.faceDir.x * player.jumpForce * 0.6f,
            player.jumpForce));
        Debug.Log("Wall Jump");

        wallJumpTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        //Debug.Log("YVelocity" + player.rigidbody2D.velocity.y);

        wallJumpTimer -= Time.deltaTime;

        /* nnd，因为进来力给完后，速度在第一帧时还没有提上来到正数，导致一进来就去 airState了
         * 焯，真的抽象*/
        if (player.rigidbody2D.velocity.y < 0 && wallJumpTimer < 0)
            stateMachine.ChangeState(player.airState);
    }
}
