using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : IState, IGravityModifier
{
    private PlayerJumpStateMachine sm;
    public FallingState(PlayerJumpStateMachine playerJumpStateMachine)
    {
        sm = playerJumpStateMachine;
    }
    public void Enter()
    {
    }
    public void Update()
    {
        if (sm.owner.lastOnWallTime > 0f && sm.owner.lastPressJumpTime > 0f)
        {
            sm.ChangeState(sm.wallJumpState);
            return;
        }
        else if (sm.owner.facingDir == sm.owner.input.moveInput.x && sm.owner.lastOnWallTime - sm.owner.movementType.wallJumpCoyoteTime >= 0f && sm.msm.currentState != sm.msm.hitState)
        {
            sm.ChangeState(sm.slideState);
            return;
        }
        else if (sm.owner.lastOnGroundTime > 0f)
        {
            sm.ChangeState(sm.idleState);
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
        if (sm.owner.input.moveInput.y < 0)
        {
            sm.owner.SetGravityScale(sm.owner.movementType.gravityScale * sm.owner.movementType.fastFallGravityMult);
            sm.owner.rb.velocity = new Vector2(sm.owner.rb.velocity.x, Mathf.Max(sm.owner.rb.velocity.y, -sm.owner.movementType.maxFastFallSpeed));
        }
        else
        {
            sm.owner.SetGravityScale(sm.owner.movementType.gravityScale * sm.owner.movementType.fallGravityMult);
            sm.owner.rb.velocity = new Vector2(sm.owner.rb.velocity.x, Mathf.Max(sm.owner.rb.velocity.y, -sm.owner.movementType.maxFallSpeed));
        }
    }
}