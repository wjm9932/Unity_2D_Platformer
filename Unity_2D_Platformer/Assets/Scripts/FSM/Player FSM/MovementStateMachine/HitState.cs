using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : IState
{
    private PlayerMovementStateMachine sm;
    private readonly float duration;
    private readonly float decelerationFactor;
    private float timer;
    public HitState(PlayerMovementStateMachine playerMovementStateMachine)
    {
        sm = playerMovementStateMachine;
        decelerationFactor = 0.2f;
        duration = Utility.CalculateTimeByDashForce(sm.owner.movementType.knockbackForce.x, decelerationFactor);
    }
    public void Enter()
    {
        ApplyKnockbackForce(sm.owner.movementType.knockbackForce);
        timer = duration;

        if (sm.jsm.currentState == sm.jsm.slideState)
        {
            sm.jsm.ChangeState(sm.jsm.fallingState);
        }
        sm.owner.animHandler.animator.ResetTrigger("ResetCombo");
        sm.owner.animHandler.animator.SetBool("IsHit", true);
        SoundManager.Instance.PlaySoundEffect(SoundManager.InGameSoundEffectType.PLAYER_HIT, 0.2f);
    }
    public void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            sm.ChangeState(sm.runState);
        }
    }
    public void FixedUpdate()
    {
        if (sm.currentState != this)
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
        float speedDiff = 0f + sm.owner.platformVelocity.x - sm.owner.rb.velocity.x;
        float movement = speedDiff * ((1 / Time.fixedDeltaTime) * decelerationFactor);

        sm.owner.rb.AddForce(movement * Vector2.right);
    }

    private void ApplyKnockbackForce(Vector2 force)
    {
        Vector2 finalForce = Vector2.zero;

        if (Mathf.Abs(sm.owner.rb.velocity.y) > 0f)
        {
            finalForce.y -= sm.owner.rb.velocity.y;
        }

        if (Mathf.Abs(sm.owner.rb.velocity.x) > 0f)
        {
            finalForce.x -= sm.owner.rb.velocity.x;
        }
        force.x *= sm.owner.hitDir;
        finalForce += force;
        sm.owner.rb.AddForce(finalForce, ForceMode2D.Impulse);
    }
}
