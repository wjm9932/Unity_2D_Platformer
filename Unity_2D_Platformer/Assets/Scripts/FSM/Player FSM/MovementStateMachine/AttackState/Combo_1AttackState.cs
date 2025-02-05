using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combo_1AttackState : AttackState
{
    private float comboAttackBufferTime;
    public Combo_1AttackState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        comboAttackBufferTime = 0f;

        sm.owner.dmg = 10f;
        sm.owner.animHandler.animator.SetTrigger("Combo_1");
    }
    public override void Update()
    {
        comboAttackBufferTime -= Time.deltaTime;

        if(canAttack == true)
        {
            Attack(false);
        }

        if (sm.owner.input.isAttack == true)
        {
            comboAttackBufferTime = sm.owner.movementType.attackBufferTime;
        }

        if (sm.jsm.currentState == sm.jsm.fallingState)
        {
            sm.ChangeState(sm.runState);
        }
        else if (comboAttackBufferTime > 0f && canComboAttack == true)
        {
            sm.ChangeState(sm.combo_2AttackState);
        }
        else
        {
            base.Update();
        }
    }
    public override void FixedUpdate()
    {
        if (sm.currentState != this)
        {
            return;
        }

        DeaccelPlayerVelocity();
    }
    public override void LateUpdate()
    {

    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void OnAnimationEnterEvent()
    {
        canAttack = true;
        SoundManager.Instance.PlaySoundEffect(SoundManager.InGameSoundEffectType.PLAYER_ATTACK_1, 0.15f);
    }
    public override void OnAnimationExitEvent()
    {
        canComboAttack = true;
    }
    public override void OnAnimationTransitionEvent()
    {
        canAttack = false;
    }
}