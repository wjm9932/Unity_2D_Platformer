using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : IState
{
    private EnemyStateMachine sm;
    private float idleStartTime;
    public EnemyIdleState(EnemyStateMachine enemyStateMachine)
    {
        sm = enemyStateMachine;
    }
    public void Enter()
    {
       idleStartTime = Time.time;
    }
    public void Update()
    {
        if(Time.time > idleStartTime + sm.owner.movementType.idleTime)
        {
            sm.ChangeState(sm.patrolState);
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
        float movement = speedDiff * sm.owner.movementType.runDeccelAmount;

        sm.owner.rb.AddForce(movement * Vector2.right);
    }
}
