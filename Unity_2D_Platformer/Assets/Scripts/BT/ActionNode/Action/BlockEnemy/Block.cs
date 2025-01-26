using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Block : IAction, ICompositionNodeResettable
{
    private Blackboard blackboard;
    private float decelerationFactor;

    public Action<int> onResetCompositionNode { private get; set; }
    public int parentCompositionNodeIndex { private get; set; }

    public Block(Blackboard blackBoard)
    {
        this.blackboard = blackBoard;
        decelerationFactor = 0.2f;
    }

    public void OnEnter()
    {
        onResetCompositionNode(parentCompositionNodeIndex);

        ApplyKnockbackForce(blackboard.GetData<Enemy>("owner").movementType.knockbackForce / 5f);
        blackboard.GetData<Enemy>("owner").animHandler.animator.SetBool("IsBlock", true);
    }

    public NodeState Execute()
    {
        if (blackboard.GetData<Enemy>("owner").target != null)
        {
            Flip(blackboard.GetData<Enemy>("owner").transform.position.x < blackboard.GetData<Enemy>("owner").target.transform.position.x);
        }

        if (blackboard.GetData<Enemy>("owner").canBeDamaged == true)
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
        blackboard.GetData<Enemy>("owner").spriteRenderer.color = blackboard.GetData<Enemy>("owner").rageColor;
        blackboard.GetData<Enemy>("owner").animHandler.animator.SetBool("IsBlock", false);
    }
    private void DeaccelPlayerVelocity()
    {
        float speedDiff = 0f - blackboard.GetData<Enemy>("owner").rb.velocity.x;
        float movement = speedDiff * ((1 / Time.fixedDeltaTime) * decelerationFactor);

        blackboard.GetData<Enemy>("owner").rb.AddForce(movement * Vector2.right);
    }
    private void ApplyKnockbackForce(Vector2 force)
    {

        if (Mathf.Abs(blackboard.GetData<Enemy>("owner").rb.velocity.y) > 0f)
        {
            force.y -= blackboard.GetData<Enemy>("owner").rb.velocity.y;
        }

        if (Mathf.Abs(blackboard.GetData<Enemy>("owner").rb.velocity.x) > 0f)
        {
            force.x -= blackboard.GetData<Enemy>("owner").rb.velocity.x;
        }
        force.x *= blackboard.GetData<Enemy>("owner").hitDir;
        blackboard.GetData<Enemy>("owner").rb.AddForce(force, ForceMode2D.Impulse);
    }
    public void SetResetAction(Action<int> resetAction, int parentCompositionNodeIndex)
    {
        this.onResetCompositionNode = resetAction;
        this.parentCompositionNodeIndex = parentCompositionNodeIndex;
    }

    public void OnAnimationEnterEvent()
    {

    }
    public void OnAnimationTransitionEvent()
    {

    }
    public void OnAnimationExitEvent()
    {

    }

    private void Flip(bool isMovingRight)
    {
        if (isMovingRight && blackboard.GetData<Enemy>("owner").transform.localRotation.eulerAngles.y != 0)
        {
            blackboard.GetData<Enemy>("owner").transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (!isMovingRight && blackboard.GetData<Enemy>("owner").transform.localRotation.eulerAngles.y != 180)
        {
            blackboard.GetData<Enemy>("owner").transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
