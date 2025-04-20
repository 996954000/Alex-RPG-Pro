

using UnityEngine;

public class EnemyStat : EntityStat
{
    [Range(1, 25)]
    [SerializeField] public int level;

    [Range(0.0f, 5.0f)]
    [SerializeField] public float modifierRate;
    public override void DoDamage(Entity _attacker)
    {
        // 伤害避免判断
        if (IsAvoidAttack())
        {
            Debug.Log("aVoid");
            return;
        }
        else
        {
            Player playerAttacker = _attacker as Player;
            GetComponent<Enemy>().DamagedByAttackPhase(playerAttacker.groundAttackPhase);
            base.DoDamage(_attacker);
        }
    }

    protected override void Start()
    {
        physicalDamage.setValue(Modifier(physicalDamage));
        maxHealth.setValue(Modifier(maxHealth));
        armor.setValue(Modifier(armor));

        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    /* 优化下逻辑的话就是每个stat单独使用一个rate */
    private float Modifier(Stat _stat)
    {
        return _stat.getValue() + level * modifierRate;
    }
}
