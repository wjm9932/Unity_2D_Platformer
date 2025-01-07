using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitState : IState
{
    private EnemyStateMachine sm;
    private readonly float duration;
    private readonly float decelerationFactor;
    private float timer;
    public EnemyHitState(EnemyStateMachine enemyStateMachine)
    {
        sm = enemyStateMachine;
        decelerationFactor = 0.2f;
        duration = Utility.CalculateTimeByDashForce(sm.owner.movementType.knockbackForce.x, decelerationFactor);
    }
    public void Enter()
    {
        ApplyKnockbackForce(sm.owner.movementType.knockbackForce);
        timer = duration;
    }
    public void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            sm.ChangeState(sm.patrolState);
        }
    }
    public void FixedUpdate()
    {
        if(sm.currentState != this)
        {
            return;
        }

        DeaccelEnemyVelocity();
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
    private void DeaccelEnemyVelocity()
    {
        float speedDiff = 0f - sm.owner.rb.velocity.x;
        float movement = speedDiff * ((1 / Time.fixedDeltaTime) * decelerationFactor);

        sm.owner.rb.AddForce(movement * Vector2.right);
    }
    private void ApplyKnockbackForce(Vector2 force)
    {

        if (Mathf.Abs(sm.owner.rb.velocity.y) > 0f)
        {
            force.y -= sm.owner.rb.velocity.y;
        }

        force.x *= -(sm.owner.transform.right.x * sm.owner.hitDir);
        if (Mathf.Abs(sm.owner.rb.velocity.x) > 0f)
        {
            force.x -= sm.owner.rb.velocity.x;
        }

        sm.owner.rb.AddForce(force, ForceMode2D.Impulse);
    }
}

