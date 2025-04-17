using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherEnemy : Enemy
{
    private BehaviorTree bt;


    [Header("Archer Enemy Component")]
    [SerializeField] private GameObject rangeWeapon;
    [SerializeField] private float attackCoolTime;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        BuildBT();
        trackStopDistance = patrolStopDistance + movementType.trackStopDistance;
    }

    void Update()
    {
        bt.blackboard.SetData<float>("attackCoolTime", bt.blackboard.GetData<float>("attackCoolTime") + Time.deltaTime);
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
        blackboard.SetData<float>("attackCoolTime", attackCoolTime);
        blackboard.SetData<GameObject>("arrow", rangeWeapon);

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
                .AddSequence()
                    .AddCondition(()=> target != null)
                    .AddSelector()
                        .AddAttackSequence()
                            .AddCondition(() => blackboard.GetData<float>("attackCoolTime") >= attackCoolTime)
                            .AddAction(new Track(blackboard))
                            .AddAction(new RangeAttack(blackboard))
                        .EndComposite()
                        .AddSequence()
                            .AddAction(new Track(blackboard))
                            .AddAction(new WaitUntilCoolTime(blackboard))
                        .EndComposite()
                    .EndComposite()
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
