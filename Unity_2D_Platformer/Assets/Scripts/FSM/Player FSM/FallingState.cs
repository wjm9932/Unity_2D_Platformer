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
        #region Collision Check
        if (Physics2D.OverlapBox(sm.owner.groundChecker.position, sm.owner.groundCheckSize, 0, sm.owner.whatIsGround) == true) //checks if set box overlaps with ground
        {
            sm.owner.lastOnGroundTime = sm.owner.movementType.coyoteTime; //if so sets the lastGrounded to coyoteTime
        }

        if (Physics2D.OverlapBox(sm.owner.wallCollisionChecker.position, sm.owner.wallCollisionCheckerSize, 0, sm.owner.whatIsGround))
        {
            sm.owner.lastOnWallTime = sm.owner.movementType.wallJumpCoyoteTime;
            if (sm.owner.input.moveInput.x != 0)
            {
                sm.facingDir = sm.owner.input.moveInput.x;
            }
        }
        #endregion

        if (sm.owner.lastOnWallTime > 0f && sm.owner.lastPressJumpTime > 0f)
        {
            sm.ChangeState(sm.wallJumpState);
            return;
        }
        else if (sm.facingDir == sm.owner.input.moveInput.x && sm.owner.lastOnWallTime > 0f)
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

    public void SetGravityScale()
    {
        if (sm.owner.rb.velocity.y < 0 && sm.owner.input.moveInput.y < 0)
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