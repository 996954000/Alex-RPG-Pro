using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlideWallState : PlayerState
{
    public PlayerSlideWallState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        player.rigidbody2D.velocity = new Vector2(0.0f, player.rigidbody2D.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.L))
        {
            /* 冲刺状态由父类接手，这里只进行人物方向转换的操作 */
            player.FlipCharacter();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            /* 这里不需要翻转，把翻转操作交给 PlayerState就好了，上面那个是因为没法进 dash 改冲刺方向所以先翻转的 */
            stateMachine.ChangeState(player.wallJumpState);
            /* 不 return 及时打断的话 该状态仍然会把本次 update 中的代码都跑一遍 */
            return;
        }

        /* 把容易触发的条件放在前面，把难触发的，需要调用函数的条件放在后面减少开销 */
        if (isGrounded || xInput == -player.faceDir.x || !player.WallCheck())
            stateMachine.ChangeState(player.idleState);
        if (yInput < 0)
            player.rigidbody2D.velocity = new Vector2(0.0f, player.rigidbody2D.velocity.y * 1.03f);

        player.rigidbody2D.velocity = new Vector2(0.0f, player.rigidbody2D.velocity.y * 0.97f);
    }
}
