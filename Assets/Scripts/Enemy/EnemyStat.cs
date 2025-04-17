

using UnityEngine;

public class EnemyStat : EntityStat
{
    public override void DoDamage(Entity _attacker)
    {
        // …À∫¶±‹√‚≈–∂œ
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
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}
