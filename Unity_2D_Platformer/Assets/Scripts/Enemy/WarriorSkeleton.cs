using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSkeleton : Enemy
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
        trackStopDistance = patrolStopDistance + 1.5f;
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
            hp -= dmg;
            target = damager.GetComponent<LivingEntity>();

            if (hp <= 0f)
            {
                Die();
            }
            else
            {
                btBuilder.blackboard.SetData<bool>("isHit", true);
            }
            return true;
        }
    }
    private void BuildBT()
    {
        btBuilder.blackboard.SetData<Enemy>("owner", this);
        btBuilder.blackboard.SetData<bool>("isHit", false);
        btBuilder.blackboard.SetData<bool>("isPlayerOnSight", false);

        root = btBuilder
            .AddSelector()
                .AddSequence()
                    .AddCondition(() => isDead == true)
                    .AddAction(new Die(btBuilder.blackboard), btBuilder.actionManager)
                .EndComposite()
                .AddSequence()
                    .AddCondition(() => btBuilder.blackboard.GetData<bool>("isHit"))
                    .AddAction(new Hit(btBuilder.blackboard), btBuilder.actionManager)
                .EndComposite()
                .AddAttackSequence(true)
                    .AddCondition(() => target != null)
                    .AddAttackSequence()
                        .AddAction(new Track(btBuilder.blackboard), btBuilder.actionManager)
                        .AddAction(new SwordAttack(btBuilder.blackboard), btBuilder.actionManager)
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
