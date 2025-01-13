using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : IAction
{
    private Blackboard blackboard;

    public SwordAttack(Blackboard blackBoard)
    {
        this.blackboard = blackBoard;
    }

    public void OnEnter()
    {
        blackboard.GetData<Enemy>("owner").rb.velocity = Vector2.zero;
    }

    public NodeState Execute()
    {
        Debug.Log("123");
        return NodeState.Running;
    }

    public void ExecuteInFixedUpdate()
    {

    }
    public void OnExit()
    {

    }

}
