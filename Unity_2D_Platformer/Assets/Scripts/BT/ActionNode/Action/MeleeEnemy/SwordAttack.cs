using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : IAction
{
    private Blackboard blackboard;
    private float decelerationFactor;
    private bool canAttack;
    public SwordAttack(Blackboard blackBoard)
    {
        this.blackboard = blackBoard;
        decelerationFactor = 0.35f;
    }

    public void OnEnter()
    {
        Flip(blackboard.GetData<Enemy>("owner").transform.position.x < blackboard.GetData<Enemy>("owner").target.transform.position.x);

        blackboard.GetData<Enemy>("owner").animHandler.animator.SetBool("IsAttack1", true);
        blackboard.GetData<Enemy>("owner").animHandler.ResetOneFrameDelay();
        canAttack = false;
    }

    public NodeState Execute()
    {
        if(canAttack == true)
        {
            Attack();
        }
        if(blackboard.GetData<Enemy>("owner").animHandler.IsAnimationFinishedWithDelay() == true)
        {
            blackboard.GetData<Enemy>("owner").animHandler.animator.SetBool("IsAttack1", false);
            return NodeState.Success;
        }

        return NodeState.Running;
    }

    public void ExecuteInFixedUpdate()
    {
        DeaccelPlayerVelocity();
    }
    public void OnExit()
    {
        blackboard.GetData<Enemy>("owner").animHandler.animator.SetBool("IsAttack1", false);
    }
    private void DeaccelPlayerVelocity()
    {
        float speedDiff = 0f - blackboard.GetData<Enemy>("owner").rb.velocity.x;
        float movement = speedDiff * ((1 / Time.fixedDeltaTime) * decelerationFactor);

        blackboard.GetData<Enemy>("owner").rb.AddForce(movement * Vector2.right);
    }

    private void Attack()
    {
        var target = Physics2D.OverlapCircle(blackboard.GetData<Enemy>("owner").attackRoot.position, blackboard.GetData<Enemy>("owner").attackRange, blackboard.GetData<Enemy>("owner").targetLayer);

        if(target != null)
        {
            target.GetComponent<Player>().TakeDamage(blackboard.GetData<Enemy>("owner").gameObject);
        }
    }

    public void OnAnimationEnterEvent()
    {
        SoundManager.Instance.PlaySoundEffect(blackboard.GetData<Enemy>("owner").attackSoundEffect, blackboard.GetData<Enemy>("owner").audioSource);

        canAttack = true;
    }
    public void OnAnimationTransitionEvent()
    {

    }
    public void OnAnimationExitEvent()
    {
        canAttack = false;
    }

    private void Flip(bool isMovingRight)
    {
        if (isMovingRight && blackboard.GetData<Enemy>("owner").transform.localRotation.eulerAngles.y != 0)
        {
            blackboard.GetData<Enemy>("owner").transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (!isMovingRight && blackboard.GetData<Enemy>("owner").transform.localRotation.eulerAngles.y != 180)
        {
            blackboard.GetData<Enemy>("owner").transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
