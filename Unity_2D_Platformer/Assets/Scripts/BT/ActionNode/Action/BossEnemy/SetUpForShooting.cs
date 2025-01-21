using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUpForShooting : IAction
{
    private Blackboard blackboard;
    private bool isTeleportingFinished;

    public SetUpForShooting(Blackboard blackBoard)
    {
        this.blackboard = blackBoard;
    }

    public void OnEnter()
    {
        blackboard.GetData<Boss>("owner").rb.velocity = Vector2.zero;

        isTeleportingFinished = false;
        blackboard.GetData<Boss>("owner").isVulnerable = true;
        blackboard.GetData<Boss>("owner").rb.isKinematic = true;

        blackboard.GetData<Boss>("owner").animHandler.animator.SetTrigger("ShootingStart");
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

    }
    public void OnAnimationEnterEvent()
    {
        var targetPos = blackboard.GetData<Boss>("owner").target.transform.position;
        targetPos.y += 10f;
        blackboard.GetData<Boss>("owner").transform.position = targetPos;
        isTeleportingFinished = true;
    }
    public void OnAnimationTransitionEvent()
    {

    }
    public void OnAnimationExitEvent()
    {
        
    }

    
}