using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : IState
{
    private PlayerMovementStateMachine sm;

    float startTime;
    float decelerationFactor;
    public DashState(PlayerMovementStateMachine playerMovementStateMachine)
    {
        sm = playerMovementStateMachine;
        decelerationFactor = 0.2f;
    }
    public void Enter()
    {
        startTime = Time.time;
        ApplyDashForce(sm.owner.movementType.dashForce);
        sm.owner.GetComponent<PlatformEffector2D>().colliderMask &= ~(sm.owner.enemyLayer.value);
        sm.owner.animHandler.animator.SetBool("IsDash", true);
    }

    public void Update()
    {
        
        if (Mathf.Abs(sm.owner.rb.velocity.x) < 5f)
        {
            sm.ChangeState(sm.runState);
        }
    }
    public void FixedUpdate()
    {
        DeaccelPlayerVelocity();
    }
    public void LateUpdate()
    {

    }
    public void Exit()
    {
        sm.owner.GetComponent<PlatformEffector2D>().colliderMask |= sm.owner.enemyLayer.value;
        sm.owner.animHandler.animator.SetBool("IsDash", false);
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

    private void ApplyDashForce(float force)
    {
        Vector2 finalForce = new Vector2(force, 0f);
        finalForce.x *= sm.owner.transform.right.x;
        
        if(Mathf.Abs(sm.owner.rb.velocity.y) > 0f)
        {
            finalForce.y -= sm.owner.rb.velocity.y;
        }

        if(Mathf.Abs(sm.owner.rb.velocity.x) > 0f)
        {
            finalForce.x -= sm.owner.rb.velocity.x;
        }

        sm.owner.rb.AddForce(finalForce, ForceMode2D.Impulse);
    }
    private void DeaccelPlayerVelocity()
    {
        float speedDiff = 0f - sm.owner.rb.velocity.x;
        float movement = speedDiff * ((1 / Time.fixedDeltaTime) * decelerationFactor);

        sm.owner.rb.AddForce(movement * Vector2.right);
    }
}