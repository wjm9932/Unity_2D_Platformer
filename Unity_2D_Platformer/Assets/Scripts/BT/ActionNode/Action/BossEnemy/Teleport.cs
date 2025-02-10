using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : IAction
{
    private Blackboard blackboard;
    private bool isTeleportingFinished;

    public Teleport(Blackboard blackBoard)
    {
        this.blackboard = blackBoard;
    }

    public void OnEnter()
    {
        isTeleportingFinished = false;
        blackboard.GetData<Boss>("owner").isGraceTime = true;
        blackboard.GetData<Boss>("owner").rb.velocity = Vector2.zero;

        blackboard.GetData<Boss>("owner").animHandler.ResetOneFrameDelay();
        blackboard.GetData<Boss>("owner").animHandler.animator.SetTrigger("TeleportStart");

        SoundManager.Instance.PlaySoundEffect(SoundManager.InGameSoundEffectType.BOSS_TELEPORT, 1f);
    }

    public NodeState Execute()
    {
        if (isTeleportingFinished == true)
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
        blackboard.GetData<Boss>("owner").isGraceTime = false;
    }
    public void OnAnimationEnterEvent()
    {
        Teleportation();
    }
    public void OnAnimationTransitionEvent()
    {

    }
    public void OnAnimationExitEvent()
    {
        isTeleportingFinished = true;
    }

    private void Teleportation()
    {
        Vector2 randomPos = blackboard.GetData<Boss>("owner").transform.position;

        randomPos.x = Random.Range(blackboard.GetData<Boss>("owner").bossRange[0].transform.position.x, blackboard.GetData<Boss>("owner").bossRange[1].transform.position.x);

        blackboard.GetData<Boss>("owner").transform.position = randomPos;

    }
}
