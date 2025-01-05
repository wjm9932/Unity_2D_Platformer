using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : IState
{
    private PlayerMovementStateMachine sm;
    private float decelerationFactor = 0.2f;
    public HitState(PlayerMovementStateMachine playerMovementStateMachine)
    {
        sm = playerMovementStateMachine;
    }
    public void Enter()
    {
        ApplyKnockbackForce(sm.owner.movementType.knockbackForce);
        sm.owner.animHandler.animator.SetBool("IsHit", true);
    }
    public void Update()
    {
        if(Mathf.Abs(sm.owner.rb.velocity.x) < 0.1f)
        {
            sm.ChangeState(sm.runState);
        }
    }
    public void FixedUpdate()
    {
        if(sm.currentState != this)
        {
            return;
        }
        DeaccelPlayerVelocity();
    }
    public void LateUpdate()
    {

    }
    public void Exit()
    {
        sm.owner.animHandler.animator.SetBool("IsHit", false);
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
    private void DeaccelPlayerVelocity()
    {
        float speedDiff = 0f - sm.owner.rb.velocity.x;
        float movement = speedDiff * ((1 / Time.fixedDeltaTime) * decelerationFactor);

        sm.owner.rb.AddForce(movement * Vector2.right);
    }

    private void ApplyKnockbackForce(Vector2 force)
    {
        float additionalForce = 0f;
        if (Mathf.Abs(sm.owner.rb.velocity.x) > 0f)
        {
            additionalForce = Mathf.Abs(sm.owner.rb.velocity.x);
        }
        force.x += additionalForce;
        force.x *= -sm.owner.transform.right.x;

        
        if(sm.jsm.currentState != sm.jsm.idleState)
        {
            force.y = 0f;
        }
        sm.owner.rb.AddForce(force, ForceMode2D.Impulse);
    }
}
