using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

/* 控制实体数据，进行状态数据计算交换等 */
public class EntityStat : MonoBehaviour
{
    // 属性
    #region 
    [Header("major stat")]
    public Stat strength; // 力量值， +1% damage
    public Stat agility;  // 敏捷度， +0.5% avoid damage, +0.5 Critical hit rate
    public Stat intelligence; // 智力, +1% magic damage
    public Stat endurance; // 耐力，+2 - 4 health

    [Header("defence stat")]
    public Stat maxHealth;      
    public Stat currentHealth;
    public Stat armor;              // 护甲       
    public Stat magicResistance;    //魔法抗性
    public Stat avoidChance;

    [Header("damage stat")]
    public Stat physicalDamage;
    private Stat magicDamage = new Stat();     // 总魔法伤害
    public Stat fireDamage;       //火焰
    public Stat iceDamage;       //冰冻
    public Stat lightDamage;       //闪电
    public Stat criticalChance;

                                    // Debuff标志位
    [Header("Debuff")]              // 鬼鬼，各个debuff，还有一堆东西，持续时间，触发几率，持续伤害，惊吓增伤
    public bool burn;               // 燃烧
    public bool freeze;             // 冻结
    public bool shock;              // 麻痹

    [Header("Debuff ODDS")]                // 攻击时各类触发几率
    public Stat burnOdds;
    public Stat freezeOdds;
    public Stat shockOdds;

    [Header("Debuff Duration")]
    public Stat burnDuration;
    public Stat freezeDuration;
    public Stat shockDuration;
    #endregion

    // 对象
    #region
    // 用于计算伤害临时保存的攻击方属性对象
    private EntityStat attackerStat;

    // Element Debuff 标志位
    private bool elementDebuffFlag;

    // 实体对象
    private Entity entity;
    private EntityFX entityFX;

    #endregion

    // 事件与委托
    #region

    /* 定义生命值UI更新事件与委托 */
    // public delegate void OnUpdateHealthUIHandler(Stat maxHealth, Stat currentHealth);
    // public event OnUpdateHealthUIHandler OnUpdateHealthUI;

    // 下面这行代码和上面两行代码等价，相当于直接使用系统事先已经写好的声明，Func与Action这两个是语法糖，分别简化了带返回值和不带返回值的委托
    public Action<Stat, Stat> OnUpdateHealthUI;

