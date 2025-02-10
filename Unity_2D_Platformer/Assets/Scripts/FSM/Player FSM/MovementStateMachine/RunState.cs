using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : IState
{
    private PlayerMovementStateMachine sm;
    private bool isFacingRight;

    #region Buffer Time
    #endregion

    public RunState(PlayerMovementStateMachine playerMovementStateMachine)
    {
        sm = playerMovementStateMachine;
    }
    public void Enter()
    {
        isFacingRight = sm.owner.transform.localRotation.y >= 0f;
    }

    public void Update()
    {
        if (sm.owner.input.moveInput.x != 0)
        {
            FlipPlayer(sm.owner.input.moveInput.x > 0);
        }

        if (sm.owner.input.isJump == true)
        {
            sm.owner.lastPressJumpTime = sm.owner.movementType.jumpInputBufferTime;
        }

        if (sm.owner.lastPressAttackTime > 0f && sm.jsm.currentState == sm.jsm.idleState)
        {
            sm.ChangeState(sm.combo_1AttackState);
        }

        if (sm.owner.lastPressDashTime > 0f && sm.jsm.currentState != sm.jsm.slideState)
        {
            sm.ChangeState(sm.dashState);
        }
    }
    public void FixedUpdate()
    {
        if (sm.currentState != this)
        {
            return;
        }

        Run(1f);

        if (sm.jsm.currentState != sm.jsm.slideState)
        {
        }
    }
    public void LateUpdate()
    {

    }
    public void Exit()
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

    private void FlipPlayer(bool isMovingRight)
    {
        if (isMovingRight != isFacingRight)
        {
            if (isMovingRight == false)
            {
                sm.owner.transform.localRotation = Quaternion.Euler(0, 180, 0);
                isFacingRight = false;
            }
            else
            {
                sm.owner.transform.localRotation = Quaternion.Euler(0, 0, 0);
                isFacingRight = true;
            }
        }
    }

    private void Run(float lerpAmount)
    {
        float targetSpeed = sm.owner.input.moveInput.x * sm.owner.movementType.runMaxSpeed;
        targetSpeed = Mathf.Lerp(sm.owner.rb.velocity.x, targetSpeed, lerpAmount);

        float accelAmount;

        if (sm.owner.lastOnGroundTime > 0f)
        {
            accelAmount = (Mathf.Abs(targetSpeed) > 0.01f) ? sm.owner.movementType.runAccelAmount : sm.owner.movementType.runDeccelAmount;
        }
        else
        {
            accelAmount = (Mathf.Abs(targetSpeed) > 0.01f) ? sm.owner.movementType.runAccelAmount * sm.owner.movementType.accelInAir : sm.owner.movementType.runDeccelAmount * sm.owner.movementType.deccelInAir;
        }

        //Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
        if ((sm.jsm.currentState == sm.jsm.jumpState || sm.jsm.currentState == sm.jsm.jumpFallingState) && Mathf.Abs(sm.owner.rb.velocity.y) < sm.owner.movementType.jumpHangVelocityThreshold)
        {
            accelAmount *= sm.owner.movementType.jumpHangAccelerationMult;
            targetSpeed *= sm.owner.movementType.jumpHangMaxSpeedMult;
        }

        if (Mathf.Abs(sm.owner.platformVelocity.x) > 0.01f && (Mathf.Abs(targetSpeed) < 0.01f || sm.jsm.currentState == sm.jsm.slideState))
        {
            accelAmount = 50f;
        }

        targetSpeed += sm.owner.platformVelocity.x;

        float speedDif = targetSpeed - sm.owner.rb.velocity.x;
        float movement = speedDif * accelAmount;

        sm.owner.rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }
}
