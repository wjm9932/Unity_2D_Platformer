using System;
using System.Collections.Generic;
using UnityEngine;

public class ResetNode : IAction, ICompositionNodeResettable
{
    public Action<int> onResetCompositionNode { private get; set; }
    public int parentCompositionNodeIndex { private get; set; }

    public ResetNode()
    {
    }

    public void OnEnter()
    {
        onResetCompositionNode(-1);
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
    public void SetResetAction(Action<int> resetAction, int parentCompositionNodeIndex)
    {
        this.onResetCompositionNode = resetAction;
        this.parentCompositionNodeIndex = parentCompositionNodeIndex;
    }
}
