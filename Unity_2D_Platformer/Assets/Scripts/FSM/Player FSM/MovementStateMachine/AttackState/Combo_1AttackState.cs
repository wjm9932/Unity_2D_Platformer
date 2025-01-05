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
        sm.owner.animHandler.animator.SetTrigger("Combo_1");
    }
    public override void Update()
    {
        comboAttackBufferTime -= Time.deltaTime;

        if (canAttack == true)
        {
            var enemies = Physics2D.OverlapCircleAll(sm.owner.attackRoot.position, sm.owner.attackRange, sm.owner.enemyLayer);

            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].GetComponent<LivinEntity>().ApplyDamage(sm.owner.dmg);
            }
        }

        if(sm.owner.input.isAttack == true)
        {
            comboAttackBufferTime = sm.owner.movementType.attackBufferTime;
        }
        
        if (sm.jsm.currentState == sm.jsm.fallingState)
        {
            sm.ChangeState(sm.runState);
        }
        else if( comboAttackBufferTime > 0f && canComboAttack == true)
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

    }
    public override void OnAnimationEnterEvent()
    {
        canAttack = true;
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