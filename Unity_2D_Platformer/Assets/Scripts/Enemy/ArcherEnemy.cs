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

    public override void Die()
    {
        base.Die();
    }

    public override bool ApplyDamage(float dmg, GameObject damager)
    {
        if (base.ApplyDamage(dmg, damager) == false)
        {
            return false;
        }
        else
        {
            hp -= dmg;
            target = damager.GetComponent<LivingEntity>();

            if (hp <= 0f)
            {
                Die();
            }
            return true;
        }
    }
    private void BuildBT()
    {
        btBuilder.blackboard.SetData<Enemy>("owner", this);
        btBuilder.blackboard.SetData<float>("attackCoolTime", 0f);

        root = btBuilder
            .AddSelector()
                .AddSequence()
                    .AddCondition(() => isDead == true)
                    .AddAction(new Die(btBuilder.blackboard), btBuilder.actionManager)
                .EndComposite()
                .AddAttackSequence()
                    .AddCondition(() => canBeDamaged == false)
                    .AddAttackSequence()
                        .AddAction(new Hit(btBuilder.blackboard), btBuilder.actionManager)
                        .AddAction(new Wait(movementType.groggyTime, () => canBeDamaged == false), btBuilder.actionManager)
                    .EndComposite()
                .EndComposite()
                .AddAttackSequence()
                    .AddCondition(() => target != null)
                    .AddAction(new Track(btBuilder.blackboard), btBuilder.actionManager)
                    .AddCondition(() => btBuilder.blackboard.GetData<float>("attackCoolTime") >= attackCoolTime)
                    .AddAction(new RangeAttack(btBuilder.blackboard), btBuilder.actionManager)
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