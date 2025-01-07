using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    private EnemyStateMachine sm;

    private float targetPositionX;
    private float stopDistance;
    public PatrolState(EnemyStateMachine enemyStateMachine)
    {
        sm = enemyStateMachine;
        stopDistance = sm.owner.stopDistance;
    }
    public void Enter()
    {
        targetPositionX = Random.Range(sm.owner.patrolPoint_1, sm.owner.patrolPoint_2);
        Flip(targetPositionX > sm.owner.transform.position.x);

        sm.owner.spriteRenderer.color = Color.white;
        sm.owner.animHandler.animator.SetBool("IsPatrol", true);
    }
    public void Update()
    {
        if(Mathf.Abs(sm.owner.transform.position.x - targetPositionX) <= stopDistance)
        {
            sm.ChangeState(sm.enemyIdleState);
        }
    }
    public void FixedUpdate()
    {
        Run();
    }
    public void LateUpdate()
    {

    }
    public void Exit()
    {
        sm.owner.animHandler.animator.SetBool("IsPatrol", false);
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

    private void Run()
    {
        float targetSpeed = sm.owner.transform.right.x * sm.owner.movementType.patrolMaxSpeed;

        float accelAmount = sm.owner.movementType.patrolAccelAmount;

        float speedDif = targetSpeed - sm.owner.rb.velocity.x;
        float movement = speedDif * accelAmount;

        sm.owner.rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    private void Flip(bool isMovingRight)
    {
        if (isMovingRight == false)
        {
            sm.owner.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            sm.owner.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}