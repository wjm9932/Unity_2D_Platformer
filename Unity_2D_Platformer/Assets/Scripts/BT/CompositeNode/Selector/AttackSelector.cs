using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSelector : CompositeNode
{
    INode runningNode;

    private int number;
    public AttackSelector(int number)
    {
        this.number = number;
    }
    public override NodeState Evaluate()
    {
        if (runningNode != null)
        {
            NodeState runningState = runningNode.Evaluate();

            if (runningState == NodeState.Running)
            {
                return NodeState.Running;
            }

            runningNode = null;
            
            if (runningState == NodeState.Success)
            {
                return NodeState.Success;
            }
        }

        foreach (var child in children)
        {
            if (runningNode != null && runningNode == child)
            {
                continue;
            }

            NodeState state = child.Evaluate();

            if (state == NodeState.Running)
            {
                runningNode = child;
                return NodeState.Running;
            }
            else if (state == NodeState.Success)
            {
                return NodeState.Success;
            }
            else
            {
                continue;
            }
        }

        runningNode = null;
        return NodeState.Failure;
    }

    public override void Reset()
    {
        base.Reset();
        runningNode = null;
    }
}
