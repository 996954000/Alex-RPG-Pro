using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimState : PlayerState
{
    private GameObject[] dots;
    public PlayerAimState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        dots = player.skill.skill_throwSword.GenDots();
    }

    public override void Exit()
    {
        base.Exit();
        foreach(GameObject dot in dots)
            GameObject.Destroy(dot);
    }

    public override void Update()
    {
        base.Update();
        player.skill.skill_throwSword.DotsPostion();
        if (Input.GetKeyUp(KeyCode.Z))
        {
            stateMachine.ChangeState(player.throwSwordState);
        }
    }
}
