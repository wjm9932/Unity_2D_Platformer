using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastSpell : IAction
{
    private Blackboard blackboard;

    public CastSpell(Blackboard blackBoard)
    {
        this.blackboard = blackBoard;
    }

    public void OnEnter()
    {
        blackboard.SetData<bool>("IsCasting", true);
        Flip(blackboard.GetData<Boss>("owner").transform.position.x < blackboard.GetData<Boss>("owner").target.transform.position.x);

        blackboard.GetData<Boss>("owner").rb.velocity = Vector2.zero;

        blackboard.GetData<Boss>("owner").animHandler.ResetOneFrameDelay();
        blackboard.GetData<Boss>("owner").animHandler.animator.SetBool("IsCast", true);
    }

    public NodeState Execute()
    {
        if (blackboard.GetData<Boss>("owner").animHandler.IsAnimationFinishedWithDelay() == true)
        {
            blackboard.GetData<Boss>("owner").animHandler.animator.SetBool("IsCast", false);
            return NodeState.Success;
        }

        return NodeState.Running;
    }

    public void ExecuteInFixedUpdate()
    {
    }
    public void OnExit()
    {
        blackboard.SetData<bool>("IsCasting", false);
        blackboard.GetData<Boss>("owner").animHandler.animator.SetBool("IsCast", false);
    }
    public void OnAnimationEnterEvent()
    {
        var position = blackboard.GetData<Boss>("owner").target.transform.position;
        position.x += 0.5f;
        position.y -= blackboard.GetData<Boss>("owner").target.playerFootOffset;

        Object.Instantiate(blackboard.GetData<Boss>("owner").spellPrefab, position, Quaternion.identity);

        SoundManager.Instance.PlaySoundEffect(SoundManager.InGameSoundEffectType.BOSS_CAST_SPELL, 0.3f);
    }
    public void OnAnimationTransitionEvent()
    {

    }
    public void OnAnimationExitEvent()
    {

    }

    private void Flip(bool isMovingRight)
    {
        if (isMovingRight && blackboard.GetData<Boss>("owner").transform.localRotation.eulerAngles.y != 0)
        {
            blackboard.GetData<Boss>("owner").transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (!isMovingRight && blackboard.GetData<Boss>("owner").transform.localRotation.eulerAngles.y != 180)
        {
            blackboard.GetData<Boss>("owner").transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }
}