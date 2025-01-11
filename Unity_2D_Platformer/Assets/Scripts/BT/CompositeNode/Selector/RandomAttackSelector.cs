using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RandomAttackSelector : CompositeNode
{
    private INode runningNode;
    private int number;

    public RandomAttackSelector(int number)
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

            return NodeState.Failure;
        }

        ShuffleChildren();

        foreach(var child in children)
        {
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

        return NodeState.Failure;
    }

    private void ShuffleChildren()
    {
        for (int i = children.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            var temp = children[i];
            children[i] = children[randomIndex];
            children[randomIndex] = temp;
        }
    }

    public override void Reset()
    {
        base.Reset();
        runningNode = null;
    }
}
