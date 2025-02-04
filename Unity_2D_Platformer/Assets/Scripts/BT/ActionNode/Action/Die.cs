using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : IAction
{
    private Blackboard blackboard;

    public Die(Blackboard blackBoard)
    {
        this.blackboard = blackBoard;
    }

    public void OnEnter()
    {
        blackboard.GetData<Enemy>("owner").spriteRenderer.color = Color.white;
        blackboard.GetData<Enemy>("owner").healthBar.gameObject.SetActive(false);
        blackboard.GetData<Enemy>("owner").animHandler.animator.SetTrigger("Die");

        SoundManager.Instance.PlaySoundEffect(SoundManager.InGameSoundEffectType.ENEMY_HIT, 0.5f);
    }

    public NodeState Execute()
    {
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
