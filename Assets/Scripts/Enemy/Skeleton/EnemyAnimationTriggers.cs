using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationTriggers : MonoBehaviour
{
    public Enemy_Skeleton enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy_Skeleton>();
    }

    public void AnimationFinished()
    {
        enemy.AnimationFinished();
    }

    public void DamageTrigger()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(enemy.attackTrans.position, enemy.attackRadius);
        foreach (var hit in collider2Ds)
        {
            if (hit.GetComponent<Player>() != null)
            {
                //Debug.Log("Skeleton Attack !!!!!!!!!!!!!!!!!!!!!!!!");
                
                hit.GetComponent<Player>().entityStat.DoDamage(enemy);
            }
        }
    }
    
    public void OpenCounterAttackWindow() => enemy.OpenCounterAttackWindow();

    public void CloseCounterAttackWindow() => enemy.CloseCounterAttackWindow();

    public void DeathTrigger()
    {
        enemy.animator.speed = 0;
        enemy.Death();
    }

}
