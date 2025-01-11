using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class Selector : CompositeNode
{
    private int number;

    public Selector(int number)
    {
        this.number = number;
    }
    public override NodeState Evaluate()
    {
        foreach (var child in children)
        {
            switch (child.Evaluate())
            {
                case NodeState.Success:
                    return NodeState.Success;
                case NodeState.Running:
                    return NodeState.Running;
                case NodeState.Failure:
                    continue;
            }
        }
        return NodeState.Failure;
    }
}
