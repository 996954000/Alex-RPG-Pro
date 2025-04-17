using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SkeletonUnderAttackState : Enemy_SkeletonState
{
    public Enemy_SkeletonUnderAttackState(Enemy baseEnemy, EnemyStateMeachine stateMeachine, string animBoolName, Enemy_Skeleton _enemy) : base(baseEnemy, stateMeachine, animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (enemy_Skeleton.underAttackPhase == 2)
            enemy_Skeleton.rigidbody2D.AddForce(new Vector2(-enemy_Skeleton.faceDir.x * enemy_Skeleton.knockbackForce.x * 2, enemy_Skeleton.knockbackForce.y));
        else if (enemy_Skeleton.underAttackPhase == 0)
            // 0 号阶段是微小攻击，击退距离小
            enemy_Skeleton.rigidbody2D.AddForce(new Vector2(-enemy_Skeleton.faceDir.x * enemy_Skeleton.knockbackForce.x * 0.5f, enemy_Skeleton.knockbackForce.y));
        else
            enemy_Skeleton.rigidbody2D.AddForce(new Vector2(-enemy_Skeleton.faceDir.x * enemy_Skeleton.knockbackForce.x, enemy_Skeleton.knockbackForce.y));
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        /* 任何状态下都可进入受击状态，受击后直接进入 battle 状态 */
        if (animationTrigger && !enemy_Skeleton.underAttack)
            stateMeachine.changeState(enemy_Skeleton.battleState);
        
        base.Update();
    }
}
