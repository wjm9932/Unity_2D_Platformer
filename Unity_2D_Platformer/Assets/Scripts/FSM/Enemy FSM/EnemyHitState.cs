using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitState : IState
{
    private EnemyStateMachine sm;
    public EnemyHitState(EnemyStateMachine enemyStateMachine)
    {
        sm = enemyStateMachine;
    }
    public void Enter()
    {

    }
    public void Update()
    {

    }
    public void FixedUpdate()
    {

    }
    public void LateUpdate()
    {

    }
    public void Exit()
    {
    }

    public virtual void OnAnimationEnterEvent()
    {

    }
    public virtual void OnAnimationExitEvent()
    {
    }
    public virtual void OnAnimationTransitionEvent()
    {
    }
}

