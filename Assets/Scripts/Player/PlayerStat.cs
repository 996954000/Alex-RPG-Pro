using UnityEngine;
public class PlayerStat : EntityStat
{
    public override void DoDamage(Entity _attacker)
    {
        // …À∫¶±‹√‚≈–∂œ
        if (IsAvoidAttack())
            return;
        else
        {
            //  ‹ª˜–ßπ˚
            GetComponent<Player>().DamagedByAttackPhase(0);

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
