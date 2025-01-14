using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : IAction
{
    private Blackboard blackboard;
    private float decelerationFactor;
    public RangeAttack(Blackboard blackBoard)
    {
        this.blackboard = blackBoard;
        decelerationFactor = 0.2f;
    }

    public void OnEnter()
    {
        blackboard.GetData<Enemy>("owner").animHandler.animator.SetBool("IsAttack1", true);
    }

    public NodeState Execute()
    {
        if (blackboard.GetData<Enemy>("owner").animHandler.IsAnimationFinished() == true)
        {
            blackboard.SetData<float>("attackCoolTime", 0f);
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

    public void OnAnimationEnterEvent()
    {
        // Instance arrow or something
    }
    public void OnAnimationTransitionEvent()
    {

    }
    public void OnAnimationExitEvent()
    {

    }
}