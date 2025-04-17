using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Components
    public Animator animator { get; private set; }
    public Rigidbody2D rigidbody2D { get; private set; }
    public EntityFX entityFX { get; private set; }
    public EntityStat entityStat { get; private set; }
    #endregion

    #region States

    #endregion

    #region Parameters
    [Header("Ground Check Info")]
    [SerializeField] protected Transform groundCheckTrans;
    [SerializeField] protected Vector2 checkDir;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    [Header("Wall Check Info")]
    [SerializeField] protected Transform wallCheckTrans;
    [SerializeField] protected Vector2 wallCheckDir;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsWall;

    [Header("Flip Info")]
    public Vector2 faceDir;
    [SerializeField] public bool faceRight;

    [Header("Damaged")]
    [SerializeField] public Vector2 knockbackForce;
    [SerializeField] protected float underAttackDuration;
    public int underAttackPhase;
    public bool underAttack;

    [Header("Gen Param")]
    [SerializeField] public float moveSpeed;
    private float oriMoveSpeed;
    #endregion
    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        entityFX = GetComponent<EntityFX>();
        entityStat = GetComponent<EntityStat>();

        if (faceRight)
            faceDir = new Vector2(1.0f, 0.0f);
        else
            faceDir = new Vector2(-1.0f, 0.0f);

    }
    protected virtual void Start()
    {
        underAttack = false;
        oriMoveSpeed = moveSpeed;
    }
    protected virtual void Update()
    {
        wallCheckDir = faceDir;
    }
    /* 翻转修正 */
    public virtual void FlipFix()
    {
        if (faceRight && rigidbody2D.velocity.x < 0 || !faceRight && rigidbody2D.velocity.x > 0)
            FlipCharacter();
    }
    public virtual void FlipCharacter()
    {
        faceRight = !faceRight;
        faceDir = -faceDir;
        gameObject.transform.localScale = new Vector3(-1.0f * gameObject.transform.localScale.x,
            gameObject.transform.localScale.y,
            gameObject.transform.localScale.z);
    }
    /* 接地检测 */
    public virtual bool GroundedCheck()
    {
        return Physics2D.Raycast(groundCheckTrans.transform.position,
            checkDir.normalized,
            groundCheckDistance,
            whatIsGround);
    }
    /* 墙面检测 */
    public virtual bool WallCheck()
    {
        return Physics2D.Raycast(wallCheckTrans.transform.position,
            wallCheckDir.normalized,
            wallCheckDistance,
            whatIsWall);
    }
    /* 绘制工具方法 */
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheckTrans.transform.position,
            new Vector2(groundCheckTrans.transform.position.x + groundCheckDistance * checkDir.normalized.x,
            groundCheckTrans.transform.position.y + groundCheckDistance * checkDir.normalized.y));

        Gizmos.DrawLine(wallCheckTrans.transform.position,
            new Vector2(wallCheckTrans.transform.position.x + wallCheckDistance * wallCheckDir.normalized.x,
            wallCheckTrans.transform.position.y + wallCheckDistance * wallCheckDir.normalized.y));
    }
    /* 设置刚体速度 */
    public virtual void SetVelocity(float x, float y)
    {
        /* 若处于击退状态则禁用其他速度设置
         * 所以使用一个总的方法控制速度的好处就在这里了 */
        if (underAttack)
            return;

        rigidbody2D.velocity = new Vector2(x, y);
    }
    /* 用来设置 animator 中的参数 */
    public virtual void SetAnimXYVelocity(float xVelocity, float yVelocity)
    {
        animator.SetFloat("xAbsVelocity", Mathf.Abs(xVelocity));
        animator.SetFloat("yVelocity", yVelocity);
    }
    public virtual void DamagedByAttackPhase(int attackPhase)
    {
        Debug.Log(gameObject.name + "was damaged");

        /* 按照 攻击者 传入的 attackPhase 更新 受击者 的underAttackPhase */
        underAttackPhase = attackPhase;
        /* 按帧更新 enemy 的受击状态供状态机使用 */
        if (!underAttack)
            StartCoroutine(UnderAttackCoroutien());
        /* 被击中时闪光反馈 */
        StartCoroutine(entityFX.FlashFXCoroutine());
    }
    protected virtual IEnumerator UnderAttackCoroutien()
    {
        underAttack = true;
        yield return new WaitForSeconds(.05f);
        underAttack = false;
    }

    // 由deathTrigger调用，在死亡动画播放完毕后销毁gameObject
    public virtual void Death()
    {
        StartCoroutine(DestroyAfter(3.0f));
    }
    private IEnumerator DestroyAfter(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    // 移动速度减速
    public void MultiMoveSpeed(float multi)
    {
        if (multi == 1.0f)
            moveSpeed = oriMoveSpeed;
        else
            moveSpeed = moveSpeed * multi;
    }
}
