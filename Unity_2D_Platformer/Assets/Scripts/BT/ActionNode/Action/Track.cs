using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : IAction
{
    private Blackboard blackboard;

    public Track(Blackboard blackBoard)
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

        if (IsTargetOnWayPoints() == false)
        {
            blackboard.GetData<Enemy>("owner").target = null;
            return NodeState.Failure;
        }
        if(Vector2.Distance(blackboard.GetData<Enemy>("owner").target.transform.position, blackboard.GetData<Enemy>("owner").transform.position) <= blackboard.GetData<Enemy>("owner").trackStopDistance)
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

    private bool IsTargetOnWayPoints()
    {
        float minX = Mathf.Min(blackboard.GetData<Enemy>("owner").patrolPoint_1, blackboard.GetData<Enemy>("owner").patrolPoint_2);
        float maxX = Mathf.Max(blackboard.GetData<Enemy>("owner").patrolPoint_1, blackboard.GetData<Enemy>("owner").patrolPoint_2);

        return minX <= blackboard.GetData<Enemy>("owner").target.transform.position.x && blackboard.GetData<Enemy>("owner").target.transform.position.x <= maxX;
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

        float accelAmount = blackboard.GetData<Enemy>("owner").movementType.trackAccelAmount;

        float speedDif = targetSpeed - blackboard.GetData<Enemy>("owner").rb.velocity.x;
        float movement = speedDif * accelAmount;

        blackboard.GetData<Enemy>("owner").rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }
}