using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SkeletonAttackState : Enemy_SkeletonState
{
    public Enemy_SkeletonAttackState(Enemy baseEnemy, EnemyStateMeachine stateMeachine, string animBoolName, Enemy_Skeleton _enemy) : base(baseEnemy, stateMeachine, animBoolName, _enemy)
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
        enemy_Skeleton.rigidbody2D.velocity = Vector3.zero;
        //Debug.Log("attack:" + enemy_Skeleton.rigidbody2D.velocity);
        if (Vector2.Distance(playerTrans.position, enemy_Skeleton.transform.position) > enemy_Skeleton.attackRange
            && animationTrigger)
            stateMeachine.changeState(enemy_Skeleton.battleState);
    }
}
