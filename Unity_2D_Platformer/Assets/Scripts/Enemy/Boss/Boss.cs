using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Boss : Enemy
{
    [Space(20)]
    [Header("Boss Components")]
    public GameObject spellPrefab;
    public GameObject bulletPrefab;

    [Header("Enemy Spwaner")]
    [SerializeField] private GameObject enemySpawnerObject;
    public EnemySpawner enemySpawner { get; private set; }

    [Header("Drop prefab")]
    [SerializeField] private GameObject dropBulletPrefab;
    [SerializeField] private GameObject[] dropItemPrefabs;

    public bool isGraceTime { get; set; }

    private FallingObjectHandler bulletDropHandler;
    private FallingObjectHandler itemDropHandler;

    private BehaviorTreeBuilder btBuilder;
    private CompositeNode root;

    protected override void Awake()
    {
        base.Awake();
        bulletDropHandler = new FallingObjectHandler(0.1f, 0.5f, 10f);
        itemDropHandler = new FallingObjectHandler(5f, 6f);

        btBuilder = GetComponent<BehaviorTreeBuilder>();
        enemySpawner = enemySpawnerObject.GetComponent<EnemySpawner>();
    }

    protected override void Start()
    {
        base.Start();
        BuildBT();

        isGraceTime = false;
        trackStopDistance = patrolStopDistance + movementType.trackStopDistance * transform.localScale.x;

        SetPatrolPoints(enemySpawner.wayPoints[0].position.x, enemySpawner.wayPoints[1].position.x, transform.position.y);
    }

    void Update()
    {
        if(target != null && isDead == false)
        {
            bulletDropHandler.TrySpawnProjectile(dropBulletPrefab, new Vector2(Random.Range(patrolPoint_1, patrolPoint_2), target.transform.position.y + 10f));

            var randItem = Random.Range(0, dropItemPrefabs.Length);
            itemDropHandler.TrySpawnProjectile(dropItemPrefabs[randItem], new Vector2(Random.Range(patrolPoint_1, patrolPoint_2), target.transform.position.y + 10f));
        }

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

    public override bool TakeDamage(float dmg, GameObject damager, bool isHardAttack = false)
    {
        if (isGraceTime == true)
        {
            return false;
        }
        else if (base.ApplyDamage(dmg, damager) == false)
        {
            return false;
        }
        else
        {
            healthBar.gameObject.SetActive(true);
            spriteRenderer.color = rageColor;

            this.isHardAttack = isHardAttack;
            target = damager.GetComponent<Player>();

            hp -= dmg;

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
        btBuilder.blackboard.SetData<Boss>("owner", this);
        btBuilder.blackboard.SetData<bool>("IsCasting", false);
        btBuilder.blackboard.SetData<EnemySpawner>("enemySpawner", enemySpawner);

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
                    .AddCondition(() => canBeDamaged == false && (isHardAttack == true || btBuilder.blackboard.GetData<bool>("IsCasting") == true))
                    .AddAction(new Hit(btBuilder.blackboard), btBuilder.actionManager)
                    .AddAction(new Wait(movementType.groggyTime, () => canBeDamaged == false), btBuilder.actionManager)
                    .AddCondition(() => RandomExecute(0.5f))
                    .AddAttackSelector()
                        .AddAttackSequence()
                            .AddCondition(() => RandomExecute(0.8f))
                            .AddAction(new SetUpForShooting(btBuilder.blackboard), btBuilder.actionManager)
                            .AddAction(new Shoot(btBuilder.blackboard), btBuilder.actionManager)
                        .EndComposite()
                        .AddAttackSequence()
                            .AddAction(new Teleport(btBuilder.blackboard), btBuilder.actionManager)
                            .AddAction(new SpawnEnemy(btBuilder.blackboard), btBuilder.actionManager)
                        .EndComposite()
                    .EndComposite()
                .EndComposite()
                #endregion
                #region Boss Pattern Sequence
                .AddAttackSequence()
                    .AddCondition(() => IsTargetValid())
                    .AddAttackSelector()
                        #region Boss Track Sequence
                        .AddSequence()
                            .AddCondition(() => !IsInRange(20f))
                            .AddAction(new BossTrack(btBuilder.blackboard), btBuilder.actionManager)
                        .EndComposite()
                        #endregion
                        #region Boss Range Attack Pattern Sequence
                        .AddAttackSequence()
                            .AddCondition(() => !IsInRange(10f))
                            .AddRandomAttackSelector()
                                .AddAttackSequence()
                                    .AddAction(new Dash(btBuilder.blackboard), btBuilder.actionManager)
                                    .AddCondition(() => RandomExecute(0.8f))
                                    .AddAction(new SwordAttack(btBuilder.blackboard), btBuilder.actionManager)
                                .EndComposite()
                                .AddAction(new CastSpell(btBuilder.blackboard), btBuilder.actionManager)
                            .EndComposite()
                            .AddSelector()
                                .AddSequence()
                                    .AddCondition(() => !IsInRange(10f))
                                    .AddAction(new ResetNode(), btBuilder.actionManager)
                                .EndComposite()
                                 .AddAction(new BossTrack(btBuilder.blackboard), btBuilder.actionManager)
                            .EndComposite()
                        .EndComposite()
                        #endregion
                        #region Boss Close Attack Sequence
                        .AddSequence()
                            .AddRandomAttackSelector()
                                .AddAttackSequence()
                                    .AddAction(new BossTrack(btBuilder.blackboard), btBuilder.actionManager)
                                    .AddAction(new SwordAttack(btBuilder.blackboard), btBuilder.actionManager)
                                .EndComposite()
                                .AddAttackSequence()
                                    .AddAction(new Teleport(btBuilder.blackboard), btBuilder.actionManager)
                                    .AddAction(new CastSpell(btBuilder.blackboard), btBuilder.actionManager)
                                    .AddAction(new CastSpell(btBuilder.blackboard), btBuilder.actionManager)
                                    .AddAction(new CastSpell(btBuilder.blackboard), btBuilder.actionManager)
                                .EndComposite()
                            .EndComposite()
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
        return distance >= Mathf.Abs(target.transform.position.x - transform.position.x);
    }
    private bool RandomExecute(float chances)
    {
        return Random.Range(0f, 1f) <= chances;
    }

    private bool IsTargetValid()
    {
        return target != null && target.isDead == false;
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
