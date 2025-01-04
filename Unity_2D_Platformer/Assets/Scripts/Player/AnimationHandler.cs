using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class AnimationHandler : MonoBehaviour
{
    //private PlayerMovement playerMovement;
    private Player player;
    public Animator animator;

    public bool isSlide { private get; set; } = false;

    private void Awake()
    {
        player = transform.root.GetComponent<Player>();
        animator = GetComponent<Animator>();
    }

    private void LateUpdate()
    {   
        animator.SetBool("IsSlide", isSlide);
        animator.SetBool("IsRun", player.input.moveInput.x != 0);
        animator.SetFloat("VelocityY", player.rb.velocity.y);
    }

    private void OnAnimationEnterEvent()
    {
        player.OnAnimationEnterEvent();
    }
    private void OnAnimationTransitionEvent()
    {
        player.OnAnimationTransitionEvent();
    }
    private void OnAnimationExitEvent()
    {
        player.OnAnimationExitEvent();
    }

    public bool IsAnimationFinished()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.normalizedTime >= 1f && !animator.IsInTransition(0); 
    }
}
