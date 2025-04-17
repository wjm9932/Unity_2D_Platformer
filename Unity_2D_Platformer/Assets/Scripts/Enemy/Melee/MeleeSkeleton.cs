using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSkeleton : Enemy
{
    BehaviorTree bt;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        BuildBT();
        trackStopDistance = patrolStopDistance + movementType.trackStopDistance * transform.localScale.x;
    }

    void Update()
    {
        bt.root.Evaluate();
    }
    private void FixedUpdate()
    {
        bt.actionManager.ExecuteCurrentActionInFixedUpdate();
    }

    public override void OnAnimationEnterEvent()
    {
        bt.actionManager.OnAnimationEnterEvent();
    }
    public override void OnAnimationTransitionEvent()
    {
        bt.actionManager.OnAnimationTransitionEvent();
    }
    public override void OnAnimationExitEvent()
    {
        bt.actionManager.OnAnimationExitEvent();
    }
    private void BuildBT()
    {
        Blackboard blackboard = new Blackboard();
        blackboard.SetData<Enemy>("owner", this);

        bt = new BehaviorTreeBuilder(blackboard)
            .AddSelector()
                .AddSequence()
                    .AddCondition(() => isDead == true)
                    .AddAction(new Die(blackboard))
                .EndComposite()
                .AddAttackSequence()
                    .AddCondition(() => canBeDamaged == false)
                    .AddAction(new Hit(blackboard))
                    .AddAction(new Wait(movementType.groggyTime, () => canBeDamaged == false))
                .EndComposite()
                .AddAttackSequence()
                    .AddCondition(() => target != null)
                    .AddAction(new Track(blackboard))
                    .AddAction(new SwordAttack(blackboard))
                .EndComposite()
                .AddAttackSequence()
                    .AddAction(new Patrol(blackboard))
                    .AddAction(new Idle(blackboard))
                .EndComposite()
            .EndComposite()
            .Build();

    }

    #region EDITOR METHODS
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackRoot.position, attackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + patrolStopDistance * transform.right.x, transform.position.y));
    }
#endif
    #endregion
}
