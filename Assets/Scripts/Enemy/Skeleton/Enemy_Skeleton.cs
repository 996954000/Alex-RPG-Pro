using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton : Enemy
{
    /* 因为 Enemy 会分为很多种敌人，而且每个敌人的参数类型，状态都不一样，所以写了个中间类 */
    #region States
    public Enemy_SkeletonIdleState idleState { get; private set; }
    public Enemy_SkeletonWalkState walkState { get; private set; }
    /* 警觉动画状态 */
    public Enemy_SkeletonReactState reactState { get; private set; }
    /* 寻敌状态 */
    public Enemy_SkeletonBattleState battleState { get; private set; }
    /* 攻击状态 */
    public Enemy_SkeletonAttackState attackState { get; private set; }
    /* 受击状态 */
    public Enemy_SkeletonUnderAttackState underAttackState { get; private set; }
    /* 晕眩状态 */
    public Enemy_SkeletonStunState stunState { get; private set; }
    /* 死亡状态 */
    public Enemy_SkeletonDeathState deathState { get; private set; }
    #endregion
    #region Parameters
    [Header("Move Info")]
    /* 追击状态移动速度乘法系数 */
    [SerializeField] public float battleSpeedMultiplier;
    #endregion
    protected override void Awake()
    {
        base.Awake();
        
        idleState = new Enemy_SkeletonIdleState(this, stateMeachine, "Idle", this);
        walkState = new Enemy_SkeletonWalkState(this, stateMeachine, "Walk", this);
        battleState = new Enemy_SkeletonBattleState(this, stateMeachine, "Battle", this);
        reactState = new Enemy_SkeletonReactState(this, stateMeachine, "React", this);
        attackState = new Enemy_SkeletonAttackState(this, stateMeachine, "Attack", this);
        underAttackState = new Enemy_SkeletonUnderAttackState(this, stateMeachine, "UnderAttack", this);
        stunState = new Enemy_SkeletonStunState(this, stateMeachine, "Stun", this);
        deathState = new Enemy_SkeletonDeathState(this, stateMeachine, "Death", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMeachine.initState(idleState);
    }

    protected override void Update()
    {
        base.Update();
        /* 检查击晕状态 */
        if (beStunned)
            stateMeachine.changeState(stunState);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }

    public override void BeStunned()
    {
        base.BeStunned();
    }
}
