using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using static UnityEngine.UI.ScrollRect;

public class IdleState : IState, IGravityModifier
{
    private PlayerJumpStateMachine sm;
    public IdleState(PlayerJumpStateMachine playerJumpStateMachine)
    {
        sm = playerJumpStateMachine;
    }
    public void Enter()
    {

        sm.isJumpCut = false;
    }
    public void Update()
    {
        if (sm.owner.lastPressJumpTime > 0f && sm.owner.lastOnGroundTime > 0f)
        {
            sm.ChangeState(sm.jumpState);
            return;
        }
        else if (sm.owner.rb.velocity.y < 0f && sm.owner.lastOnGroundTime <= 0f)
        {
            sm.ChangeState(sm.fallingState);
            return;
        }

        SetGravityScale();

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
    public void SetGravityScale()
    {
        if (sm.msm.currentState == sm.msm.dashState)
        {
            sm.owner.SetGravityScale(0f);
        }
        else
        {
            sm.owner.SetGravityScale(sm.owner.movementType.gravityScale);
        }
    }
}
