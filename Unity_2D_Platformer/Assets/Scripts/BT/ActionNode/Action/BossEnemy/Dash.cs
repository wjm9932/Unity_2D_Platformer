using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : IAction
{
    private Blackboard blackboard;
    private readonly float dashTime;
    private float targetPositionX;
    private float speed;
    private bool isCloseEnough;

    private float timeElapsed;
    public Dash(Blackboard blackBoard)
    {
        this.blackboard = blackBoard;
        dashTime = 0.3f;
    }

    public void OnEnter()
    {
        timeElapsed = 0f;
        Flip(blackboard.GetData<Enemy>("owner").transform.position.x < blackboard.GetData<Enemy>("owner").target.transform.position.x);

        float relativeDistance = Mathf.Abs(blackboard.GetData<Enemy>("owner").transform.position.x - blackboard.GetData<Enemy>("owner").target.transform.position.x);

        if (relativeDistance <= blackboard.GetData<Enemy>("owner").trackStopDistance)
        {
            targetPositionX = blackboard.GetData<Enemy>("owner").transform.position.x;
            speed = 0f;
            timeElapsed = dashTime;
        }
        else
        {
            blackboard.GetData<Enemy>("owner").animHandler.animator.SetBool("IsDash", true);

            targetPositionX = blackboard.GetData<Enemy>("owner").target.transform.position.x
                              - (blackboard.GetData<Enemy>("owner").trackStopDistance / 2f) * blackboard.GetData<Enemy>("owner").transform.right.x;

            float distance = Mathf.Abs(blackboard.GetData<Enemy>("owner").transform.position.x - targetPositionX);
            speed = distance / dashTime;
        }

        blackboard.GetData<Enemy>("owner").rb.velocity = blackboard.GetData<Enemy>("owner").transform.right * speed;
    }

    public NodeState Execute()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= dashTime)
        {
            return NodeState.Success;
        }

        return NodeState.Running;
    }

    public void ExecuteInFixedUpdate()
    {
    }
    public void OnExit()
    {
        blackboard.GetData<Enemy>("owner").animHandler.animator.SetBool("IsDash", false);
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
