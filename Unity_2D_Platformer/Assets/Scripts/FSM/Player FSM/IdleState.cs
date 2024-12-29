using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private PlayerMovementStateMachine sm;
    private bool isFalling;
    public IdleState(PlayerMovementStateMachine playerMovementStateMachine)
    {
        sm = playerMovementStateMachine;
    }
    public void Enter()
    {
        if (!(Mathf.Abs(sm.owner.rb.velocity.y) > 0))
        {
            sm.owner.rb.velocity = Vector2.zero;
            isFalling = false;
        }
        else
        {
            isFalling = true;
        }
    }
    public void Update()
    {
        if (Mathf.Abs(sm.owner.input.moveInput) > Mathf.Epsilon)
        {
            sm.ChangeState(sm.runState);
        }

        if(sm.owner.input.isPressingJump == true)
        {
            sm.ChangeState(sm.jumpState);
        }

        if (isFalling == true)
        {
            if (Mathf.Abs(sm.owner.rb.velocity.y) <= Mathf.Epsilon)
            {
                sm.owner.rb.velocity = Vector2.zero;
                isFalling = false;
            }
        }
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
}
