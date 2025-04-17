using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SkeletonPatrolState : Enemy_SkeletonState
{
    /* 巡逻状态，是 Walk 和 Idle 的父类，在这两个状态下可转为警觉追击状态 */
    public Enemy_SkeletonPatrolState(Enemy baseEnemy, EnemyStateMeachine stateMeachine, string animBoolName, Enemy_Skeleton _enemy) : base(baseEnemy, stateMeachine, animBoolName, _enemy)
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
        if (enemy_Skeleton.PlayerDetection())
            stateMeachine.changeState(enemy_Skeleton.reactState);
    }
}
