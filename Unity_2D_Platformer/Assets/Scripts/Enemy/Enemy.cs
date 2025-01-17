using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : LivingEntity
{
    [Space(20)]
    [Header("Enemy Components")]

    [Header("Health")]
    [SerializeField] public HealthBar healthBar;
    [SerializeField] protected float _hp;
    [SerializeField] protected float maxHp;
    protected float hp
    {
        set
        {
            _hp = value;
            healthBar.UpdateHealthBar(_hp, maxHp);
        }
        get
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


    #region LayerMask
    [Header("Layer")]
    public LayerMask targetLayer;
    #endregion

    #region Attack Root
    [Header("Attack Root")]
    public Transform attackRoot;
    public float attackRange;
    #endregion

    #region Drop Item
    [Header("Drop Item")]
    [SerializeField] private GameObject dropItem;
    [SerializeField] [Range(0f, 1f)] private float dropChances;
    #endregion

    public LivingEntity target { get; set; }
    public float patrolPoint_1 { get; private set; }
    public float patrolPoint_2 { get; private set; }

    public Rigidbody2D rb { get; private set; }
    public float patrolStopDistance { get; private set; }
    public float trackStopDistance { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        patrolStopDistance = (GetComponent<BoxCollider2D>().size.x / 2f) * transform.localScale.x;
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void Start()
    {
        lastTimeDamaged = Time.time - timeBetDamaged;
        hp = maxHp;
    }

    public override void Die()
    {
        base.Die();

        DropItem(dropChances);
        GetComponent<Collider2D>().enabled = false;
        collisionBox.SetActive(false);
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;

        StartCoroutine(FadeAndDestroy(1.5f));
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
        base.KillInstant();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<Player>();

        if (player != null && canBeDamaged == true)
        {
            player.ApplyDamage(1, this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<Player>();

        if (player != null && canBeDamaged == true)
        {
            player.ApplyDamage(1, this.gameObject);
        }
    }

    private void DropItem(float chances)
    {
        if(dropItem != null)
        {
            if (Random.Range(0f, 1f) <= chances)
            {
                ObjectPoolManager.Instance.GetPoolableObject(dropItem, transform.position, transform.rotation);
            }
        }
    }
}
