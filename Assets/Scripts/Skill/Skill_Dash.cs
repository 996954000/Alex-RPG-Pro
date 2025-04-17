using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Dash : Skill
{
    [Header("Unique attributes")]
    [SerializeField] protected float dashDuration;
    [SerializeField] protected float dashSpeed;
    private float dashDurationTimer;
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        dashDurationTimer -= Time.deltaTime;
        
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
    public override void UseSkill()
    {
        base.UseSkill();
        /* 更新冲刺持续时间计时器 */
        dashDurationTimer = dashDuration;
    }

    public override void SkillAction()
    {
        base.SkillAction();
        Debug.Log("SKILL::DASH::ACTION");
        if (dashDurationTimer > 0)
        {
            player.rigidbody2D.velocity = new Vector2(player.faceDir.x * player.dashSpeed,
                0.0f);
        }
    }

    /* 冲刺过程中检测 */
    public bool IsDashing()
    {
        if (dashDurationTimer > 0)
        {
            return true;
        }
        return false;
    }

}
