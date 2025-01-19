using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Boss : Enemy
{
    private BehaviorTreeBuilder btBuilder;
    private CompositeNode root;

    protected override void Awake()
    {
        base.Awake();
        btBuilder = GetComponent<BehaviorTreeBuilder>();
    }

    protected override void Start()
    {
        base.Start();
        BuildBT();
        trackStopDistance = patrolStopDistance + movementType.trackStopDistance * transform.localScale.x;
    }

    void Update()
    {
        root.Evaluate();
    }
    private void FixedUpdate()
    {
        btBuilder.actionManager.ExecuteCurrentActionInFixedUpdate();
    }

    public override void OnAnimationEnterEvent()
    {
        btBuilder.actionManager.OnAnimationEnterEvent();
    }
    public override void OnAnimationTransitionEvent()
    {
        btBuilder.actionManager.OnAnimationTransitionEvent();
    }
    public override void OnAnimationExitEvent()
    {
        btBuilder.actionManager.OnAnimationExitEvent();
    }

    public override void Die()
    {
        base.Die();
    }

    private void BuildBT()
    {
        btBuilder.blackboard.SetData<Enemy>("owner", this);

        root = btBuilder
            .AddSelector()
                .AddSequence()
                    .AddCondition(() => isDead == true)
                    .AddAction(new Die(btBuilder.blackboard), btBuilder.actionManager)
                .EndComposite()
                .AddAttackSequence()
                    .AddCondition(() => canBeDamaged == false && isHardAttack == true)
                    .AddAction(new Hit(btBuilder.blackboard), btBuilder.actionManager)
                    .AddAction(new Wait(movementType.groggyTime, () => canBeDamaged == false), btBuilder.actionManager)
                .EndComposite()
                .AddAttackSequence()
                    .AddCondition(() => target != null && IsTargetOnWayPoints() == true && target.isDead == false)
                    .AddAttackSelector()
                        .AddSequence()
                            .AddCondition(() => !IsInRange(10f))
                            .AddAction(new Track(btBuilder.blackboard), btBuilder.actionManager)
                        .EndComposite()
                        .AddAttackSequence()
                            .AddCondition(() => RandomExecute(0.6f) && !IsInRange(6f))
                            .AddAction(new Dash(btBuilder.blackboard), btBuilder.actionManager)
                            .AddAttackSelector()
                                .AddAttackSequence()
                                    .AddCondition(() => RandomExecute(0f))
                                    .AddAction(new SwordAttack(btBuilder.blackboard), btBuilder.actionManager)
                                .EndComposite()
                                .AddAttackSequence()
                                    .AddAction(new Track(btBuilder.blackboard), btBuilder.actionManager)
                                    .AddAction(new SwordAttack(btBuilder.blackboard), btBuilder.actionManager)
                                .EndComposite()
                            .EndComposite()
                        .EndComposite()
                        .AddAttackSequence()
                            .AddAction(new Track(btBuilder.blackboard), btBuilder.actionManager)
                            .AddAction(new SwordAttack(btBuilder.blackboard), btBuilder.actionManager)
                        .EndComposite()
                    .EndComposite()
                .EndComposite()
                .AddAttackSequence()
                    .AddAction(new Patrol(btBuilder.blackboard), btBuilder.actionManager)
                    .AddAction(new Idle(btBuilder.blackboard), btBuilder.actionManager)
                .EndComposite()
            .EndComposite()
            .Build();
    }

    private bool IsTargetOnWayPoints()
    {
        float minX = Mathf.Min(patrolPoint_1, patrolPoint_2);
        float maxX = Mathf.Max(patrolPoint_1, patrolPoint_2);

        return minX <= target.transform.position.x && target.transform.position.x <= maxX;
    }

    private bool IsInRange(float distance)
    {
        return distance >= Mathf.Abs(target.transform.position.x - transform.position.x);
    }
    private bool RandomExecute(float chances)
    {
        return Random.Range(0f, 1f) <= chances;
    }
    #region EDITOR METHODS
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackRoot.position, attackRange);
    }
#endif
    #endregion
}
