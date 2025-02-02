using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrack : IAction
{
    private Blackboard blackboard;

    public BossTrack(Blackboard blackBoard)
    {
        this.blackboard = blackBoard;
    }

    public void OnEnter()
    {
        blackboard.GetData<Enemy>("owner").animHandler.animator.SetBool("IsTrack", true);
    }

    public NodeState Execute()
    {
        Flip(blackboard.GetData<Enemy>("owner").transform.position.x < blackboard.GetData<Enemy>("owner").target.transform.position.x);

        if (blackboard.GetData<Enemy>("owner").target.isDead == true)
        {
            blackboard.GetData<Enemy>("owner").target = null;
            return NodeState.Failure;
        }
        if (Mathf.Abs(blackboard.GetData<Enemy>("owner").target.transform.position.x - blackboard.GetData<Enemy>("owner").transform.position.x) <= blackboard.GetData<Enemy>("owner").trackStopDistance)
        {
            return NodeState.Success;
        }

        return NodeState.Running;
    }

    public void ExecuteInFixedUpdate()
    {
        Run();
    }
    public void OnExit()
    {
        blackboard.GetData<Enemy>("owner").animHandler.animator.SetBool("IsTrack", false);
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
    private void Run()
    {
        float targetSpeed = blackboard.GetData<Enemy>("owner").transform.right.x * blackboard.GetData<Enemy>("owner").movementType.trackMaxSpeed;

        float speedDif = targetSpeed - blackboard.GetData<Enemy>("owner").rb.velocity.x;
        float accelAmount = (targetSpeed > Mathf.Abs(blackboard.GetData<Enemy>("owner").rb.velocity.x)) ? blackboard.GetData<Enemy>("owner").movementType.trackAccelAmount : blackboard.GetData<Enemy>("owner").movementType.trackDeccelAmount;
        float movement = speedDif * accelAmount;

        blackboard.GetData<Enemy>("owner").rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
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