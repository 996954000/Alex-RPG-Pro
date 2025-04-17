using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SkeletonWalkState : Enemy_SkeletonPatrolState
{
    public Enemy_SkeletonWalkState(Enemy baseEnemy, EnemyStateMeachine stateMeachine, string animBoolName, Enemy_Skeleton _enemy) : base(baseEnemy, stateMeachine, animBoolName, _enemy)
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
        baseEnemy.SetVelocity(enemy_Skeleton.moveSpeed * baseEnemy.faceDir.x, baseEnemy.rigidbody2D.velocity.y);

        if (!baseEnemy.GroundedCheck() || baseEnemy.WallCheck())
        {
            stateMeachine.changeState(enemy_Skeleton.idleState);
            /* 即使转了 idle，仍然会把以下代码执行完毕 */
            baseEnemy.FlipCharacter();
        }
    }
}
