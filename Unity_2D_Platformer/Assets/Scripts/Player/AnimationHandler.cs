using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class AnimationHandler : MonoBehaviour
{
    //private PlayerMovement playerMovement;
    private Player playerMovement;
    private Animator animator;

    public bool isJumpStarted { private get; set; }

    private void Awake()
    {
        playerMovement = transform.root.GetComponent<Player>();
        animator = GetComponent<Animator>();
    }


    private void LateUpdate()
    {
        if(isJumpStarted == true)
        {
            animator.SetTrigger("Jump");
            isJumpStarted = false;
        }

        //animator.SetBool("IsSlide", playerMovement.isSlide);
        animator.SetBool("IsRun", playerMovement.input.moveInput.x != 0);
        animator.SetFloat("VelocityY", playerMovement.rb.velocity.y);
    }
}
