using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy, ITargetHandler
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

    [SerializeField] GameObject key;

    public bool isGraceTime { get; set; }

    private FallingObjectHandler bulletDropHandler;
    private FallingObjectHandler itemDropHandler;

    private BehaviorTree bt;

    public float yPos { get; private set; }

    public Transform[] bossRange { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        bulletDropHandler = new FallingObjectHandler(0.1f, 0.5f, 10f);
        itemDropHandler = new FallingObjectHandler(5f, 6f);

        enemySpawner = enemySpawnerObject.GetComponent<EnemySpawner>();
    }

    protected override void Start()
    {
        base.Start();
        BuildBT();
        yPos = transform.position.y;
        isGraceTime = false;
        trackStopDistance = patrolStopDistance + movementType.trackStopDistance * transform.localScale.x;
        bossRange = enemySpawner.wayPoints;
    }

    void Update()
    {
        if (target != null && isDead == false)
        {
            bulletDropHandler.TrySpawnProjectile(dropBulletPrefab, new Vector2(Random.Range(bossRange[0].transform.position.x, bossRange[1].transform.position.x), target.transform.position.y + 10f));

            var randItem = Random.Range(0, dropItemPrefabs.Length);
            itemDropHandler.TrySpawnProjectile(dropItemPrefabs[randItem], new Vector2(Random.Range(bossRange[0].transform.position.x, bossRange[1].transform.position.x), target.transform.position.y + 20f));
        }

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

    public override void Die()
    {
        base.Die();
        Instantiate(key, transform.position, key.transform.rotation);
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
            spriteRenderer.color = rageColor;
            this.isHardAttack = isHardAttack;

            hp -= dmg;

            if (hp <= 0f)
            {
                Die();
            }

            SoundManager.Instance.PlaySoundEffect(SoundManager.InGameSoundEffectType.ENEMY_HIT, 0.7f);

            return true;
        }
    }

    private void BuildBT()
    {
        Blackboard blackboard = new Blackboard();

        blackboard.SetData<Enemy>("owner", this);
        blackboard.SetData<Boss>("owner", this);
        blackboard.SetData<bool>("IsCasting", false);
        blackboard.SetData<EnemySpawner>("enemySpawner", enemySpawner);

        bt = new BehaviorTreeBuilder(blackboard)
            .AddSelector()
        #region Die Sequence
                .AddSequence()
                    .AddCondition(() => isDead == true)
                    .AddAction(new Die(blackboard))
                .EndComposite()
        #endregion
        #region Hit Sequence
                .AddAttackSequence()
                    .AddCondition(() => canBeDamaged == false && (isHardAttack == true || blackboard.GetData<bool>("IsCasting") == true))
                    .AddAction(new Hit(blackboard))
                    .AddAction(new Wait(movementType.groggyTime, () => canBeDamaged == false))
                    .AddCondition(() => RandomExecute(0.65f))
                    .AddAttackSelector()
                        .AddAttackSequence()
                            .AddCondition(() => RandomExecute(0.6f))
                            .AddAction(new SetUpForShooting(blackboard))
                            .AddAction(new Shoot(blackboard))
                        .EndComposite()
                        .AddAttackSequence()
                            .AddAction(new Teleport(blackboard))
                            .AddAction(new SpawnEnemy(blackboard))
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
                            .AddAction(new BossTrack(blackboard))
                        .EndComposite()
        #endregion
        #region Boss Range Attack Pattern Sequence
                        .AddAttackSequence()
                            .AddCondition(() => !IsInRange(10f))
                            .AddRandomAttackSelector()
                                .AddAttackSequence()
                                    .AddAction(new Dash(blackboard))
                                    .AddCondition(() => RandomExecute(0.8f))
                                    .AddAction(new SwordAttack(blackboard))
                                .EndComposite()
                                .AddAction(new CastSpell(blackboard))
                            .EndComposite()
                            .AddSelector()
                                .AddSequence()
                                    .AddCondition(() => !IsInRange(10f))
                                    .AddAction(new ResetNode())
                                .EndComposite()
                                 .AddAction(new BossTrack(blackboard))
                            .EndComposite()
                        .EndComposite()
        #endregion
        #region Boss Close Attack Sequence
                        .AddSequence()
                            .AddRandomAttackSelector()
                                .AddAttackSequence()
                                    .AddAction(new BossTrack(blackboard))
                                    .AddAction(new SwordAttack(blackboard))
                                .EndComposite()
                                .AddAttackSequence()
                                    .AddAction(new Teleport(blackboard))
                                    .AddAction(new CastSpell(blackboard))
                                    .AddAction(new CastSpell(blackboard))
                                    .AddAction(new CastSpell(blackboard))
                                .EndComposite()
                            .EndComposite()
                        .EndComposite()
        #endregion
                    .EndComposite()
                .EndComposite()
        #endregion
                .AddAttackSequence()
                    .AddAction(new Patrol(blackboard))
                    .AddAction(new Idle(blackboard))
                .EndComposite()
            .EndComposite()
            .Build();
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

    public void SetTarget(Player target)
    {
        this.target = target;
        healthBar.gameObject.SetActive(true);
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
