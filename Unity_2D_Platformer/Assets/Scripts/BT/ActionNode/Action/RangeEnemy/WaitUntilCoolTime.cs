using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitUntilCoolTime : IAction
{
    private Blackboard blackboard;

    public WaitUntilCoolTime(Blackboard blackBoard)
    {
        this.blackboard = blackBoard;
    }

    public void OnEnter()
    {
        blackboard.GetData<Enemy>("owner").animHandler.animator.SetBool("IsWait", true);
    }

    public NodeState Execute()
    {

        Flip(blackboard.GetData<Enemy>("owner").transform.position.x < blackboard.GetData<Enemy>("owner").target.transform.position.x);

        return NodeState.Running;
    }

    public void ExecuteInFixedUpdate()
    {
        DeaccelPlayerVelocity();
    }
    public void OnExit()
    {
        blackboard.GetData<Enemy>("owner").animHandler.animator.SetBool("IsWait", false);
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
    private void DeaccelPlayerVelocity()
    {
        float speedDiff = 0f - blackboard.GetData<Enemy>("owner").rb.velocity.x;
        float movement = speedDiff * blackboard.GetData<Enemy>("owner").movementType.patrolDeccelAmount;

        blackboard.GetData<Enemy>("owner").rb.AddForce(movement * Vector2.right);
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
