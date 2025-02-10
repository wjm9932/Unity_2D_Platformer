using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;


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

    [Header("Attack Sound Tyle")]
    public SoundManager.InGameSoundEffectType attackSoundEffect;

    #region LayerMask
    [Header("Layer")]
    public LayerMask targetLayer;
    public LayerMask whatIsGround;
    #endregion

    #region Attack Root
    [Header("Attack Root")]
    public Transform attackRoot;
    public float attackRange;
    #endregion

    #region Drop Item
    [Header("Drop Item")]
    [SerializeField] private List<GameObject> dropItem = new List<GameObject>();
    [SerializeField] [Range(0f, 1f)] private float dropChances;
    #endregion

    public Player target { get; set; }

    public Rigidbody2D rb { get; private set; }
    public AudioSource audioSource { get; private set; }
    public float patrolStopDistance { get; private set; }
    public float trackStopDistance { get; protected set; }
    [HideInInspector] public bool isHardAttack;


    protected override void Awake()
    {
        base.Awake();
        patrolStopDistance = (GetComponent<BoxCollider2D>().size.x / 2f) * transform.localScale.x + 0.1f;
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    protected override void Start()
    {
        base.Start();
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

    public override void KillInstant()
    {
        hp = 0;
        base.KillInstant();
    }

    public virtual bool TakeDamage(float dmg, GameObject damager, bool isHardAttack = false)
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

            hp -= dmg;

            Player player = damager.GetComponent<Player>();

            if (player != null)
            {
                target = damager.GetComponent<Player>();
            }

            if (hp <= 0f)
            {
                Die();
            }
            return true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<Player>();

        if (player != null && canBeDamaged == true)
        {
            player.TakeDamage(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<Player>();

        if (player != null && canBeDamaged == true)
        {
            player.TakeDamage(this.gameObject);
        }
    }

    private void DropItem(float chances)
    {
        if(dropItem != null)
        {
            if (Random.Range(0f, 1f) <= chances)
            {
                var randItemIndex = Random.Range(0, dropItem.Count);
                if (ObjectPoolManager.Instance.GetPoolableObject(dropItem[randItemIndex], transform.position, transform.rotation) == null)
                {
                    Instantiate(dropItem[randItemIndex], transform.position, transform.rotation);
                }
            }
        }
    }

    public void AddDropItem(GameObject gameObject)
    {
        dropItem.Add(gameObject);
    }
}
