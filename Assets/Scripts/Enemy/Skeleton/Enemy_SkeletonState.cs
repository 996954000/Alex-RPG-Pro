using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SkeletonState : EnemyState
{
    /* 为了方便使用enemy子类的独有参数，只好把这玩意传进来了，后面应该会有优化 */
    protected Enemy_Skeleton enemy_Skeleton;
    protected Transform playerTrans;
    public Enemy_SkeletonState(Enemy baseEnemy, EnemyStateMeachine stateMeachine, string animBoolName, Enemy_Skeleton _enemy) : base(baseEnemy, stateMeachine, animBoolName)
    {
        enemy_Skeleton = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("STATE::ENEMY::SKELETON::Enter: " + animBoolName);
        playerTrans = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("STATE::ENEMY::SKELETON::Exit: " + animBoolName);
    }

    public override void Update()
    {
        if (enemy_Skeleton.underAttack)
            stateMeachine.changeState(enemy_Skeleton.underAttackState);
        /* 检查自身血量，如果血量为0，则切换到死亡状态 */
        if (enemy_Skeleton.entityStat.currentHealth.getValue() <= 0)
            stateMeachine.changeState(enemy_Skeleton.deathState);
        base.Update();
    }
}
