using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Boss : Enemy
{
    private BehaviorTreeBuilder btBuilder;
    private CompositeNode root;

    [Space(20)]
    [Header("Boss Components")]
    public GameObject spellPrefab;

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
        btBuilder.blackboard.SetData<Boss>("owner", this);

        root = btBuilder
            .AddSelector()
                #region Die Sequence
                .AddSequence()
                    .AddCondition(() => isDead == true)
                    .AddAction(new Die(btBuilder.blackboard), btBuilder.actionManager)
                .EndComposite()
        #endregion
                #region Hit Sequence
                .AddAttackSequence()
                    .AddCondition(() => canBeDamaged == false && isHardAttack == true)
                    .AddAction(new Hit(btBuilder.blackboard), btBuilder.actionManager)
                    .AddAction(new Wait(movementType.groggyTime, () => canBeDamaged == false), btBuilder.actionManager)
                .EndComposite()
                #endregion
                #region Boss Pattern Sequence
                .AddAttackSequence()
                    .AddCondition(() => IsTargetValid())
                    .AddAttackSelector()
                        #region Boss Track Sequence
                        .AddSequence()
                            .AddCondition(() => !IsInRange(20f))
                            .AddAction(new Track(btBuilder.blackboard), btBuilder.actionManager)
                        .EndComposite()
                        #endregion
                        #region Boss Dash Sequence
                        .AddAttackSequence()
                            .AddCondition(() => !IsInRange(10f))
                            .AddRandomAttackSelector()
                                .AddAttackSequence()
                                    .AddAction(new Dash(btBuilder.blackboard), btBuilder.actionManager)
                                    .AddCondition(() => RandomExecute(0.5f))
                                    .AddAction(new SwordAttack(btBuilder.blackboard), btBuilder.actionManager)
                                .EndComposite()
                                .AddAction(new CastSpell(btBuilder.blackboard), btBuilder.actionManager)
                            .EndComposite()
                            .AddSelector()
                                .AddSequence()
                                    .AddCondition(() => !IsInRange(10f))
                                    .AddAction(new ResetNode(), btBuilder.actionManager)
                                .EndComposite()
                                 .AddAction(new Track(btBuilder.blackboard), btBuilder.actionManager)
                            .EndComposite()
                        .EndComposite()
                        #endregion
                        #region Boss Track Sword Attack Sequence
                        .AddAttackSequence()
                            .AddAction(new Track(btBuilder.blackboard), btBuilder.actionManager)
                            .AddAction(new SwordAttack(btBuilder.blackboard), btBuilder.actionManager)
                        .EndComposite()
                        #endregion
                    .EndComposite()
                .EndComposite()
                #endregion
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
        //Debug.Log(Mathf.Abs(target.transform.position.x - transform.position.x));
        return distance >= Mathf.Abs(target.transform.position.x - transform.position.x);
    }
    private bool RandomExecute(float chances)
    {
        return Random.Range(0f, 1f) <= chances;
    }

    private bool IsTargetValid()
    {
        return target != null && IsTargetOnWayPoints() == true && target.isDead == false;
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
