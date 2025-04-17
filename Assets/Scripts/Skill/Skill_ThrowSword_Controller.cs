using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Skill_Sword_Controller : MonoBehaviour
{
    #region Components
    private Rigidbody2D rb;
    private Collider2D col;
    private Animator animator;
    private GameObject sword;
    private GameObject playerObj;
    private PlayerManager playerMg;
    #endregion

    #region Param
    private bool canRotate;

    // 收剑相关
    private bool isReturning;
    private float returnSpeed;

    // 弹射相关
    private bool canBouncy;
    float bouncyRadius;
    int maxBouncyTime;
    int currentBouncyTimes;
    List<GameObject> hasBouncedEnemies = new List<GameObject>(); // 已经被反弹过的敌人列表
    GameObject nextEnemy; // 下一个目标

    // 穿刺相关
    private bool canPierce;
    int maxPierceTimes;
    int currentPierceTimes;

    // 旋转相关
    private bool canSpin;
    private float maxSpinTime; // 旋转持续时间
    private float startSpinDistance; // 开始旋转距离
    private Vector2 startPosition; // 投掷起始点
    private bool isSpinning; // 是否正在旋转

    private float spinDuraion; // 旋转时间
    
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        animator = GetComponentInChildren<Animator>();

        sword = transform.gameObject;
        canRotate = true;
    }

    private void Start()
    {
        playerMg = PlayerManager.instance;
    }

    /* 从 skill 那边传信息过来的 */
    /* 弹射剑和穿刺剑的行为是不一样的，并且它们是互斥的所以分开创建 */

    /* 设置穿刺剑 */
    public void SetUpPierce(bool _canPierce, int _maxPierceTimes)
    {
        canPierce = _canPierce;
        maxPierceTimes = _maxPierceTimes;

        SetupIgnoreSwordCollision(false);
    }

    /* 设置弹射剑 */
    public void SetUpBounce(bool _canBouncy, float _bouncyRadius, int _maxBouncyTimes)
    {

        canBouncy = _canBouncy;
        bouncyRadius = _bouncyRadius;
        maxBouncyTime = _maxBouncyTimes;

        SetupIgnoreSwordCollision(false);
    }

    /* 设置旋转剑 */
    public void SetUpSpin(bool _canSpin, float _spinDuration, float _startSpinDistance, Vector2 _startPosition)
    {
        canSpin = _canSpin;
        maxSpinTime = _spinDuration;
        startSpinDistance = _startSpinDistance;
        startPosition = _startPosition;

        SetupIgnoreSwordCollision(false);
    }

    /* 设置重复属性 */
    public void SetUpSword(GameObject _player, float _swordGravity, Vector2 _force, Vector2 _aimDir, bool _isReturning, float _returnSpeed)
    {
        sword.transform.position = _player.transform.position;
        playerObj = _player;
        rb.gravityScale = _swordGravity;
        isReturning = _isReturning;

        rb.velocity = _force * _aimDir;
        returnSpeed = _returnSpeed;
    }


    private void Update()
    {
        /* 即使 free 了，但如果这行代码持续生效的话，还是会改变物体的旋转方向 */
        if (canRotate)
            transform.right = rb.velocity;

        /* 收剑 */
        if (isReturning)
        {
            rb.velocity = new Vector2((playerObj.transform.position.x - sword.transform.position.x) * returnSpeed,
                (playerObj.transform.position.y - sword.transform.position.y) * returnSpeed);
            /* 销毁距离硬编码得了 */
            if (Vector2.Distance(playerObj.transform.position, sword.transform.position) < 0.5f && sword != null)
            {
                /* 设置接剑方向 */
                if (transform.position.x - playerObj.transform.position.x > 0 && !playerMg.player.faceRight)
                    playerMg.player.FlipCharacter();
                if (transform.position.x - playerObj.transform.position.x < 0 && playerMg.player.faceRight)
                    playerMg.player.FlipCharacter();

                /* 销毁飞剑 */
                GameObject.Destroy(sword);

                /* 设置标志位，进入接剑状态 */
                PlayerManager.instance.player.catchSword = true;
            }

            
        }
        // 定位弹射
        if (canBouncy && nextEnemy != null)
            rb.velocity = new Vector2((nextEnemy.transform.position.x - sword.transform.position.x) * returnSpeed,
                (nextEnemy.transform.position.y - sword.transform.position.y) * returnSpeed);

        // 定点旋转
        if (canSpin && Vector2.Distance(transform.position, startPosition) > startSpinDistance && !isSpinning)
        {
            // 距离投掷点超过一定距离时开始定点旋转
            rb.velocity = new Vector2(0.0f, 0.0f);
            isSpinning = true;
        }
        if (isSpinning)
        {
            spinDuraion += Time.deltaTime;
            if (spinDuraion > maxSpinTime)
                ReturnSword();
        }

    }

    /* 接触到物体停止 */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        /* 击中敌人后，且范围内有其他敌人，且具有弹射能力 */
        if (canBouncy && collision.CompareTag("Enemy") && CanContinueBouncyCheck())
        {
            hasBouncedEnemies.Add(collision.gameObject);
            nextEnemy = GetNearestEnemyInRadius();

            // 受到伤害，暂且硬编码为玩家的第一段攻击
            collision.GetComponent<Enemy>().DamagedByAttackPhase(1);
        } else if (canPierce && maxPierceTimes > 0) 
        {
            maxPierceTimes--;
            collision.GetComponent<Enemy>().DamagedByAttackPhase(1);
        } else if (canSpin)
        {
            // 保持旋转状态并定时造成伤害
            StartCoroutine(FixedUpdateDamage());
        }
        else
        {
            /* 基础形式，不反弹 */
            BaseSwordAttack(collision);
            /* 如果反弹过程中打到 ground 上，清除 nextEnemy ，避免 update 中持续向 nextEnemy 移动*/
            nextEnemy = null;
        }

        
    }

    public void ReturnSword()
    {
        transform.parent = null;
        isReturning = true;
        canRotate = true;

        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints2D.None;

        SetupIgnoreSwordCollision(true);
    }

    private void BaseSwordAttack(Collider2D collision)
    {
        col.enabled = false;
        canRotate = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        transform.parent = collision.transform;
        
        /* anim */
        animator.SetBool("Rotate", false);
        animator.SetBool("Insert", true);
    }


    /* 设置剑与 enemy 和 ground 的碰撞关系，layer 序号硬关联，不好 */
    private void SetupIgnoreSwordCollision(bool _flag)
    {
        Physics2D.IgnoreLayerCollision(10, 9, _flag);
        Physics2D.IgnoreLayerCollision(10, 3, _flag);
    }

    /* 是否可以继续弹射检查 */
    private bool CanContinueBouncyCheck()
    {
        /* 攻击范围内存在敌人 & 未达到弹射上限 */
        if (currentBouncyTimes <= maxBouncyTime && GetNearestEnemyInRadius() != null)
            return true;
        else
            return false;
    }

    /* 最近的弹射目标 */
    private GameObject GetNearestEnemyInRadius()
    {
        GameObject nearestEnemy = null;
        // stat
        Collider2D[] cols = Physics2D.OverlapCircleAll(sword.transform.position, bouncyRadius);
        foreach (Collider2D col in cols)
        {
            /* 是敌人 & 不在已被弹射列表中 */
            if (col.CompareTag("Enemy") && !hasBouncedEnemies.Contains(col.gameObject))
            {
                
                nearestEnemy = EnemyWhoCloserToPlayer(col.gameObject, nearestEnemy);
            }
        }

        return nearestEnemy;
    }

    private GameObject EnemyWhoCloserToPlayer(GameObject A, GameObject B)
    {
        if (B == null)
            return A;
        else
        {
            if (Vector2.Distance(A.transform.position, playerObj.transform.position) <
            Vector2.Distance(B.transform.position, playerObj.transform.position))
                return A;
            else
                return B;
        }
    }

    private IEnumerator FixedUpdateDamage()
    {
        Collider2D[] cols;
        while (spinDuraion < maxSpinTime)
        {
            yield return new WaitForSeconds(0.3f);
            cols = Physics2D.OverlapCircleAll(sword.transform.position, 1);
            foreach(Collider2D col in cols)
                if (col.CompareTag("Enemy"))
                    col.GetComponent<Enemy>().DamagedByAttackPhase(0);
        }
    }

}
