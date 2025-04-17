using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree
{
    public Blackboard blackboard { get; private set; }
    public ActionManager actionManager { get; private set; }
    public CompositeNode root { get; private set; }

    public BehaviorTree(CompositeNode root, Blackboard blackboard, ActionManager actionManager)
    {
        this.blackboard = blackboard;
        this.actionManager = actionManager;
        this.root = root;
    }
}
