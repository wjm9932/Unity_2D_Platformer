using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private PlayerMovementStateMachine sm;
    public AttackState(PlayerMovementStateMachine playerMovementStateMachine)
    {
        sm = playerMovementStateMachine;
    }
    public void Enter()
    {
        sm.owner.animHandler.isAttackTriggered = true;
    }
    public void Update()
    {
        if (sm.owner.animHandler.IsAnimationFinished() == true)
        {
            sm.ChangeState(sm.runState);
        }

        if(sm.jsm.currentState == sm.jsm.fallingState)
        {
            sm.ChangeState(sm.runState);
        }
    }
    public void FixedUpdate()
    {
        if (sm.currentState != this)
        {
            return;
        }

        SetPlayerTargetSpeed();
    }
    public void LateUpdate()
    {

    }
    public void Exit()
    {
    }
    public void OnAnimatorIK()
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

    private void SetPlayerTargetSpeed()
    {
        float speedDiff = 0f - sm.owner.rb.velocity.x;
        float movement = speedDiff * ((1 / Time.fixedDeltaTime) * 0.2f);

        sm.owner.rb.AddForce(movement * Vector2.right);
    }
}