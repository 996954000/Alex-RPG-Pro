using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyState
{
    protected Enemy baseEnemy;
    protected EnemyStateMeachine stateMeachine;
    protected string animBoolName;

    protected float stateTimer;
    protected bool animationTrigger;
    public EnemyState(Enemy baseEnemy, EnemyStateMeachine stateMeachine, string animBoolName)
    {
        this.baseEnemy = baseEnemy;
        this.stateMeachine = stateMeachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        stateTimer = 0;
        animationTrigger = false;
        baseEnemy.animator.SetBool(animBoolName, true);
    }

    public virtual void Exit()
    {
        baseEnemy.animator.SetBool(animBoolName, false);
    }

    public virtual void Update()
    {
        stateTimer += Time.deltaTime;
    }

    /* 用来被 Player 在动画结束时调用对应状态的 trigger 置 true
     * trigger = true 代表动画播放完毕，可以进入下一个状态了*/
    public void AnimationFinished()
    {
        animationTrigger = true;
    }
}
