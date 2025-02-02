using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyToAttack : IAction
{
    private Blackboard blackboard;
    private readonly float readyTime;
    private float timeElapsed;

    private GameObject readyEffect;

    public ReadyToAttack(Blackboard blackBoard, float readyTime)
    {
        this.blackboard = blackBoard;
        this.readyTime = readyTime;
    }

    public void OnEnter()
    {
        timeElapsed = 0f;
        var transform = blackboard.GetData<ChasingEnemy>("owner").transform;
        var targetPrefab = blackboard.GetData<ChasingEnemy>("owner").lightningAttackReadyPrefab;
        readyEffect = Object.Instantiate(targetPrefab, transform.position + targetPrefab.transform.position, targetPrefab.transform.rotation, transform);
    }

    public NodeState Execute()
    {
        timeElapsed += Time.deltaTime;

        if(timeElapsed >= readyTime)
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
        Object.Destroy(readyEffect);
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
