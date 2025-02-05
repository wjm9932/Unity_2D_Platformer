using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : IState
{
    private PlayerMovementStateMachine sm;

    float decelerationFactor;
    float dashForce;
    public DashState(PlayerMovementStateMachine playerMovementStateMachine)
    {
        sm = playerMovementStateMachine;
        decelerationFactor = 0.2f;
    }
    public void Enter()
    {
        sm.owner.lastPressDashTime = 0f;

        if (sm.owner.dashCount <= 0)
        {
            sm.ChangeState(sm.runState);
            return;
        }

        sm.owner.isDashing = true;
        dashForce = sm.owner.movementType.dashForce;

        RaycastHit2D hit = Physics2D.Raycast(new Vector2(sm.owner.rb.position.x + (sm.owner.transform.right.x * 0.525f), sm.owner.rb.position.y), sm.owner.transform.right, CalculateDashDistanceByDashForce(dashForce), sm.owner.whatIsGround);
        if (hit.collider != null)
        {
            if (hit.distance < 0.5f)
            {
                sm.ChangeState(sm.runState);
                return;
            }

            dashForce = CalculateRequiredImpulseForDistance(hit.distance);
        }
        ApplyDashForce(dashForce);

        sm.owner.dashCount--;
        sm.owner.animHandler.animator.SetBool("IsDash", true);
        SoundManager.Instance.PlaySoundEffect(SoundManager.InGameSoundEffectType.PLAYER_DASH, 0.2f);
    }

    public void Update()
    {
        if (Mathf.Abs(sm.owner.rb.velocity.x) < 5f)
        {
            sm.ChangeState(sm.runState);
            return;
        }
        if (Mathf.Sign(sm.owner.rb.velocity.x) != Mathf.Sign(sm.owner.transform.right.x))
        {
            sm.ChangeState(sm.runState);
            return;
        }

    }
    public void FixedUpdate()
    {
        if (sm.owner.input.moveInput.y < 0)
        {
            DeaccelPlayerVelocity(0.6f);
            sm.ChangeState(sm.runState);
        }
        else if (sm.currentState != this)
        {
            return;
        }
        else
        {
            DeaccelPlayerVelocity(decelerationFactor);
        }
    }
    public void LateUpdate()
    {

    }
    public void Exit()
    {
        sm.owner.isDashing = false;
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
        sm.owner.SetGravityScale(0f);

        Vector2 finalForce = new Vector2(force, 0f);
        finalForce.x *= sm.owner.transform.right.x;

        if (Mathf.Abs(sm.owner.rb.velocity.y) > 0f)
        {
            finalForce.y -= sm.owner.rb.velocity.y;
        }

        if (Mathf.Abs(sm.owner.rb.velocity.x) > 0f)
        {
            finalForce.x -= sm.owner.rb.velocity.x;
        }
        sm.owner.rb.AddForce(finalForce, ForceMode2D.Impulse);
    }
    private void DeaccelPlayerVelocity(float decelerationFactor)
    {
        float speedDiff = 0f - sm.owner.rb.velocity.x;
        float movement = speedDiff * (1 / Time.fixedDeltaTime) * decelerationFactor;

        sm.owner.rb.AddForce(movement * Vector2.right);
    }

    private float CalculateDashDistanceByDashForce(float dashForce)
    {
        float impulseForce = dashForce;

        float velocity = impulseForce;
        float totalDistance = 0f;

        while (velocity > 0.1f)
        {
            float decelerationForce = velocity * (1 / Time.fixedDeltaTime) * decelerationFactor;
            float accelerationDecel = decelerationForce;
            velocity -= accelerationDecel * Time.fixedDeltaTime;


            float distanceThisFrame = velocity * Time.fixedDeltaTime;
            totalDistance += distanceThisFrame;
        }
        return totalDistance;
    }

    private float CalculateRequiredImpulseForDistance(float distance)
    {
        float obstacleDistance = distance;

        float velocity = 0f;
        float totalDistance = 0f;
        float requiredImpulse = 0f;
        float step = 0.1f;

        while (totalDistance < obstacleDistance)
        {
            totalDistance = 0f;
            velocity = requiredImpulse;

            while (velocity > 0.1f)
            {

                float decelerationForce = velocity * (1 / Time.fixedDeltaTime) * decelerationFactor;
                float accelerationDecel = decelerationForce;
                velocity -= accelerationDecel * Time.fixedDeltaTime;

                float distanceThisFrame = velocity * Time.fixedDeltaTime;
                totalDistance += distanceThisFrame;
            }

            if (totalDistance >= obstacleDistance)
            {
                break;
            }

            requiredImpulse += step;
        }

        return requiredImpulse;
    }
}