using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combo_2AttackState : AttackState
{
    private float dashForce;
    private float stopDistance;
    private bool canChargeDash;
    public Combo_2AttackState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        stopDistance = 0.5f;
    }
    public override void Enter()
    {
        base.Enter();
        canChargeDash = true;
        dashForce = 50f;

        if (sm.owner.input.moveInput.x != 0)
        {
            FlipPlayer(sm.owner.input.moveInput.x > 0);
        }

        RaycastHit2D hit = Physics2D.Raycast(new Vector2(sm.owner.rb.position.x + (sm.owner.transform.right.x * 0.525f), sm.owner.rb.position.y), sm.owner.transform.right, CalculateDashDistanceByDashForce(dashForce) + stopDistance, sm.owner.enemyLayer | sm.owner.whatIsGround);
        if (hit.collider != null) 
        {
            dashForce = CalculateRequiredImpulseForDistance(hit.distance - stopDistance);
        }
        sm.owner.rb.AddForce(sm.owner.transform.right * dashForce, ForceMode2D.Impulse);

        sm.owner.dmg = 20f;
        sm.owner.animHandler.animator.SetTrigger("Combo_2");
    }
    public override void Update()
    {
        if (canAttack == true)
        {
            var enemies = Physics2D.OverlapCircleAll(sm.owner.attackRoot.position, sm.owner.attackRange, sm.owner.enemyLayer);
            if(enemies.Length > 0 && canChargeDash == true)
            {
                sm.owner.dashCount++;
                canChargeDash = false;
            }
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].transform.root.GetComponent<LivingEntity>().ApplyDamage(sm.owner.dmg, sm.owner.gameObject);
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

    private float CalculateDashDistanceByDashForce(float dashForce)
    {
        float impulseForce = dashForce;

        float time = 0f;

        float velocity = impulseForce;
        float totalDistance = 0f;

        while (velocity > 0.1f)
        {
            float decelerationForce = velocity * (1 / Time.fixedDeltaTime) * decelerationFactor;
            float accelerationDecel = decelerationForce; 
            velocity -= accelerationDecel * Time.fixedDeltaTime; 


            float distanceThisFrame = velocity * Time.fixedDeltaTime;
            totalDistance += distanceThisFrame;

            time += Time.fixedDeltaTime;
        }
        return totalDistance;
    }

    private float CalculateRequiredImpulseForDistance(float distance)
    {
        float obstacleDistance = distance;

        float time = 0f;

        float velocity = 0f;
        float totalDistance = 0f;
        float requiredImpulse = 0f; 
        float step = 0.1f; 

        while (totalDistance < obstacleDistance)
        {
            time = 0f;
            totalDistance = 0f;
            velocity = requiredImpulse;

            while (velocity > 0.1f)
            {
                
                float decelerationForce = velocity * (1 / Time.fixedDeltaTime) * decelerationFactor;
                float accelerationDecel = decelerationForce;
                velocity -= accelerationDecel * Time.fixedDeltaTime;

                float distanceThisFrame = velocity * Time.fixedDeltaTime;
                totalDistance += distanceThisFrame;

                time += Time.fixedDeltaTime;
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