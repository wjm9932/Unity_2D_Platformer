using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : CompositeNode
{
    private int number;

    public Sequence(int number)
    {
        this.number = number;
    }

    public override NodeState Evaluate()
    {
        foreach (var child in children)
        {
            switch (child.Evaluate())
            {
                case NodeState.Failure:
                    return NodeState.Failure;
                case NodeState.Running:
                    return NodeState.Running;
                case NodeState.Success:
                    continue;
            }
        }
        return NodeState.Success;
    }
}
