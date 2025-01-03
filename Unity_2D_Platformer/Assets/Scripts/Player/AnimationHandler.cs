using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class AnimationHandler : MonoBehaviour
{
    //private PlayerMovement playerMovement;
    private Player playerMovement;
    private Animator animator;

    public bool isJumpTriggered { private get; set; } = false;
    public bool isAttackTriggered { private get; set; } = false;   
    public bool isSlide { private get; set; } = false;

    private void Awake()
    {
        playerMovement = transform.root.GetComponent<Player>();
        animator = GetComponent<Animator>();
    }


    private void LateUpdate()
    {
        if (isJumpTriggered == true)
        {
            animator.SetTrigger("Jump");
            isJumpTriggered = false;
        }

        if(isAttackTriggered == true)
        {
            animator.SetTrigger("Attack");
            isAttackTriggered = false;
        }

        animator.SetBool("IsSlide", isSlide);
        animator.SetBool("IsRun", playerMovement.input.moveInput.x != 0);
        animator.SetFloat("VelocityY", playerMovement.rb.velocity.y);
    }

    public bool IsAnimationFinished()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.normalizedTime >= 0.95f && !animator.IsInTransition(0); 
    }
}
