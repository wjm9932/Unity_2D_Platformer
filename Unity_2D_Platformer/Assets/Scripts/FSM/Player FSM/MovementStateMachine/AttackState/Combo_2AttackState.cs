using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combo_2AttackState : AttackState
{
    public Combo_2AttackState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        sm.owner.rb.AddForce(sm.owner.transform.right * 50f, ForceMode2D.Impulse);
        sm.owner.animHandler.comboAttack_2 = true;
    }
    public override void Update()
    {
        if (canAttack == true)
        {
            var enemies = Physics2D.OverlapCircleAll(sm.owner.attackRoot.position, sm.owner.attackRange, sm.owner.enemyLayer);

            for (int i = 0; i < enemies.Length; i++)
            {
                Debug.Log(enemies[i].name);
            }
        }


        if (sm.jsm.currentState == sm.jsm.fallingState)
        {
            sm.ChangeState(sm.runState);
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
    }
    public override void OnAnimationTransitionEvent()
    {
        canAttack = false;
    }

    private void DeaccelPlayerVelocity()
    {
        float speedDiff = 0f - sm.owner.rb.velocity.x;
        float movement = speedDiff * ((1 / Time.fixedDeltaTime) * 0.2f);

        sm.owner.rb.AddForce(movement * Vector2.right);
    }
}