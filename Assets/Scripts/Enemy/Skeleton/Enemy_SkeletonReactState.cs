using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SkeletonReactState : Enemy_SkeletonState
{
    public Enemy_SkeletonReactState(Enemy baseEnemy, EnemyStateMeachine stateMeachine, string animBoolName, Enemy_Skeleton _enemy) : base(baseEnemy, stateMeachine, animBoolName, _enemy)
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
        if (animationTrigger)
            stateMeachine.changeState(enemy_Skeleton.battleState);
    }
}
