using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        //base.Enter();
        Debug.Log("enter: " + "Air");
        player.animator.SetBool(animBoolName, true);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        //Debug.Log(isGrounded);
        Debug.Log(player.rigidbody2D.velocity.y);

        if (isGrounded)
            stateMachine.ChangeState(player.idleState);

        player.rigidbody2D.velocity = new Vector2(xInput * player.moveSpeed, player.rigidbody2D.velocity.y);

        if (player.WallCheck())
            stateMachine.ChangeState(player.slideWallState);
    }
}
