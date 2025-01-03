using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : IState, IGravityModifier
{
    private PlayerJumpStateMachine sm;
    public JumpState(PlayerJumpStateMachine playerJumpStateMachine)
    {
        sm = playerJumpStateMachine;
    }
    public void Enter()
    {
        Jump();
        sm.isJumpCut = false;
        sm.owner.animHandler.isJumpTriggered = true;
    }
    public void Update()
    {
        if (sm.owner.rb.velocity.y < 0f)
        {
            sm.ChangeState(sm.jumpFallingState);
            return;
        }

        if (sm.owner.input.isJumpCut == true)
        {
            sm.isJumpCut = true;
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

    private void Jump()
    {
        //Ensures we can't call Jump multiple times from one press
        sm.owner.lastPressJumpTime = 0;
        sm.owner.lastOnGroundTime = 0;

        #region Perform Jump
        //We increase the force applied if we are falling
        //This means we'll always feel like we jump the same amount 
        //(setting the player's Y velocity to 0 beforehand will likely work the same, but I find this more elegant :D)
        float force = sm.owner.movementType.jumpForce;
        if (sm.owner.rb.velocity.y < 0)
            force -= sm.owner.rb.velocity.y;

        sm.owner.rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        #endregion
    }

    public void SetGravityScale()
    {
        if (sm.owner.input.moveInput.y < 0)
        {
            sm.owner.SetGravityScale(sm.owner.movementType.gravityScale * sm.owner.movementType.fastFallGravityMult);
            sm.owner.rb.velocity = new Vector2(sm.owner.rb.velocity.x, Mathf.Max(sm.owner.rb.velocity.y, -sm.owner.movementType.maxFastFallSpeed));
        }
        else if(sm.isJumpCut == true)
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
            sm.owner.SetGravityScale(sm.owner.movementType.gravityScale);
        }
    }
}
