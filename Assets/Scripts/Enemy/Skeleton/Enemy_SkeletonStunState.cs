using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SkeletonStunState : Enemy_SkeletonState
{
    public Enemy_SkeletonStunState(Enemy baseEnemy, EnemyStateMeachine stateMeachine, string animBoolName, Enemy_Skeleton _enemy) : base(baseEnemy, stateMeachine, animBoolName, _enemy)
    {

    }

    public override void Enter()
    {
        enemy_Skeleton.entityFX.InvokeRepeating("StartWhiteToYellowCoroutine", 0, 0.21f);
        enemy_Skeleton.canBeCounterAttackedWindow = false;
        base.Enter();
    }

    public override void Exit()
    {
        enemy_Skeleton.entityFX.CancelInvoke("StartWhiteToYellowCoroutine");
        enemy_Skeleton.beStunned = false;
        base.Exit();
    }

    public override void Update()
    {
        if (stateTimer > enemy_Skeleton.beStunnedDuration)
            stateMeachine.changeState(enemy_Skeleton.battleState);
        base.Update();
    }
}