    #endregion
    protected virtual void Awake()
    {
        entity = GetComponent<Entity>();
        entityFX = GetComponent<EntityFX>(); 
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentHealth.setValue(maxHealth.getValue());
        float temp = burnOdds.getValue();
        // 初始化攻击附带异常状态概率
        //burnOdds.setValue(fireDamage.getValue() > 20 ? 20 : fireDamage.getValue());
        //freezeOdds.setValue(iceDamage.getValue() > 20 ? 20 : iceDamage.getValue());
        //shockOdds.setValue(lightDamage.getValue() > 20 ? 20 : lightDamage.getValue());
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    // 主要的伤害计算逻辑
    public virtual void DoDamage(Entity _attacker)
    {

        attackerStat = _attacker.GetComponent<EntityStat>();

        magicDamage.setValue(attackerStat.fireDamage.getValue() + attackerStat.iceDamage.getValue() + attackerStat.lightDamage.getValue());  // 元素点伤总和为总魔法伤害
        int _totalMagicDamage = (int)Mathf.Floor((float)(attackerStat.magicDamage.getValue() * (1.0 + 0.1 * attackerStat.intelligence.getValue()) * MagicDamageReduction(80)));  // 魔法伤害总值 _ 算魔抗
        int _totalPhysicalDamage = (int)Mathf.Floor((float)(attackerStat.physicalDamage.getValue() * (1.0 + 0.1 * attackerStat.strength.getValue()) * PhysicalDamageReduction(80)));  // 物理伤害总值 _ 算护甲

        /* 总伤害计算 */
        int sumDamage = (_totalMagicDamage + _totalPhysicalDamage) * isCriticalHit(attackerStat);        // 暴击判断
        Debug.Log("total damage: " + sumDamage);

        // 元素反应检测   
        ElementDebuff(attackerStat);

        TakeDamage(sumDamage);
    }

    // 受到指定伤害
    public virtual void TakeDamage(int _damage)
    {
        currentHealth.setValue(currentHealth.getValue() - _damage);

        OnUpdateHealthUI?.Invoke(maxHealth, currentHealth);
        
    }

    /* 判断敌人伤害是否暴击，暴击伤害倍率 * 2，暂时性的硬编码 */
    protected virtual int isCriticalHit(EntityStat _attackerStat)
    {
        
        if (UnityEngine.Random.Range((float)0.0, (float)100.0) < criticalChance.getValue() + (0.5) * agility.getValue())
            return 2;
        return 1;
    }

    // 判断自身令伤害失效几率， 加上敌人命中之类的属性会再传入状态信息的
    protected virtual bool IsAvoidAttack()
    {
        if (UnityEngine.Random.Range((float)0.0, (float)100.0) < avoidChance.getValue() * (0.5) * agility.getValue())
            return true;
        return false;
    }


    // 判断护甲伤害减免，边际效应，最低受到伤害为20.0
    // senParam为敏感度，控制边际效应
    protected virtual double PhysicalDamageReduction(int senParam)
    {
        double rate = 80 / (80 + armor.getValue());
        if (rate < 0.2)
            return 0.2;
        return rate;
    }
    // 魔抗减免，类似
    protected virtual double MagicDamageReduction(int senParam)
    {
        double rate = 80 / (80 + magicResistance.getValue());
        if (rate < 0.2)
            return 0.2;
        return rate;
    }

    // 元素判断，根据随机roll点确定是否触发元素反应
    protected virtual void ElementDebuff(EntityStat _attackerStat)
    {
        // 异常状态叠加触发，可以套if，也可以拿标志位组等
        if (UnityEngine.Random.Range((float)0.0, (float)100.0) <= _attackerStat.burnOdds.getValue() && !burn)
        {
            // 燃烧标志位与计时
            burn = true;
            elementDebuffFlag = true;
            StartCoroutine(BurnCoroutine((float)_attackerStat.freezeDuration.getValue()));

            // debuff效果
            // 烧伤dot效果，暂且将伤害和伤害间隔进行硬编码
            StartCoroutine(DotCoroutine(_attackerStat.burnDuration.getValue(), 0.5f, 2));
            StartCoroutine(entityFX.BurnFXCoroutine((float)_attackerStat.burnDuration.getValue()));
            
            if (UnityEngine.Random.Range((float)0.0, (float)100.0) <= _attackerStat.freezeOdds.getValue() && !freeze)
            {
                // 冻结标志位与计时
                freeze = true;
                elementDebuffFlag = true;
                StartCoroutine(FreezeCoroutine((float)_attackerStat.freezeDuration.getValue()));

                // debuff效果
                // 动画减速
                entity.animator.speed = 0.5f;
                // 移动减速
                entity.MultiMoveSpeed(0.7f);
                // FX

                StartCoroutine(entityFX.FreezeFXCoroutine((float)_attackerStat.freezeDuration.getValue()));
                if (UnityEngine.Random.Range((float)0.0, (float)100.0) <= _attackerStat.shockOdds.getValue() && !shock)
                {
                    shock = true;
                    elementDebuffFlag = true;
                    StartCoroutine(ShockCoroutine((float)_attackerStat.freezeDuration.getValue()));
                }
            }
        }
    }

    // 异常持续时间，后续会开放持续时间参数
    IEnumerator BurnCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        burn = false;
        
    }
    IEnumerator FreezeCoroutine(float _duration)
    {
        yield return new WaitForSeconds(_duration);
        freeze = false;
        entity.animator.speed = 1.0f;
        entity.MultiMoveSpeed(1.0f);
    }
    IEnumerator ShockCoroutine(float _duration)
    {
        yield return new WaitForSeconds(_duration);
        shock = false;
    }

    // Dot伤害协程
    // 持续时间, 伤害生效间隔时间，Dot伤害值
    IEnumerator DotCoroutine(float _duration, float _span, int _dotDamage)
    {
        float elapsed = 0.0f;
        while (elapsed <= _duration)
        {
            yield return new WaitForSeconds(_span);
            TakeDamage(_dotDamage);
            elapsed = elapsed + _span;
        }
    }
}
