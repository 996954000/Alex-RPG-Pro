using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMeachine
{
    public EnemyState currentState;
    public EnemyState previousState;
    public void changeState(EnemyState _newState)
    {
        previousState = currentState;
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }

    public void initState(EnemyState _initState)
    {
        currentState = _initState;
        currentState.Enter();
    }
}
