using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSkeleton : Enemy
{
    private BehaviorTreeBuilder btBuilder;
    private CompositeNode root;

    [Header("Shield Skeleton Components")]
    [SerializeField] [Range(0f,1f)] private float blockChances;
    private bool isBlock;
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
            isBlock = false;
            target = damager.GetComponent<LivingEntity>();

            if (Random.Range(0f, 1f) <= blockChances)
            {
                isBlock = true;
            }
            else
            {
                hp -= dmg;
            }

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

        root = btBuilder
            .AddSelector()
                .AddSequence()
                    .AddCondition(() => isDead == true)
                    .AddAction(new Die(btBuilder.blackboard), btBuilder.actionManager)
                .EndComposite()
                .AddAttackSequence()
                    .AddCondition(() => canBeDamaged == false)
                    .AddAttackSelector()
                        .AddAttackSequence()
                            .AddCondition(() => isBlock == true)
                            .AddAction(new Block(btBuilder.blackboard), btBuilder.actionManager)
                        .EndComposite()
                        .AddAttackSequence()
                            .AddAction(new Hit(btBuilder.blackboard), btBuilder.actionManager)
                            .AddAction(new Wait(movementType.groggyTime, () => canBeDamaged == false), btBuilder.actionManager)
                        .EndComposite()
                    .EndComposite()
                .EndComposite()
                .AddAttackSequence()
                    .AddCondition(() => target != null)
                    .AddAction(new Track(btBuilder.blackboard), btBuilder.actionManager)
                    .AddAction(new SwordAttack(btBuilder.blackboard), btBuilder.actionManager)
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