using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackState : IState
{
    protected PlayerMovementStateMachine sm;
    protected bool canAttack;
    protected bool canComboAttack;
    protected float decelerationFactor;
    public AttackState(PlayerMovementStateMachine playerMovementStateMachine)
    {
        sm = playerMovementStateMachine;
        canAttack = false;
        canComboAttack = false;
        decelerationFactor = 0.2f;
    }
    public virtual void Enter()
    {
        canAttack = false;
        canComboAttack = false;
    }
    public virtual void Update()
    {
        if (sm.owner.animHandler.IsAnimationFinished() == true)
        {
            sm.owner.animHandler.animator.SetTrigger("ResetCombo");
            sm.ChangeState(sm.runState);
        }
    }
    public virtual void FixedUpdate()
    {

    }
    public virtual void LateUpdate()
    {

    }
    public virtual void Exit()
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
    protected void DeaccelPlayerVelocity()
    {
        float speedDiff = 0f - sm.owner.rb.velocity.x;
        float movement = speedDiff * ((1 / Time.fixedDeltaTime) * decelerationFactor);

        sm.owner.rb.AddForce(movement * Vector2.right);
    }

    protected bool Attack(bool isHardAttack)
    {
        bool isAttackSucced = false;
        var targetColliders = Physics2D.OverlapCircleAll(sm.owner.attackRoot.position, sm.owner.attackRange, sm.owner.targetLayer);

        foreach (var target in targetColliders)
        {
            if (target.transform.GetComponent<Enemy>() != null)
            {
                var enemy = target.transform.GetComponent<Enemy>();

                if (enemy.TakeDamage(sm.owner.dmg, sm.owner.gameObject) == true)
                {
                    isAttackSucced = true;

                    if(enemy.GetComponent<Boss>() != null)
                    {
                        enemy.GetComponent<Boss>().isHardAttack = isHardAttack;
                    }
                }
            }
            else if (target.transform.GetComponent<IInteractable>() != null)
            {
                var interactable = target.transform.GetComponent<IInteractable>();

                interactable.Trigger();
            }
        }

        return isAttackSucced;
    }
}