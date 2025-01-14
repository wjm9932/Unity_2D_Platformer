using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class AnimationHandler : MonoBehaviour
{
    //private PlayerMovement playerMovement;
    private LivingEntity entity;
    public Animator animator { get; private set; }

    private void Awake()
    {
        entity = transform.root.GetComponent<LivingEntity>();
        animator = GetComponent<Animator>();
    }

    private void LateUpdate()
    {   
        
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

    public bool IsAnimationFinished()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        return stateInfo.normalizedTime >= 1f && !animator.IsInTransition(0); 
    }
}
