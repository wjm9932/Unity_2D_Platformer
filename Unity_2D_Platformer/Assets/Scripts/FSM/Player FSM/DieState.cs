using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : IState
{
    private PlayerMovementStateMachine sm;
    public DieState(PlayerMovementStateMachine PlayerMovementStateMachine)
    {
        sm = PlayerMovementStateMachine;
    }
    public void Enter()
    {
        sm.owner.rb.velocity = Vector2.zero;
        sm.jsm.ChangeState(sm.jsm.idleState);
        sm.owner.animHandler.animator.SetBool("IsDie", true);
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
        sm.owner.animHandler.animator.SetBool("IsDie", false);
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
