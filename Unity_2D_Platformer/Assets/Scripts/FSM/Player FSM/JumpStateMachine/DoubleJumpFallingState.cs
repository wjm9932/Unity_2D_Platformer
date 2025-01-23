using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpFallingState : IState, IGravityModifier
{
    private PlayerJumpStateMachine sm;
    public DoubleJumpFallingState(PlayerJumpStateMachine playerJumpStateMachine)
    {
        sm = playerJumpStateMachine;
    }
    public void Enter()
    {
        //SetGravityScale();
    }
    public void Update()
    {
        if (sm.owner.lastOnGroundTime > 0f)
        {
            sm.ChangeState(sm.idleState);
            return;
        }
        else if (sm.owner.lastOnWallTime > 0f && sm.owner.lastPressJumpTime > 0f)
        {
            sm.ChangeState(sm.wallJumpState);
            return;
        }
        else if (sm.owner.facingDir == sm.owner.input.moveInput.x && sm.owner.lastOnWallTime - sm.owner.movementType.wallJumpCoyoteTime >= 0f && sm.msm.currentState != sm.msm.hitState)
        {
            sm.ChangeState(sm.slideState);
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
        else if (sm.owner.input.moveInput.y < 0)
        {
            sm.owner.SetGravityScale(sm.owner.movementType.gravityScale * sm.owner.movementType.fastFallGravityMult);
            sm.owner.rb.velocity = new Vector2(sm.owner.rb.velocity.x, Mathf.Max(sm.owner.rb.velocity.y, -sm.owner.movementType.maxFastFallSpeed));
        }
        else if (sm.isJumpCut == true)
        {
            sm.owner.SetGravityScale(sm.owner.movementType.gravityScale * sm.owner.movementType.jumpCutGravityMult);
            sm.owner.rb.velocity = new Vector2(sm.owner.rb.velocity.x, Mathf.Max(sm.owner.rb.velocity.y, -sm.owner.movementType.maxFallSpeed));
        }
        else if (Mathf.Abs(sm.owner.rb.velocity.y) < sm.owner.movementType.jumpHangVelocityThreshold)
        {
            sm.owner.SetGravityScale(sm.owner.movementType.gravityScale * sm.owner.movementType.jumpHangGravityMult);
        }
        else
        {
            sm.owner.SetGravityScale(sm.owner.movementType.gravityScale * sm.owner.movementType.fallGravityMult);
            sm.owner.rb.velocity = new Vector2(sm.owner.rb.velocity.x, Mathf.Max(sm.owner.rb.velocity.y, -sm.owner.movementType.maxFallSpeed));
        }
    }
}
