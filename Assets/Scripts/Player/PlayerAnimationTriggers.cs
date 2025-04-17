using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour {
    public Player player;
    private Enemy enemy;
    public void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    public void AnimationFinishedTrigger() => player.AnimationFinishedTrigger();

    public void DamageTrigger()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(player.groundAttackTrans.position, player.groundAttackRadius[player.groundAttackPhase]);
        foreach(var hit in collider2Ds)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                enemy = hit.GetComponent<Enemy>();
                /* 不同的攻击阶段对应不同的受伤效果 */
                //enemy.DamagedByAttackPhase(player.primaryAttackState.GetGroundAttackPhase());
                
                
                /* enemy损失生命值，攻击者为player */
                hit.GetComponent<Enemy>().entityStat.DoDamage(player);
            }
        }
    }
}
