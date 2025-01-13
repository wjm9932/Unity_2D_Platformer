using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : IAction
{
    private Blackboard blackboard;
    private float idleStartTime;

    public Idle(Blackboard blackBoard)
    {
        this.blackboard = blackBoard;

    }

    public void OnEnter()
    {
        idleStartTime = Time.time;
    }

    public NodeState Execute()
    {
        if (Time.time > idleStartTime + blackboard.GetData<Enemy>("owner").movementType.idleTime)
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

    }

    private void DeaccelPlayerVelocity()
    {
        float speedDiff = 0f - blackboard.GetData<Enemy>("owner").rb.velocity.x;
        float movement = speedDiff * blackboard.GetData<Enemy>("owner").movementType.patrolDeccelAmount;

        blackboard.GetData<Enemy>("owner").rb.AddForce(movement * Vector2.right);
    }

}

