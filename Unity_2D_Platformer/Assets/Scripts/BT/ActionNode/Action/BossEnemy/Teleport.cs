using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : IAction
{
    private Blackboard blackboard;
    private bool isTeleportingFinished;

    public Teleport(Blackboard blackBoard)
    {
        this.blackboard = blackBoard;
    }

    public void OnEnter()
    {
        isTeleportingFinished = false;
        blackboard.GetData<Boss>("owner").isVulnerable = true;
        blackboard.GetData<Boss>("owner").rb.velocity = Vector2.zero;

        blackboard.GetData<Boss>("owner").animHandler.ResetOneFrameDelay();
        blackboard.GetData<Boss>("owner").animHandler.animator.SetTrigger("TeleportStart");
    }

    public NodeState Execute()
    {
        if (isTeleportingFinished == true)
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
        blackboard.GetData<Boss>("owner").isVulnerable = false;
    }
    public void OnAnimationEnterEvent()
    {
        Teleportation();
    }
    public void OnAnimationTransitionEvent()
    {

    }
    public void OnAnimationExitEvent()
    {
        isTeleportingFinished = true;
    }

    private void Teleportation()
    {
        Vector2 randomPos = blackboard.GetData<Boss>("owner").transform.position;

        randomPos.x = Random.Range(blackboard.GetData<Boss>("owner").patrolPoint_1, blackboard.GetData<Boss>("owner").patrolPoint_2);

        blackboard.GetData<Boss>("owner").transform.position = randomPos;
    }

    private void Flip(bool isMovingRight)
    {
        if (isMovingRight && blackboard.GetData<Boss>("owner").transform.localRotation.eulerAngles.y != 0)
        {
            blackboard.GetData<Boss>("owner").transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (!isMovingRight && blackboard.GetData<Boss>("owner").transform.localRotation.eulerAngles.y != 180)
        {
            blackboard.GetData<Boss>("owner").transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
