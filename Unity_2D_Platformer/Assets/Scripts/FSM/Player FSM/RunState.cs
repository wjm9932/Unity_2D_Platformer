using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : IState
{
    private PlayerMovementStateMachine sm;
    private float moveSpeed;
    public RunState(PlayerMovementStateMachine playerMovementStateMachine)
    {
        sm = playerMovementStateMachine;
        moveSpeed = 4f;
    }
    public void Enter()
    {
        sm.owner.animator.SetBool("IsRun", true);

        sm.owner.FlipPlayer(sm.owner.input.tempmoveInput < 0);
    }
    public void Update()
    {
        if (Mathf.Abs(sm.owner.input.tempmoveInput) <= Mathf.Epsilon)
        {
            sm.ChangeState(sm.idleState);
        }

        if (sm.owner.input.isJump == true && sm.owner.IsOnGround() == true)
        {
            sm.ChangeState(sm.jumpState);
        }
    }
    public void FixedUpdate()
    {
        sm.owner.rb.velocity = new Vector2(sm.owner.input.tempmoveInput * moveSpeed, sm.owner.rb.velocity.y);
    }
    public void LateUpdate()
    {

    }
    public void Exit()
    {
        sm.owner.animator.SetBool("IsRun", false);
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
