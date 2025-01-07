using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDieState : IState
{
    private EnemyStateMachine sm;
    public EnemyDieState(EnemyStateMachine enemyStateMachine)
    {
        sm = enemyStateMachine;
    }
    public void Enter()
    {
        sm.owner.animHandler.animator.SetTrigger("Die");
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
