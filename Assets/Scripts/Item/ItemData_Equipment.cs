using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EquipmentType
{
    armor,
    weapon
}

[CreateAssetMenu(fileName = "new Equipment Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;

    /* 装备附加属性 */
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
    public Stat criticalChance;     // 暴击几率

    #endregion

}
