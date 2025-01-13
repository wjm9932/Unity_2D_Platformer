using System;
using System.Collections.Generic;
using UnityEngine;

public class ResetNode : IAction, ICompositionNodeResettable
{
    private Action onResetCompositionNode;
    public ResetNode()
    {
    }

    public void OnEnter()
    {
        onResetCompositionNode();
    }

    public NodeState Execute()
    {
        return NodeState.Success;
    }
    public void ExecuteInFixedUpdate()
    {

    }
    public void OnExit()
    {

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
    public void SetResetAction(Action resetAction)
    {
        this.onResetCompositionNode = resetAction;
    }
}
