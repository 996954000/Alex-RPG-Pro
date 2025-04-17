using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy_SkeletonBattleState : Enemy_SkeletonState
{
    public Enemy_SkeletonBattleState(Enemy baseEnemy, EnemyStateMeachine stateMeachine, string animBoolName, Enemy_Skeleton _enemy) : base(baseEnemy, stateMeachine, animBoolName, _enemy)
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
        /* ÏòÍæ¼ÒÒÆ¶¯ */
        Vector2 moveDirection = new Vector2((playerTrans.position.x - enemy_Skeleton.transform.position.x),
            playerTrans.position.y - enemy_Skeleton.transform.position.y).normalized;
        enemy_Skeleton.rigidbody2D.velocity = new Vector2(moveDirection.x * enemy_Skeleton.moveSpeed * enemy_Skeleton.battleSpeedMultiplier,
            enemy_Skeleton.rigidbody2D.velocity.y);

        baseEnemy.FlipFix();   

        float distance = Vector2.Distance(playerTrans.position, enemy_Skeleton.transform.position);

        if (distance < enemy_Skeleton.attackRange)
        {
            stateMeachine.changeState(enemy_Skeleton.attackState);
        }

        if (distance > enemy_Skeleton.detectionDistance * 2.0f)
            stateMeachine.changeState(enemy_Skeleton.idleState);
    }
}
