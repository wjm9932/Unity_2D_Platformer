using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherEnemy : Enemy
{
    private BehaviorTreeBuilder btBuilder;
    private CompositeNode root;

    [Header("Archer Enemy Component")]
    [SerializeField] private GameObject rangeWeapon;
    [SerializeField] private float attackCoolTime;
    protected override void Awake()
    {
        base.Awake();
        btBuilder = GetComponent<BehaviorTreeBuilder>();
    }

    protected override void Start()
    {
        base.Start();
        BuildBT();
        trackStopDistance = patrolStopDistance + movementType.trackStopDistance;
    }

    void Update()
    {
        btBuilder.blackboard.SetData<float>("attackCoolTime", btBuilder.blackboard.GetData<float>("attackCoolTime") + Time.deltaTime);
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


    private void BuildBT()
    {
        btBuilder.blackboard.SetData<Enemy>("owner", this);
        btBuilder.blackboard.SetData<float>("attackCoolTime", attackCoolTime);
        btBuilder.blackboard.SetData<GameObject>("arrow", rangeWeapon);

        root = btBuilder
            .AddSelector()
                .AddSequence()
                    .AddCondition(() => isDead == true)
                    .AddAction(new Die(btBuilder.blackboard), btBuilder.actionManager)
                .EndComposite()
                .AddAttackSequence()
                    .AddCondition(() => canBeDamaged == false)
                    .AddAction(new Hit(btBuilder.blackboard), btBuilder.actionManager)
                    .AddAction(new Wait(movementType.groggyTime, () => canBeDamaged == false), btBuilder.actionManager)
                .EndComposite()
                .AddSequence()
                    .AddCondition(()=> target != null)
                    .AddSelector()
                        .AddAttackSequence()
                            .AddCondition(() => btBuilder.blackboard.GetData<float>("attackCoolTime") >= attackCoolTime)
                            .AddAction(new Track(btBuilder.blackboard), btBuilder.actionManager)
                            .AddAction(new RangeAttack(btBuilder.blackboard), btBuilder.actionManager)
                        .EndComposite()
                        .AddSequence()
                            .AddAction(new Track(btBuilder.blackboard), btBuilder.actionManager)
                            .AddAction(new WaitUntilCoolTime(btBuilder.blackboard), btBuilder.actionManager)
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
