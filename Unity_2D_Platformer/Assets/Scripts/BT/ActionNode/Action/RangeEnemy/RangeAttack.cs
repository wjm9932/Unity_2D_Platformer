using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : IAction
{
    private Blackboard blackboard;
    private float decelerationFactor;
    private bool isDone;
    public RangeAttack(Blackboard blackBoard)
    {
        this.blackboard = blackBoard;
        decelerationFactor = 0.2f;
    }

    public void OnEnter()
    {
        isDone = false;
        blackboard.GetData<Enemy>("owner").animHandler.animator.SetBool("IsAttack1", true);
        Flip(blackboard.GetData<Enemy>("owner").transform.position.x < blackboard.GetData<Enemy>("owner").target.transform.position.x);
    }

    public NodeState Execute()
    {
        if (isDone == true)
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

    public void OnAnimationEnterEvent()
    {
        var arrow = ObjectPoolManager.Instance.GetPoolableObject(blackboard.GetData<GameObject>("arrow"), blackboard.GetData<Enemy>("owner").attackRoot.position, blackboard.GetData<Enemy>("owner").transform.rotation).GetComponent<Projectile>();
        arrow.SetTargetDistanceAndVelocity(25f, 25f);
        blackboard.SetData<float>("attackCoolTime", 0f);
    }
    public void OnAnimationTransitionEvent()
    {

    }
    public void OnAnimationExitEvent()
    {
        isDone = true;
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