using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSkeleton : Enemy
{
    BehaviorTree bt;

    [Header("Shield Skeleton Components")]
    [SerializeField] [Range(0f,1f)] private float blockChances;
    private bool isBlock;
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
    public override bool TakeDamage(float dmg, GameObject damager, bool isHardAttack = false)
    {
        if (base.ApplyDamage(dmg, damager) == false)
        {
            return false;
        }
        else
        {
            healthBar.gameObject.SetActive(true);
            spriteRenderer.color = rageColor;

            this.isHardAttack = isHardAttack;
            isBlock = false;

            Player player = damager.GetComponent<Player>();

            if (player != null)
            {
                target = damager.GetComponent<Player>();
            }

            float blockChances = this.blockChances;

            if(Mathf.Sign(hitDir) == Mathf.Sign(transform.right.x))
            {
                blockChances /= 2f;
            }

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
                    .AddAttackSelector()
                        .AddAttackSequence()
                            .AddCondition(() => isBlock == true)
                            .AddAction(new Block(blackboard))
                        .EndComposite()
                        .AddAttackSequence()
                            .AddAction(new Hit(blackboard))
                            .AddAction(new Wait(movementType.groggyTime, () => canBeDamaged == false))
                        .EndComposite()
                    .EndComposite()
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
    }
#endif
    #endregion
}