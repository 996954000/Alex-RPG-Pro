using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    #region Components
    protected PlayerStateMachine stateMachine;
    public GameObject flyingSword;
    #endregion

    #region States
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    /* ÌøÔ¾ÉýÆð×´Ì¬ */
    public PlayerJumpState jumpState { get; private set; }
    /* ÏÂÂä×´Ì¬ °üÀ¨ÌøÔ¾ºó°ë¶Î ÒÔ¼°´Ó¸ß´¦ÌøÏÂ½×¶Î */
    public PlayerAirState airState { get; private set; }
    /* ³å´Ì×´Ì¬ */
    public PlayerDashState dashState { get; private set; }
    /* »¬Ç½×´Ì¬ */
    public PlayerSlideWallState slideWallState { get; private set; }
    /* »¬Ç½Ìø×´Ì¬ */
    public PlayerWallJumpState wallJumpState { get; private set; }
    /* Ê×Òª¹¥»÷×´Ì¬ */
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    /* ·´»÷×´Ì¬ */
    public PlayerCounterAttackState counterAttackState { get; private set; }
    /* Ãé×¼×´Ì¬ */
    public PlayerAimState aimState { get; private set; }
    /* Í¶ÖÀ×´Ì¬ */
    public PlayerThrowSwordState throwSwordState { get; private set;}
    /* ½Ó½£×´Ì¬ */
    public PlayerCatchState catchSwordState { get; private set;}
    /* ËÀÍö×´Ì¬ */
    public PlayerDeathState deathState { get; private set; }
    #endregion

    #region Parameters
    [SerializeField] private float timeScale;

    [Header("Movement Info")]
    // [SerializeField] public float moveSpeed;
    [SerializeField] public float jumpForce;
    [SerializeField] public Vector2[] attackMovement;

    [Header("Dash Info")]
    [SerializeField] public float dashSpeed;
    [SerializeField] public float dashCD; // ³å´Ì CD
    [SerializeField] public float dashDuration; // ³å´Ì³ÖÐøÊ±¼ä
    public float dashTempCD;

    [Header("Ground Attack Info")]
    [SerializeField] public float groundAttackWindow;
    [SerializeField] public Transform groundAttackTrans;
    [SerializeField] public float[] groundAttackRadius;
    [SerializeField] public int groundAttackPhase;
    #endregion

    #region Managers
    public PlayerManager player { get; private set; }
    public SkillManager skill { get; private set; }
    #endregion
    private bool temp;
    public bool catchSword;

    protected override void Awake()
    {
        base.Awake();
        Debug.Log("LIFECYCLE::" + gameObject.name + "awake");

        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(stateMachine, this, "Idle");
        moveState = new PlayerMoveState(stateMachine, this, "Move");
        jumpState = new PlayerJumpState(stateMachine, this, "Jump");
        airState  = new PlayerAirState(stateMachine, this, "Jump");
        wallJumpState = new PlayerWallJumpState(stateMachine, this, "Jump");
        dashState = new PlayerDashState(stateMachine, this, "Dash");
        slideWallState = new PlayerSlideWallState(stateMachine, this, "SlideWall");
        primaryAttackState = new PlayerPrimaryAttackState(stateMachine, this, "Attack");
        counterAttackState = new PlayerCounterAttackState(stateMachine, this, "CounterAttack");
        aimState = new PlayerAimState(stateMachine, this, "Aim");
        throwSwordState = new PlayerThrowSwordState(stateMachine, this, "ThrowSword");
        catchSwordState = new PlayerCatchState(stateMachine, this, "CatchSword");
        deathState = new PlayerDeathState(stateMachine, this, "Death");

    }

    protected override void Start()
    {
        Debug.Log("LIFECYCLE::" + gameObject.name + " start");
        base.Start();
        stateMachine.Initialize(idleState);

        player = PlayerManager.instance;
        skill = SkillManager.instance;

        
    }

    protected override void Update()
    {
        base.Update();

        Time.timeScale = timeScale;
        stateMachine.currentState.Update();

        animator.SetBool("IsGrounded", GroundedCheck());
        if (faceRight && rigidbody2D.velocity.x < 0 || !faceRight && rigidbody2D.velocity.x > 0)
            FlipCharacter();

        temp = WallCheck();
        DashCDTimer();

        /* ÊµÊ±¸üÐÂ¹¥»÷½×¶ÎÓ¦ÓÃ²»Í¬µÄ¹¥»÷·¶Î§ */
        groundAttackPhase = primaryAttackState.GetGroundAttackPhase();
    }
    /* Timers */
    private void DashCDTimer()
    {
        dashTempCD -= Time.deltaTime;
    }
    /* animation trigger */
    public void AnimationFinishedTrigger() => stateMachine.currentState.AnimationFinished();
    public override void DamagedByAttackPhase(int _attackPhase)
    {
        base.DamagedByAttackPhase(_attackPhase);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundAttackTrans.position, groundAttackRadius[groundAttackPhase]);
    }

    /* ×¢²á·É³öÈ¥µÄ½£ */
    public void AssignedSword(GameObject _newSword)
    {
        flyingSword = _newSword;
    }

    /* È¡Ïû½£µÄµÇ¼Ç */
    public void ClearSword()
    {
        GameObject.Destroy(flyingSword);
    }
}
