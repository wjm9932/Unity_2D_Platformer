using System;
using System.Collections.Generic;
using UnityEngine;

public class ActionNode : INode
{
    private IAction action;
    private ActionManager actionManager;
    public int parentCompositionNodeIndex { get; private set; }

    public ActionNode(IAction action, ActionManager actionManager, int compositionNodeIndex)
    {
        this.action = action;
        this.actionManager = actionManager;
        this.parentCompositionNodeIndex = compositionNodeIndex;
    }

    public NodeState Evaluate()
    {
        actionManager.ChangeAction(action);
        return actionManager.ExecuteCurrentAction();
    }

    public void SetResetAction(Action<int> resetAction, int parentCompositionNodeIndex)
    {
        if(action is ICompositionNodeResettable dependentAction)
        {
            dependentAction.SetResetAction(resetAction, parentCompositionNodeIndex); 
        }
    }
}
