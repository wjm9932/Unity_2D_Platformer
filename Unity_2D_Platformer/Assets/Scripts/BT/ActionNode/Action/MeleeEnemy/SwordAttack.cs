using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : IAction
{
    private Blackboard blackboard;
    private float decelerationFactor;
    private bool canAttack;
    public SwordAttack(Blackboard blackBoard)
    {
        this.blackboard = blackBoard;
        decelerationFactor = 0.2f;
    }

    public void OnEnter()
    {
        blackboard.GetData<Enemy>("owner").animHandler.animator.SetBool("IsAttack1", true);
        blackboard.GetData<Enemy>("owner").animHandler.ResetOneFrameDelay();
        canAttack = false;
    }

    public NodeState Execute()
    {
        if(canAttack == true)
        {
            Attack();
        }
        if(blackboard.GetData<Enemy>("owner").animHandler.IsAnimationFinishedWithDelay() == true)
        {
            return NodeState.Success;
        }

        return NodeState.Running;
    }

    public void ExecuteInFixedUpdate()
    {
        DeaccelPlayerVelocity();
    }
    public void OnExit()
    {
        blackboard.GetData<Enemy>("owner").animHandler.animator.SetBool("IsAttack1", false);
    }
    private void DeaccelPlayerVelocity()
    {
        float speedDiff = 0f - blackboard.GetData<Enemy>("owner").rb.velocity.x;
        float movement = speedDiff * ((1 / Time.fixedDeltaTime) * decelerationFactor);

        blackboard.GetData<Enemy>("owner").rb.AddForce(movement * Vector2.right);
    }

    private void Attack()
    {
        var target = Physics2D.OverlapCircle(blackboard.GetData<Enemy>("owner").attackRoot.position, blackboard.GetData<Enemy>("owner").attackRange, blackboard.GetData<Enemy>("owner").targetLayer);

        if(target != null)
        {
            target.GetComponent<LivingEntity>().ApplyDamage(1, blackboard.GetData<Enemy>("owner").gameObject);
        }
    }

    public void OnAnimationEnterEvent()
    {
        canAttack = true;
    }
    public void OnAnimationTransitionEvent()
    {

    }
    public void OnAnimationExitEvent()
    {
        canAttack = false;
    }
}
