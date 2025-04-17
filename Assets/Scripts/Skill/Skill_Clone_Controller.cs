using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Clone_Controller : MonoBehaviour
{
    private float duration;
    private Animator animator;
    private int attackPhase;

    [SerializeField] private Transform attackTrans;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        duration -= Time.deltaTime;
        transform.parent.GetComponentInChildren<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, duration);

        /* 持续时间过后销毁，或许也需要一个集中的 gameobject 管理器来控制这些东西的销毁与生成 */
        if(duration < 0.0f)
        {
            Destroy(transform.parent.gameObject);
        }


    }

    public void SetupClone(Vector2 _position, float _duration, bool _canAttack, bool _faceRight)
    {
        transform.parent.position = _position;
        duration = _duration;
        /* 设置克隆体不下落 */
        transform.parent.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
        if (_canAttack)
        {
            animator.SetBool("Attack", true);
            attackPhase = Random.Range(0, 2);
            animator.SetInteger("GroundAttackPhase", attackPhase);
        }

        /* 反正克隆技能目前就给 player 用，player 的 sprite 初始就是向右面的，所以就不判断初始的面朝方向了 */
        if (!_faceRight)
            transform.parent.transform.localScale = new Vector3(-transform.parent.transform.localScale.x,
                transform.parent.transform.localScale.y,
                transform.parent.transform.localScale.z);
    }

    public void DamageTrigger()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(attackTrans.position, PlayerManager.instance.player.groundAttackRadius[attackPhase]);
        foreach(var hit in collider2Ds)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                /* 不同的攻击阶段对应不同的受伤效果 */
                hit.GetComponent<Enemy>().DamagedByAttackPhase(attackPhase);
            }
        }
    }
}
