using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSelector : CompositeNode
{
    INode runningNode;

    private int index;
    public AttackSelector(int number)
    {
        this.index = number;
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
            else if (runningState == NodeState.Success)
            {
                runningNode = null;
                return NodeState.Success;
            }
            else
            {
                runningNode = null;
                return NodeState.Failure;
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

    public override void Reset(int index)
    {
        base.Reset(index);

        if (index < this.index)
        {
            runningNode = null;
        }
    }
}
