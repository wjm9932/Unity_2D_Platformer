using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackState : IState
{
    protected PlayerMovementStateMachine sm;
    protected bool canAttack;
    protected bool canComboAttack;
    public AttackState(PlayerMovementStateMachine playerMovementStateMachine)
    {
        sm = playerMovementStateMachine;
        canComboAttack = false;
    }
    public virtual void Enter()
    {
        canAttack = false;
        canComboAttack = false;
    }
    public virtual void Update()
    {
        if (sm.owner.animHandler.IsAnimationFinished() == true)
        {
            sm.owner.animHandler.animator.SetTrigger("ResetCombo");
            sm.ChangeState(sm.runState);
        }
    }
    public virtual void FixedUpdate()
    {

    }
    public virtual void LateUpdate()
    {

    }
    public virtual void Exit()
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