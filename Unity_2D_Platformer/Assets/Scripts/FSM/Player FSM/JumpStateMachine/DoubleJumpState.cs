using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpState : IState, IGravityModifier
{
    private PlayerJumpStateMachine sm;
    public DoubleJumpState(PlayerJumpStateMachine playerJumpStateMachine)
    {
        sm = playerJumpStateMachine;
    }
    public void Enter()
    {
        sm.isJumpCut = false;

        DobuleJump(sm.owner.transform.right.x);

        sm.owner.animHandler.animator.SetTrigger("Jump");

        ObjectPoolManager.Instance.GetPoolableObject(sm.owner.doubleJumpEffectPrefab, sm.owner.doubleJumpEffectTransform.position, sm.owner.doubleJumpEffectTransform.rotation);
        SoundManager.Instance.PlaySoundEffect(SoundManager.InGameSoundEffectType.PLAYER_DOUBLE_JUMP, 0.2f);
    }
    public void Update()
    {
        if (sm.owner.rb.velocity.y <= 0f)
        {
            sm.ChangeState(sm.doubleJumpFallingState);
            return;
        }

        if (sm.owner.input.isJumpCut == true)
        {
            sm.isJumpCut = true;
        }

        SetGravityScale();
    }
    public void FixedUpdate()
    {

    }
    public void LateUpdate()
    {

    }
    public void Exit()
    {

    }

    public virtual void OnAnimationEnterEvent()
    {

    }
    public virtual void OnAnimationExitEvent()
    {
    }
    public virtual void OnAnimationTransitionEvent()
    {
    }
    public void SetGravityScale()
    {
        if (sm.msm.currentState == sm.msm.dashState)
        {
            sm.owner.SetGravityScale(0f);
        }
        else if(sm.owner.input.moveInput.y < 0)
        {
            sm.owner.SetGravityScale(sm.owner.movementType.gravityScale * sm.owner.movementType.fastFallGravityMult);
            sm.owner.rb.velocity = new Vector2(sm.owner.rb.velocity.x, Mathf.Max(sm.owner.rb.velocity.y, -sm.owner.movementType.maxFastFallSpeed));
        }
        else if (sm.isJumpCut == true)
        {
            sm.owner.SetGravityScale(sm.owner.movementType.gravityScale * sm.owner.movementType.jumpCutGravityMult);
            sm.owner.rb.velocity = new Vector2(sm.owner.rb.velocity.x, Mathf.Max(sm.owner.rb.velocity.y, -sm.owner.movementType.maxFallSpeed));
        }
        else if (Mathf.Abs(sm.owner.rb.velocity.y) < sm.owner.movementType.jumpHangVelocityThreshold)
        {
            sm.owner.SetGravityScale(sm.owner.movementType.gravityScale * sm.owner.movementType.jumpHangGravityMult);
        }
        else
        {
            sm.owner.SetGravityScale(sm.owner.movementType.gravityScale);
        }
    }

    private void DobuleJump(float dir)
    {

        Vector2 force = new Vector2(sm.owner.movementType.doubleJumpForce.x, sm.owner.movementType.doubleJumpForce.y);
        force.x *= dir;

        if (Mathf.Sign(sm.owner.rb.velocity.x) != Mathf.Sign(force.x))
            force.x -= sm.owner.rb.velocity.x;

        if (Mathf.Abs(sm.owner.rb.velocity.y) > 0)
            force.y -= sm.owner.rb.velocity.y;

        sm.owner.rb.AddForce(force, ForceMode2D.Impulse);
    }
}