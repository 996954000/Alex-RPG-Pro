using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Clone : Skill
{
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float duration;

    [Header("Advanced Info")]
    [SerializeField] private bool canAttack;
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void SkillAction()
    {
        base.SkillAction();
        GameObject clone = GameObject.Instantiate(clonePrefab);
        clone.GetComponentInChildren<Skill_Clone_Controller>().SetupClone(player.transform.position, duration, canAttack, player.faceRight);
    }

    public override void UseSkill()
    {
        base.UseSkill();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        Debug.Log(gameObject.name + "::" + player.transform.position);
    }

    private void Awake()
    {

    }
}
