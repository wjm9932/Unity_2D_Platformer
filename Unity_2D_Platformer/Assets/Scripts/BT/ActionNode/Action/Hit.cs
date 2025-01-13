using System;
using System.Collections.Generic;
using UnityEngine;

public class Hit : IAction, ICompositionNodeResettable
{
    private Action onResetCompositionNode;

    private Blackboard blackboard;
    private readonly float duration;
    private readonly float decelerationFactor;
    private float timer;

    public Hit(Blackboard blackBoard)
    {
        this.blackboard = blackBoard;
        decelerationFactor = 0.2f;
        duration = Utility.CalculateTimeByDashForce(blackboard.GetData<Enemy>("owner").movementType.knockbackForce.x, decelerationFactor);
    }

    public void OnEnter()
    {
        onResetCompositionNode();
        blackboard.GetData<Enemy>("owner").healthBar.gameObject.SetActive(true);

        ApplyKnockbackForce(blackboard.GetData<Enemy>("owner").movementType.knockbackForce);
        timer = duration;

        blackboard.GetData<Enemy>("owner").spriteRenderer.color = blackboard.GetData<Enemy>("owner").rageColor;
        blackboard.GetData<Enemy>("owner").animHandler.animator.SetBool("IsHit", true);
    }

    public NodeState Execute()
    {
        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            blackboard.SetData<bool>("isHit", false);
            return NodeState.Success;
        }

        return NodeState.Running;
    }

    public void ExecuteInFixedUpdate()
    {
        DeaccelEnemyVelocity();
    }
    public void OnExit()
    {
        blackboard.SetData<bool>("isHit", false);
        blackboard.GetData<Enemy>("owner").animHandler.animator.SetBool("IsHit", false);
    }

    private void DeaccelEnemyVelocity()
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
    public void SetResetAction(Action resetAction)
    {
        this.onResetCompositionNode = resetAction;
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
}