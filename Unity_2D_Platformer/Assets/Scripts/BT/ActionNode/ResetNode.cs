using System;
using System.Collections.Generic;
using UnityEngine;

public class ResetNode : IAction, ICompositionNodeResettable
{
    private Blackboard blackboard;
    private Action onResetCompositionNode;
    public ResetNode(Blackboard blackBoard)
    {
        this.blackboard = blackBoard;
    }

    public void OnEnter()
    {
        onResetCompositionNode();
    }

    public NodeState Execute()
    {
        return NodeState.Success;
    }

    public void OnExit()
    {

    }

    public void SetResetAction(Action resetAction)
    {
        this.onResetCompositionNode = resetAction;
    }
}
