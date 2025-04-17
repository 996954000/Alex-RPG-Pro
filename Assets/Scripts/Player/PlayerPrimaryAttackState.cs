using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private float attackWindow;
    private int attackPhase;

    private float attackTime;
    private float lastAttackTime;

    private bool continueAttack;
    private float stateTime;
    public PlayerPrimaryAttackState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
        attackWindow = player.groundAttackWindow;
        attackPhase = 0;
        lastAttackTime = -player.groundAttackWindow + 0.5f;
    }

    public override void Enter()
    {
        base.Enter();

        attackTime = Time.time;
        continueAttack = false;
        stateTime = .1f;

        player.animator.SetInteger("GroundAttackPhase", attackPhase);

        /* 攻击时向面朝方向位移 还可以小跳增加力量感 */
        player.rigidbody2D.velocity = new Vector2(player.attackMovement[attackPhase].x * player.moveSpeed * player.faceDir.x,
            player.attackMovement[attackPhase].y);

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        /* 开始攻击 0.1s 内会随着惯性移动 */
        if (stateTime < 0)
            player.rigidbody2D.velocity = Vector3.zero;

        stateTime -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.J))
            continueAttack = true;

        if (animationTrigger)
        {
            if (attackTime - lastAttackTime < attackWindow)
            {
                attackPhase += 1;
                attackPhase %= 3;
            }
            else
                attackPhase = 0;

            lastAttackTime = Time.time;

            if (continueAttack && attackPhase != 0)
                stateMachine.ChangeState(player.primaryAttackState);
            else
                stateMachine.ChangeState(player.idleState);
        }
    }

    public int GetGroundAttackPhase() => attackPhase;
}
