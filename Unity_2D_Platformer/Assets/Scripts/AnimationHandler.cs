using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class AnimationHandler : MonoBehaviour
{
    //private PlayerMovement playerMovement;
    private LivingEntity entity;
    public Animator animator { get; private set; }
    private bool isReadyToCheck; 
    private void Awake()
    {
        entity = transform.parent.GetComponent<LivingEntity>();
        animator = GetComponent<Animator>();
        isReadyToCheck = false;
    }

    private void OnAnimationEnterEvent()
    {
        entity.OnAnimationEnterEvent();
    }
    private void OnAnimationTransitionEvent()
    {
        entity.OnAnimationTransitionEvent();
    }
    private void OnAnimationExitEvent()
    {
        entity.OnAnimationExitEvent();
    }
    public void ResetOneFrameDelay()
    {
        isReadyToCheck = false;
        StartCoroutine(EnableCheckAfterOneFrame());
    }

    private IEnumerator EnableCheckAfterOneFrame()
    {
        yield return null;
        isReadyToCheck = true;
    }
    public bool IsAnimationFinishedWithDelay()
    {
        if (!isReadyToCheck)
            return false;

        return IsAnimationFinished();
    }
    public bool IsAnimationFinished()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        return stateInfo.normalizedTime >= 1f && !animator.IsInTransition(0); 
    }
}
