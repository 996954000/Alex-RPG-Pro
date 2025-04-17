using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Skill : MonoBehaviour
{
    [Header("Managers")]
    protected Player player;

    [Header("Skill Message")]
    [SerializeField] protected string skillName;

    [Header("CoolDown")]
    [SerializeField] protected float coolDownTime;
    protected float currentCoolDownTime;

    [Header("Skill State")]
    /* 标记技能可用状态，若有沉默等状态可使用此属性 */
    protected bool skillCanUse;

    protected virtual void Update()
    {
        if (currentCoolDownTime > 0)
            currentCoolDownTime -= Time.deltaTime;
        else
            currentCoolDownTime = 0;
    }
    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }
    protected virtual void OnDestroy()
    {
        
    }
    public bool IsCooldownComplete()
    {
        return currentCoolDownTime == 0 ? true : false;
    }

    /* 暂且设为公开 */
    public virtual bool CanUseSkill()
    {
        /* 简单的CD检查 */
        if (IsCooldownComplete())
            return true;
        else
        {
            Debug.Log("Skill: " +  skillName + " is CoolDowning ");
            return false;
        }
    }
    /* 使用技能 */
    public virtual void UseSkill()
    {
        if (CanUseSkill()) {
            /* 技能 */
            SkillAction();

            /* 重新计算CD */
            currentCoolDownTime = coolDownTime;
        }

    }

    /* 具体的技能逻辑 */
    public virtual void SkillAction() { 
    
    }

    protected virtual void Awake()
    {
        
    }
}
