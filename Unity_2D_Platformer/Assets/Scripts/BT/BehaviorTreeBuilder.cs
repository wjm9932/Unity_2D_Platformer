using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTreeBuilder : MonoBehaviour
{
    public Blackboard blackboard { get; private set; }
    public ActionManager actionManager { get; private set; }

    private Stack<CompositeNode> nodeStack = new Stack<CompositeNode>();
    private CompositeNode currentNode;
    private int compositionNodeIndex = -1;

    private void Awake()
    {
        blackboard = new Blackboard();
        actionManager = new ActionManager();
    }

    public BehaviorTreeBuilder AddSelector()
    {
        var selector = new Selector(++compositionNodeIndex);
        if (currentNode != null) nodeStack.Push(currentNode);
        currentNode = selector;
        return this;
    }

    public BehaviorTreeBuilder AddAttackSelector()
    {
        var selector = new AttackSelector(++compositionNodeIndex);
        if (currentNode != null) nodeStack.Push(currentNode);
        currentNode = selector;
        return this;
    }

    public BehaviorTreeBuilder AddRandomAttackSelector()
    {
        var selector = new RandomAttackSelector(++compositionNodeIndex);
        if (currentNode != null) nodeStack.Push(currentNode);
        currentNode = selector;
        return this;
    }

    public BehaviorTreeBuilder AddSequence()
    {
        var sequence = new Sequence(++compositionNodeIndex);
        if (currentNode != null) nodeStack.Push(currentNode);
        currentNode = sequence;
        return this;
    }
    public BehaviorTreeBuilder AddAttackSequence(bool requireAllSuccess = false)
    {
        var selector = new AttackSequence(requireAllSuccess, ++compositionNodeIndex);
        if (currentNode != null) nodeStack.Push(currentNode);
        currentNode = selector;
        return this;
    }


    public BehaviorTreeBuilder AddCondition(Func<bool> condition)
    {
        currentNode.AddChild(new ConditionNode(condition));
        return this;
    }

    public BehaviorTreeBuilder AddAction(IAction action, ActionManager actionManager)
    {
        currentNode.AddChild(new ActionNode(action, actionManager, compositionNodeIndex));
        return this;
    }

    public BehaviorTreeBuilder EndComposite()
    {
        var finishedNode = currentNode;
        if (nodeStack.Count > 0)
        {
            currentNode = nodeStack.Pop();
            currentNode.AddChild(finishedNode);
        }
        return this;
    }

    public CompositeNode Build()
    {

        if(nodeStack.Count > 0)
        {
            Debug.LogError("EndComposite Error");
            return null;
        }

        var root = currentNode;

        InjectResetActionDependencies(root.Reset, currentNode);

        return currentNode;
    }

    private void InjectResetActionDependencies(Action<int> resetAction, CompositeNode node)
    {
        foreach (var child in node.GetChildren())
        {
            if (child is ActionNode actionNode)
            {
                actionNode.SetResetAction(resetAction, actionNode.parentCompositionNodeIndex);
            }
            else if (child is CompositeNode compositeChild)
            {
                InjectResetActionDependencies(resetAction, compositeChild);
            }
        }
    }
}
