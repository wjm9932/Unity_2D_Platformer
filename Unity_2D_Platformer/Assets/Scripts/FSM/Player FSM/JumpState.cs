using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : IState
{
    private PlayerMovementStateMachine sm;
    private float jumpAmount;
    public JumpState(PlayerMovementStateMachine playerMovementStateMachine)
    {
        sm = playerMovementStateMachine;
        jumpAmount = 10f;
    }
    public void Enter()
    {
        sm.owner.animator.SetTrigger("IsJump");

        sm.owner.rb.AddForce(Vector2.up * jumpAmount, ForceMode2D.Impulse);

        sm.ChangeState(sm.idleState);
    }
    public void Update()
    {

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
