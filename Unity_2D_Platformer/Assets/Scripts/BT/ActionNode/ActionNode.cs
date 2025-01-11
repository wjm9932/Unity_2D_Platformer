using System;
using System.Collections.Generic;
using UnityEngine;

public class ActionNode : INode
{
    private IAction action;
    private ActionManager actionManager;

    public ActionNode(IAction action, ActionManager actionManager)
    {
        this.action = action;
        this.actionManager = actionManager;
    }

    public NodeState Evaluate()
    {
        actionManager.ChangeAction(action);
        return actionManager.ExecuteCurrentAction();
    }

    public void SetResetAction(Action resetAction)
    {
        if(action is ICompositionNodeResettable dependentAction)
        {
            dependentAction.SetResetAction(resetAction); 
        }
    }
}
