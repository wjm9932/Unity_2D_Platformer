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

        RaycastHit2D hit = Physics2D.Raycast(new Vector2(sm.owner.rb.position.x + (sm.owner.transform.right.x * 0.525f), sm.owner.rb.position.y), sm.owner.transform.right, Utility.CalculateDashDistanceByDashForce(dashForce, decelerationFactor) + stopDistance, sm.owner.enemyLayer | sm.owner.whatIsGround);
        if (hit.collider != null) 
        {
            dashForce = Utility.CalculateRequiredImpulseForDistance(hit.distance - stopDistance, decelerationFactor);
        }
        sm.owner.rb.AddForce(sm.owner.transform.right * dashForce, ForceMode2D.Impulse);

        sm.owner.dmg = 20f;
        sm.owner.animHandler.animator.SetTrigger("Combo_2");
    }
    public override void Update()
    {
        if (canAttack == true)
        {
            if(Attack() == true && canChargeDash == true)
            {
                sm.owner.dashCount++;
                canChargeDash = false;
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
        base.Exit();
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

    
}