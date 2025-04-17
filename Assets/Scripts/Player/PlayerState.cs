using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;

    /* 换成public 好在machine中用一下 */
    public string animBoolName;

    protected float xInput;
    protected float yInput;
    protected float yVelocity;
    protected bool isGrounded;

    /* 本来以为只是用来做 attack 的 trigger，但好像每个需要完整播放的，需要动画结束控制流程的东西都可以用它 */
    protected bool animationTrigger;

    /* 计时器 */
    protected float stateTimer;

    public PlayerState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName)
    {
        this.stateMachine = _stateMachine;
        this.player = _player;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        Debug.Log("enter: " + animBoolName);
        player.animator.SetBool(animBoolName, true);

        animationTrigger = false;

        stateTimer = 0;
    }

    public virtual void Exit()
    {
        Debug.Log("exit: " + animBoolName);
        player.animator.SetBool(animBoolName, false);
    }

    public virtual void Update()
    {
        /* 计时器更新 */
        stateTimer += Time.deltaTime;

        /* 任何状态下都进行 xInput 和 yInput 的更新 */
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        /* 任何状态下进行接地检测 */
        isGrounded = player.GroundedCheck();

        /* 任何状态下更新 y 方向速度 */
        /* 只能说为了和其他的参数设置代码放在一块而这样写是真的 傻逼 */
        player.SetAnimXYVelocity(player.rigidbody2D.velocity.x, player.rigidbody2D.velocity.y);

        /* 或许每个状态都可以直接冲刺 */
        if (Input.GetKeyDown(KeyCode.LeftShift) && player.dashTempCD < 0 && stateMachine.currentState != player.dashState)
            stateMachine.ChangeState(player.dashState);

        /* flip (原来写在这里的 flip 抽象到 entity 里了 */

        /* 死亡检测 */
        if (player.entityStat.currentHealth.getValue() <= 0)
            stateMachine.ChangeState(player.deathState);
    }

    /* 用来被 Player 在动画结束时调用对应状态的 trigger 置 true */
    public void AnimationFinished()
    {
        animationTrigger = true;
    }
}
