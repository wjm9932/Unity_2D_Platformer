using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class AnimationHandler : MonoBehaviour
{
    //private PlayerMovement playerMovement;
    private Player player;
    public Animator animator;

    public bool isJumpTriggered { private get; set; } = false;
    public bool comboAttack_1 { private get; set; } = false;   
    public bool resetCombo { private get; set; } = false;
    public bool isSlide { private get; set; } = false;

    private void Awake()
    {
        player = transform.root.GetComponent<Player>();
        animator = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        if (isJumpTriggered == true)
        {
            animator.SetTrigger("Jump");
            isJumpTriggered = false;
        }

        if(comboAttack_1 == true)
        {
            animator.SetTrigger("Combo_1");
            comboAttack_1 = false;
        }

        if(resetCombo == true)
        {
            animator.SetTrigger("ResetCombo");
            resetCombo = false;
        }

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
