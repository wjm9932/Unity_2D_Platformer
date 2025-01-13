using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : IAction
{
    private Blackboard blackboard;
    private float stopDistance;
    private float targetPositionX;
    public Patrol(Blackboard blackBoard)
    {
        this.blackboard = blackBoard;
        stopDistance = blackboard.GetData<Enemy>("owner").patrolStopDistance;
    }

    public void OnEnter()
    {
        blackboard.GetData<Enemy>("owner").healthBar.gameObject.SetActive(false);

        targetPositionX = Random.Range(blackboard.GetData<Enemy>("owner").patrolPoint_1, blackboard.GetData<Enemy>("owner").patrolPoint_2);
        Flip(targetPositionX > blackboard.GetData<Enemy>("owner").transform.position.x);

        blackboard.GetData<Enemy>("owner").spriteRenderer.color = Color.white;
        blackboard.GetData<Enemy>("owner").animHandler.animator.SetBool("IsPatrol", true);
    }

    public NodeState Execute()
    {
        if (Mathf.Abs(blackboard.GetData<Enemy>("owner").transform.position.x - targetPositionX) <= stopDistance)
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
        blackboard.GetData<Enemy>("owner").animHandler.animator.SetBool("IsPatrol", false);
    }
    private void Flip(bool isMovingRight)
    {
        if (isMovingRight == false)
        {
            blackboard.GetData<Enemy>("owner").transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            blackboard.GetData<Enemy>("owner").transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void Run()
    {
        float targetSpeed = blackboard.GetData<Enemy>("owner").transform.right.x * blackboard.GetData<Enemy>("owner").movementType.patrolMaxSpeed;

        float accelAmount = blackboard.GetData<Enemy>("owner").movementType.patrolAccelAmount;

        float speedDif = targetSpeed - blackboard.GetData<Enemy>("owner").rb.velocity.x;
        float movement = speedDif * accelAmount;

        blackboard.GetData<Enemy>("owner").rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }
}
