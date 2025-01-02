using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideState : IState, IGravityModifier
{
    private PlayerJumpStateMachine sm;
    private bool test;
    public SlideState(PlayerJumpStateMachine playerJumpStateMachine)
    {
        sm = playerJumpStateMachine;
    }
    public void Enter()
    {
        sm.owner.animHandler.isSlide = true;
        SetGravityScale();
    }
    public void Update()
    {
        if (Physics2D.OverlapBox(sm.owner.groundChecker.position, sm.owner.groundCheckSize, 0, sm.owner.whatIsGround) == true)
        {
            sm.owner.lastOnGroundTime = sm.owner.movementType.coyoteTime;
        }
        if (Physics2D.OverlapBox(sm.owner.wallCollisionChecker.position, sm.owner.wallCollisionCheckerSize, 0, sm.owner.whatIsGround))
        {
            sm.owner.lastOnWallTime = sm.owner.movementType.wallJumpCoyoteTime;
            if (sm.owner.input.moveInput.x != 0)
            {
                sm.facingDir = sm.owner.input.moveInput.x;
            }
        }

        if (sm.owner.lastOnWallTime > 0f && sm.owner.lastPressJumpTime > 0f)
        {
            sm.ChangeState(sm.wallJumpState);
            return;
        }
        else if (sm.facingDir != sm.owner.input.moveInput.x)
        {
            sm.ChangeState(sm.fallingState);
            return;
        }
        else if (sm.owner.lastOnGroundTime > 0f)
        {
            sm.ChangeState(sm.idleState);
            return;
        }
    }
    public void FixedUpdate()
    {
        if (sm.currentState != this)
        {
            return;
        }

        Slide();
    }
    public void LateUpdate()
    {

    }
    public void Exit()
    {
        sm.owner.animHandler.isSlide = false;

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
        sm.owner.SetGravityScale(0f);
    }

    private void Slide()
    {
        float speedDif = sm.owner.movementType.slideSpeed - sm.owner.rb.velocity.y;

        float movement = speedDif * sm.owner.movementType.slideAccelAmount;
        //So, we clamp the movement here to prevent any over corrections (these aren't noticeable in the Run)
        //The force applied can't be greater than the (negative) speedDifference * by how many times a second FixedUpdate() is called. For more info research how force are applied to rigidbodies.
        //movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));
        sm.owner.rb.AddForce(movement * Vector2.up);
    }
}
