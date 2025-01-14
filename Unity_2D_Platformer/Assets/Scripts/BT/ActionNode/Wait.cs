using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait : IAction
{
    private float waitTime;
    private float elapsedTime;
    private Func<bool> interruptCondition;
    public Wait(float waitTime, Func<bool> interruptCondition = null)
    {
        this.waitTime = waitTime;
        this.interruptCondition = interruptCondition;
    }

    public void OnEnter()
    {
        elapsedTime = 0f;
    }

    public NodeState Execute()
    {
        if(interruptCondition?.Invoke() == true)
        {
            return NodeState.Success;
        }
        
        elapsedTime += Time.deltaTime;
        if(elapsedTime >= waitTime)
        {
            return NodeState.Success;
        }

        return NodeState.Running;
    }

    public void ExecuteInFixedUpdate()
    {

    }
    public void OnExit()
    {

    }
    public void OnAnimationEnterEvent()
    {

    }
    public void OnAnimationTransitionEvent()
    {

    }
    public void OnAnimationExitEvent()
    {

    }
}

