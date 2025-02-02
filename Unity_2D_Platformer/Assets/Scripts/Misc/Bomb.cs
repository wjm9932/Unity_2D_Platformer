using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour, IInteractable
{
    [SerializeField] private float timer;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask target;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private float timeElapsed;
    private bool isTrigger;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        isTrigger = false;
        timeElapsed = 0f;
    }

    void Update()
    {
        
    }

    public void Trigger()
    {
        if(isTrigger == true)
        {
            return;
        }
        StartCoroutine(ActivateBomb());
    }

    private IEnumerator ActivateBomb()
    {
        isTrigger = true;
        while (timeElapsed < timer)
        {
            timeElapsed += Time.deltaTime;
            float progress = timeElapsed / timer;
            spriteRenderer.color = Color.Lerp(Color.white, Color.red, progress);
            yield return null; 
        }

        Explode();
        spriteRenderer.color = Color.white;
        animator.enabled = true;

        yield return new WaitUntil(() => IsAnimationFinished());

        Destroy(gameObject);
    }

    private void Explode()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRange, target);

        foreach(var collider in hitColliders)
        {
            if(collider.GetComponent<Enemy>() != null)
            {
                collider.GetComponent<Enemy>().TakeDamage(10f, this.gameObject);
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

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
#endif
}
