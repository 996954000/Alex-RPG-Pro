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

    public void AddModifier()
    {
        PlayerStat stat = PlayerManager.instance.player.GetComponent<PlayerStat>();
        stat.strength.AddModifier(strength.getValue());
        stat.agility.AddModifier(agility.getValue());
        stat.intelligence.AddModifier(intelligence.getValue());
        stat.endurance.AddModifier(endurance.getValue());

        stat.maxHealth.AddModifier(maxHealth.getValue());
        stat.armor.AddModifier(armor.getValue());
        stat.magicResistance.AddModifier(magicResistance.getValue());
        stat.avoidChance.AddModifier(avoidChance.getValue());

        stat.physicalDamage.AddModifier(physicalDamage.getValue());
        stat.fireDamage.AddModifier(fireDamage.getValue());
        stat.iceDamage.AddModifier(iceDamage.getValue());
        stat.lightDamage.AddModifier(lightDamage.getValue());
        stat.criticalChance.AddModifier(criticalChance.getValue());
    }
    public void RemoveModifier()
    {
        PlayerStat stat = PlayerManager.instance.player.GetComponent<PlayerStat>();
        stat.strength.RemoveModifier(strength.getValue());
        stat.agility.RemoveModifier(agility.getValue());
        stat.intelligence.RemoveModifier(intelligence.getValue());
        stat.endurance.RemoveModifier(endurance.getValue());

        stat.maxHealth.RemoveModifier(maxHealth.getValue());
        stat.armor.RemoveModifier(armor.getValue());
        stat.magicResistance.RemoveModifier(magicResistance.getValue());
        stat.avoidChance.RemoveModifier(avoidChance.getValue());

        stat.physicalDamage.RemoveModifier(physicalDamage.getValue());
        stat.fireDamage.RemoveModifier(fireDamage.getValue());
        stat.iceDamage.RemoveModifier(iceDamage.getValue());
        stat.lightDamage.RemoveModifier(lightDamage.getValue());
        stat.criticalChance.RemoveModifier(criticalChance.getValue());
    }

}
