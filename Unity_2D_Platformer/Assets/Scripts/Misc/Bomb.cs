using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bomb : MonoBehaviour, IInteractable, IPoolableObject
{
    [SerializeField] private float timer;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask target;

    public IObjectPool<GameObject> pool { get; private set; }

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private float timeElapsed;
    private bool isTriggered;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        isTriggered = false;
        timeElapsed = 0f;
    }

    void Update()
    {

    }

    public void Trigger()
    {
        if (isTriggered == true)
        {
            return;
        }
        StartCoroutine(ActivateBomb());
    }

    private IEnumerator ActivateBomb()
    {
        isTriggered = true;
        while (timeElapsed < timer)
        {
            timeElapsed += Time.deltaTime;
            float progress = timeElapsed / timer;
            spriteRenderer.color = Color.Lerp(Color.white, Color.red, progress);
            yield return null;
        }

        Explode();
        SoundManager.Instance.PlaySoundEffect(SoundManager.InGameSoundEffectType.EXPLODE, GetComponent<AudioSource>());
        spriteRenderer.color = Color.white;
        animator.enabled = true;

        yield return new WaitUntil(() => IsAnimationFinished());

        if (pool == null)
        {
            Destroy(gameObject);
        }
        else
        {
            Release();
        }
    }

    private void Explode()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRange, target);

        foreach (var collider in hitColliders)
        {
            if (collider.GetComponent<Enemy>() != null)
            {
                collider.GetComponent<Enemy>().TakeDamage(25f, this.gameObject);
            }
            else
            {
                collider.GetComponent<Player>().TakeDamage(this.gameObject, true);
            }
        }
    }

    public bool IsAnimationFinished()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        return stateInfo.normalizedTime >= 1f && !animator.IsInTransition(0);
    }

    public void SetPool(IObjectPool<GameObject> pool)
    {
        this.pool = pool;
    }

    public void Release()
    {
        pool.Release(gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
#endif
}
