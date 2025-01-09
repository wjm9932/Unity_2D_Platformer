using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : LivingEntity
{
    [Space(20)]
    [Header("Enemy Components")]

    [Header("Health")]
    [SerializeField] public HealthBar healthBar;
    [SerializeField] private float _hp;
    [SerializeField] protected float maxHp;
    protected float hp
    {
        set
        {
            _hp = value;
            healthBar.UpdateHealthBar(_hp, maxHp);
        }
        private get
        {
            return _hp;
        }
    }

    [Header("Movement  Type")]
    public EnemyMovementTypeSO movementType;

    [Header("Collision Box")]
    [SerializeField] private GameObject collisionBox;

    [Header("Track Color")]
    [SerializeField] public Color rageColor;

    public LivingEntity target { get; set; }
    public float patrolPoint_1 { get; private set; }
    public float patrolPoint_2 { get; private set; }

    public Rigidbody2D rb { get; private set; }
    public float stopDistance { get; private set; }

    private EnemyStateMachine enemyStateMachine;
    private void Awake()
    {
        stopDistance = (GetComponent<BoxCollider2D>().size.x / 2f) + transform.localScale.x;
        rb = GetComponent<Rigidbody2D>();
        enemyStateMachine = new EnemyStateMachine(this);
    }

    void Start()
    {
        lastTimeDamaged = Time.time - timeBetDamaged;

        hp = maxHp;
        enemyStateMachine.ChangeState(enemyStateMachine.patrolState);
    }

    // Update is called once per frame
    void Update()
    {
        enemyStateMachine.Update();
    }

    private void FixedUpdate()
    {
        enemyStateMachine.FixedUpdate();
    }

    public override bool ApplyDamage(float dmg, LivingEntity damager)
    {
        if (base.ApplyDamage(dmg, damager) == false)
        {
            return false;
        }
        else
        {
            hp -= dmg;
            target = damager;

            if (hp <= 0f)
            {
                Die();
            }
            else
            {
                enemyStateMachine.ChangeState(enemyStateMachine.enemyHitState);
            }

            return true;
        }
    }

    public override void OnAnimationEnterEvent()
    {
        enemyStateMachine.OnAnimationEnterEvent();
    }
    public override void OnAnimationTransitionEvent()
    {
        enemyStateMachine.OnAnimationTransitionEvent();
    }
    public override void OnAnimationExitEvent()
    {
        enemyStateMachine.OnAnimationExitEvent();
    }
    public override void Die()
    {
        base.Die();

        GetComponent<Collider2D>().enabled = false;
        collisionBox.SetActive(false);
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;

        StartCoroutine(FadeAndDestroy(1.5f));

        enemyStateMachine.ChangeState(enemyStateMachine.enemyDieState);
    }

    private IEnumerator FadeAndDestroy(float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);

            yield return null; 
        }

        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f);

        Destroy(gameObject);
    }

    public void SetPatrolPoints(float x1, float x2)
    {
        patrolPoint_1 = x1;
        patrolPoint_2 = x2;
    }
    public override void KillInstant()
    {
        hp = 0;
        Die();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<Player>();

        if (player != null && canBeDamaged == true)
        {
            player.ApplyDamage(dmg, this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<Player>();

        if (player != null && canBeDamaged == true)
        {
            player.ApplyDamage(dmg, this);
        }
    }
}
