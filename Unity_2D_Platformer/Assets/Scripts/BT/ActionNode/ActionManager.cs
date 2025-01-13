using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction
{
    void OnEnter();
    void OnExit();
    NodeState Execute();

    void ExecuteInFixedUpdate();
    void OnAnimationEnterEvent();
    void OnAnimationTransitionEvent();
    void OnAnimationExitEvent();
}

public class ActionManager
{
    private IAction currentAction;
    private NodeState currentState;
    public void ChangeAction(IAction newAction)
    {
        if (currentAction != newAction || currentState == NodeState.Success)
        {
            currentAction?.OnExit();
            currentAction = newAction;
            currentAction.OnEnter();
        }
    }

    public NodeState ExecuteCurrentAction()
    {
        if (currentAction == null)
        {
            currentState = NodeState.Failure;
            return currentState;
        }
        else
        {
            currentState = currentAction.Execute();
            return currentState;
        }
    }

    public void ExecuteCurrentActionInFixedUpdate()
    {
        if(currentState == NodeState.Running)
        {
            currentAction?.ExecuteInFixedUpdate();
        }
    }

    public void OnAnimationEnterEvent()
    {
        currentAction.OnAnimationEnterEvent();
    }
    public void OnAnimationTransitionEvent()
    {
        currentAction.OnAnimationTransitionEvent();
    }
    public void OnAnimationExitEvent()
    {
        currentAction.OnAnimationExitEvent();
    }
}
