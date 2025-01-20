using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rino : Enemy
{
    private EnemyStateMachine enemyStateMachine;

    protected override void Awake()
    {
        base.Awake();
        enemyStateMachine = new EnemyStateMachine(this);
    }

    protected override void Start()
    {
        base.Start();
        enemyStateMachine.ChangeState(enemyStateMachine.patrolState);
    }

    void Update()
    {

        enemyStateMachine.Update();
    }
    private void FixedUpdate()
    {
        enemyStateMachine.FixedUpdate();
    }

    public override void Die()
    {
        base.Die();

        enemyStateMachine.ChangeState(enemyStateMachine.enemyDieState);
    }

    public override bool TakeDamage(float dmg, GameObject damager, bool isHardAttack = false)
    {
        if (base.TakeDamage(dmg, damager) == false)
        {
            return false;
        }
        else
        {
            hp -= dmg;
            target = damager.GetComponent<Player>();

            if (hp <= 0f)
            {
                Die();
            }
            else
            {
                enemyStateMachine.ChangeState(enemyStateMachine.enemyHitState);
            }

            return true;
        }
    }

    public override void OnAnimationEnterEvent()
    {
        enemyStateMachine.OnAnimationEnterEvent();
    }
    public override void OnAnimationTransitionEvent()
    {
        enemyStateMachine.OnAnimationTransitionEvent();
    }
    public override void OnAnimationExitEvent()
    {
        enemyStateMachine.OnAnimationExitEvent();
    }
}
