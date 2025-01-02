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
        //SetGravityScale();
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
        else if (sm.owner.rb.velocity.y < 0 && sm.owner.lastOnGroundTime <= 0f)
        {
            sm.owner.SetGravityScale(sm.owner.movementType.gravityScale * sm.owner.movementType.fallGravityMult);
            sm.owner.rb.velocity = new Vector2(sm.owner.rb.velocity.x, Mathf.Max(sm.owner.rb.velocity.y, -sm.owner.movementType.maxFallSpeed));
        }
        else
        {
            sm.owner.SetGravityScale(sm.owner.movementType.gravityScale);
        }
    }
}
