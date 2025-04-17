
using UnityEngine;

public class Enemy_SkeletonIdleState : Enemy_SkeletonPatrolState
{
    public Enemy_SkeletonIdleState(Enemy baseEnemy, EnemyStateMeachine stateMeachine, string animBoolName, Enemy_Skeleton _enemy) : base(baseEnemy, stateMeachine, animBoolName, _enemy)
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
        if (stateTimer > 1f)
            stateMeachine.changeState(enemy_Skeleton.walkState);
    }
}
