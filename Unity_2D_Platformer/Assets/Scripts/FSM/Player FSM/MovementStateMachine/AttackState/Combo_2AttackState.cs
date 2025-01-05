using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combo_2AttackState : AttackState
{
    private float dashForce;
    public Combo_2AttackState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        
    }
    public override void Enter()
    {
        base.Enter();
        dashForce = 50f;

        if (sm.owner.input.moveInput.x != 0)
        {
            FlipPlayer(sm.owner.input.moveInput.x > 0);
        }

        RaycastHit2D hit = Physics2D.Raycast(new Vector2(sm.owner.rb.position.x + (sm.owner.transform.right.x * (0.525f + 0.5f)), sm.owner.rb.position.y), sm.owner.transform.right, CalculateDashDistance(dashForce), sm.owner.enemyLayer | sm.owner.whatIsGround);
        if (hit.collider != null) 
        {
            dashForce = CalculateRequiredImpulse(hit.distance);
        }
        sm.owner.rb.AddForce(sm.owner.transform.right * dashForce, ForceMode2D.Impulse);

        sm.owner.animHandler.animator.SetTrigger("Combo_2");
    }
    public override void Update()
    {
        if (canAttack == true)
        {
            var enemies = Physics2D.OverlapCircleAll(sm.owner.attackRoot.position, sm.owner.attackRange, sm.owner.enemyLayer);

            for (int i = 0; i < enemies.Length; i++)
            {
                Debug.Log(enemies[i].name);
            }
        }


        if (sm.jsm.currentState == sm.jsm.fallingState)
        {
            sm.ChangeState(sm.runState);
        }
        else
        {
            base.Update();
        }
    }
    public override void FixedUpdate()
    {
        if (sm.currentState != this)
        {
            return;
        }

        DeaccelPlayerVelocity();
    }
    public override void LateUpdate()
    {

    }
    public override void Exit()
    {

    }
    public override void OnAnimationEnterEvent()
    {
        canAttack = true;
    }
    public override void OnAnimationExitEvent()
    {
    }
    public override void OnAnimationTransitionEvent()
    {
        canAttack = false;
    }


    private void FlipPlayer(bool isRight)
    {
        if (isRight == false)
        {
            sm.owner.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            sm.owner.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private float CalculateDashDistance(float dashForce)
    {
        float impulseForce = dashForce;

        float initialVelocity = impulseForce; 
        float velocity = initialVelocity;
        float totalDistance = 0f;

        while (velocity > 0.001f)
        {
            float decelerationForce = velocity * (1 / Time.fixedDeltaTime) * decelerationFactor;
            float accelerationDecel = decelerationForce; 
            velocity -= accelerationDecel * Time.fixedDeltaTime; 


            float distanceThisFrame = velocity * Time.fixedDeltaTime;
            totalDistance += distanceThisFrame;
        }

        return totalDistance;
    }

    private float CalculateRequiredImpulse(float distance)
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