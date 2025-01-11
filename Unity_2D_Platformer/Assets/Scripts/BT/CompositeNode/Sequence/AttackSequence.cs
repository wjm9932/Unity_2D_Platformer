using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSequence : CompositeNode
{
    private int currentIndex;
    private INode currentNode;
    private bool requireAllSuccess;


    private int attackSequenceNumber;
    public AttackSequence(bool requireAllSuccess, int attackSequenceNumber)
    {
        this.requireAllSuccess = requireAllSuccess;
        this.attackSequenceNumber = attackSequenceNumber;
    }

    public override NodeState Evaluate()
    {
        if (currentNode == null)
        {
            if (currentIndex < children.Count)
            {
                currentNode = children[currentIndex];
            }
            else
            {
                ResetNodeInfo();
                return NodeState.Success;
            }
        }

        NodeState state = currentNode.Evaluate();

        if (state == NodeState.Running)
        {
            return NodeState.Running;
        }
        else if (state == NodeState.Success)
        {
            currentIndex++;
            currentNode = null;
            return NodeState.Running; 
        }
        else
        {
            if(!requireAllSuccess)
            {
                if (currentIndex != 0)
                {
                    ResetNodeInfo();
                    return NodeState.Success;
                }
                else
                {
                    ResetNodeInfo();
                    return NodeState.Failure;
                }
            }
            else
            {
                ResetNodeInfo();
                return NodeState.Failure;
            }
        }
    }

    public override void Reset()
    {
        base.Reset();
        ResetNodeInfo();
    }

    private void ResetNodeInfo()
    {
        currentNode = null;
        currentIndex = 0;
    }
}