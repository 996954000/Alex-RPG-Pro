using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Entity
{
    #region Parameter
    [Header("Detection Info")]
    [SerializeField] protected Transform detectionTrans;
    [SerializeField] protected Vector2 detectionDir;
    [SerializeField] public float detectionDistance;
    [SerializeField] protected LayerMask whatIsPlayer;

    [Header("Attack Info")]
    [SerializeField] public float attackRange;
    [SerializeField] public Transform attackTrans;
    [SerializeField] public float attackRadius;

    [SerializeField] public bool canBeCounterAttackedWindow;
    [SerializeField] public float beStunnedDuration;
    public bool beStunned;

    [Header("move Info")]
    // [SerializeField] public float moveSpeed;
    private float originalSpeed;
    #endregion
    #region Components
    protected EnemyStateMeachine stateMeachine;
    [SerializeField] protected GameObject canBeCAImage;
    #endregion

    public bool tempGrounded;
    protected override void Awake()
    {
        base.Awake();
        stateMeachine = new EnemyStateMeachine();

        underAttack = false;

        canBeCAImage.SetActive(false);
        canBeCounterAttackedWindow = false;
        originalSpeed = moveSpeed;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        stateMeachine.currentState.Update();
        tempGrounded = GroundedCheck();
        /* 寻敌方向与面对方向始终保持一致 */
        detectionDir = faceDir;


        /* 反击标志与 window 始终保持一致
         * 这样做性能肯定会低吧，应该有更好的方法 */
        canBeCAImage.SetActive(canBeCounterAttackedWindow);

    }

    /* 寻敌以及范围绘制 感觉后面得换成圆形寻敌 */
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        /* 追击范围 */
        Gizmos.color = Color.green;
        Gizmos.DrawLine(detectionTrans.transform.position,
            new Vector2(detectionTrans.transform.position.x + detectionDistance * detectionDir.normalized.x,
            detectionTrans.transform.position.y + detectionDistance * detectionDir.normalized.y));

        /* 绘制攻击范围（直线）
         * 这个是开始执行攻击动作的检测距离 */
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position,
            new Vector2(transform.position.x + attackRange * faceDir.x,
            transform.position.y));

        /* 绘制伤害范围（圆形）
         * 这个是攻击发动后，能造成伤害的范围*/
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackTrans.position, attackRadius);
    }

    public bool PlayerDetection()
    {
        return Physics2D.Raycast(detectionTrans.transform.position,
            detectionDir.normalized,
            detectionDistance,
            whatIsPlayer);
    }

    public override void DamagedByAttackPhase(int attackPhase)
    {
        base.DamagedByAttackPhase(attackPhase);
    }

    public void AnimationFinished() => stateMeachine.currentState.AnimationFinished();

    /* 反击相关 */
    public bool CanBeCounterAttackWindowCheck() => canBeCounterAttackedWindow;

    public void OpenCounterAttackWindow()
    {
        canBeCounterAttackedWindow = true;
        //canBeCAImage.SetActive(true);
    }

    public void CloseCounterAttackWindow()
    {
        canBeCounterAttackedWindow = false;
        //canBeCAImage.SetActive(false);
    }

    /* 反击击晕状态检查，及时关闭击晕窗口
     * 更新击晕状态*/
    public virtual void BeStunned()
    {
        if (beStunned)
        {
            Debug.Log("BESTUNNED" + beStunned);
            canBeCounterAttackedWindow = false;
            return;
        }
        else
            beStunned = true;
    }


}
