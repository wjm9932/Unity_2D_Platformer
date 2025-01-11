using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestAction : IAction
{
    private Blackboard blackboard;
    private float testTime;
    private int index;
    public TestAction(Blackboard blackBoard, int index)
    {
        this.blackboard = blackBoard;
        this.index = index;
    }

    public void OnEnter()
    {
        testTime = 0f;
    }

    public NodeState Execute()
    {
        testTime += Time.deltaTime;
        if (testTime >= 2f)
        {
            Debug.Log("Success " + index);
            return NodeState.Success;
        }
        else
        {
            Debug.Log("Running " +  index);
            return NodeState.Running;
        }
    }

    public void OnExit()
    {

    }

}

