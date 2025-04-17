using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwordType
{
    baseSword,
    BounceSword,
    PierceSword,
    SpinSword
}
public class Skill_ThrowSword : Skill
{
    [SerializeField] private SwordType type;

    [Header("Prefab")]
    [SerializeField] private GameObject swordPrefab;

    [Header("Rigidbody")]
    [SerializeField] private float swordGravity;
    [SerializeField] private Vector2 swordVelocity;

    [Header("Force & Speed")]
    [SerializeField] private float returnSpeed;

    [Header("Dots")]
    [SerializeField] private int dotNum;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private float dotGap;

    [Header("Bouncy Info")]
    [SerializeField] private float bouncyRadius;
    [SerializeField] private int maxBouncyTimes;
    [SerializeField] private float bouncySwordGravity;
    [SerializeField] private Vector2 bouncySwordVelocity;

    [Header("Pierce Info")]
    [SerializeField] private int maxPierceTimes;
    [SerializeField] private float pierceSwordGravity;
    [SerializeField] private Vector2 pierceSwordVelocity;

    [Header("Spin Info")]
    [SerializeField] private float spinSwordGravity;
    [SerializeField] private Vector2 spinSwordVelocity;
    [SerializeField] private float maxSpinTime;
    [SerializeField] private float startSpinDistance;

    private GameObject[] dots;
    [SerializeField] private Transform dotsParent;
    private Vector2 aimDir;
    private bool isReturning;

    Skill_Sword_Controller controller;

    public override bool CanUseSkill()
    {
        /* 简单的CD检查 */
        if (IsCooldownComplete())
            return true;
        else
        {
            if (!IsCooldownComplete())
                Debug.Log("Skill: " + skillName + " is CoolDowning ");
            
            return false;
        }
    }

    public override void SkillAction()
    {
        base.SkillAction();
        if ( player.flyingSword == null)
        {
            /* 创建实体 */
            GameObject newSword = GameObject.Instantiate(swordPrefab);
            switch (type)
            {
                case SwordType.BounceSword:
                    newSword.GetComponent<Skill_Sword_Controller>().SetUpBounce(true, bouncyRadius, maxBouncyTimes);
                    swordGravity = bouncySwordGravity;
                    swordVelocity = bouncySwordVelocity;
                    break;
                case SwordType.PierceSword:
                    newSword.GetComponent<Skill_Sword_Controller>().SetUpPierce(true, maxPierceTimes);
                    swordGravity = pierceSwordGravity;
                    swordVelocity = pierceSwordVelocity;
                    break;
                case SwordType.SpinSword:
                    newSword.GetComponent<Skill_Sword_Controller>().SetUpSpin(true, maxSpinTime, startSpinDistance, player.transform.position);
                    swordGravity = spinSwordGravity;
                    swordVelocity = spinSwordVelocity;
                    break;
            }

            // 通用属性
            newSword.GetComponent<Skill_Sword_Controller>().SetUpSword(player.gameObject,
                    swordGravity, swordVelocity, aimDir, isReturning, returnSpeed);


            /* 登记剑 */
            player.AssignedSword(newSword);
        } else
            ReturnSword();
    }



    public override void UseSkill()
    {
        base.UseSkill();
    }

    /* 返回归一化瞄准方向 */
    public Vector2 AimDirection()
    {
        /* 起始点 */
        Vector2 startPos = player.transform.position;
        /* 鼠标瞄准 */
        Vector2 aimPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        return new Vector2(aimPos.x - startPos.x, aimPos.y - startPos.y).normalized;
    }

    /* 生成 dots 实例 */
    public GameObject[] GenDots()
    {
        dots = new GameObject[dotNum];
        for (int i = 0; i < dotNum; i++)
        {
            /* 生成实例并归于 父对象下  */
            dots[i] = GameObject.Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
        return dots;
    }

    /* 根据 dot 的 index 排列 dots
     * 这里的计算其实就是将时间参数替换为 dot 的 index 参数
     * 线和实际运动轨迹不一致，不知道为啥 */
    public void DotsPostion()
    {
        switch (type)
        {
            case SwordType.BounceSword:
                swordGravity = bouncySwordGravity;
                swordVelocity = bouncySwordVelocity;
                break;
            case SwordType.PierceSword:
                swordGravity = pierceSwordGravity;
                swordVelocity = pierceSwordVelocity;
                break;
        }

        for (int t = 0; t < dotNum; t++)
        {
            float finalT = t * dotGap;
            dots[t].transform.localPosition = new Vector2(aimDir.x * swordVelocity.x * finalT,
                (0.5f * Physics2D.gravity.y * swordGravity * finalT * finalT) + (aimDir.y * swordVelocity.y * finalT));
            dots[t].SetActive(true);
        }
        dotsParent.gameObject.SetActive(true);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        aimDir = AimDirection();
        /* dots 的位置于 player 同步，并不参与 player 的翻转 */
        dotsParent.transform.position = player.transform.position;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    private void ReturnSword()
    {
        player.flyingSword.GetComponent<Skill_Sword_Controller>().ReturnSword();
    }
}
