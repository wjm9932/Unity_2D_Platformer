using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class PushableObject : MonoBehaviour, IPoolableObject, IInteractable
{
    public IObjectPool<GameObject> pool { get; private set; }

    private Rigidbody2D pushingRigidbody;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Color originColor;
    private const float fadeDuration = 0.5f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originColor = spriteRenderer.color;
    }

    private void OnEnable()
    {
        spriteRenderer.color = originColor;
        rb.isKinematic = false;
    }

    private void Update()
    {
        if (pushingRigidbody != null)
        {
            if (pushingRigidbody.GetComponent<Player>() != null && pushingRigidbody.GetComponent<Player>().movementStateMachine.jsm.currentState != pushingRigidbody.GetComponent<Player>().movementStateMachine.jsm.wallJumpState && pushingRigidbody.GetComponent<Player>().movementStateMachine.jsm.currentState != pushingRigidbody.GetComponent<Player>().movementStateMachine.jsm.jumpState)
            {
                rb.velocity = new Vector2(pushingRigidbody.velocity.x, rb.velocity.y);
            }
        }
    }

    public void SetPool(IObjectPool<GameObject> pool)
    {
        this.pool = pool;
    }

    public void Release()
    {
        pool.Release(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() == null && collision.gameObject.GetComponent<PushableObject>() == null) return;

        if (pushingRigidbody != null) return;

        var otherRb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (otherRb == null) return;

        if (collision.contacts.Any(contact => Mathf.Abs(contact.normal.y) > 0f))
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (collision.gameObject.GetComponent<Player>() != null)
        {
            if (collision.gameObject.GetComponent<Player>().movementStateMachine.jsm.currentState == collision.gameObject.GetComponent<Player>().movementStateMachine.jsm.jumpState || collision.gameObject.GetComponent<Player>().movementStateMachine.jsm.currentState == collision.gameObject.GetComponent<Player>().movementStateMachine.jsm.wallJumpState)
            {
                return;
            }
        }

        pushingRigidbody = otherRb;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() == null && collision.gameObject.GetComponent<PushableObject>() == null) return;

        var otherRb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (otherRb == null) return;

        if (otherRb == pushingRigidbody)
        {
        }
        pushingRigidbody = null;
        rb.velocity = new Vector2(0f, rb.velocity.y);
    }

    public void Trigger()
    {
        if (rb.isKinematic == false)
        {
            StartCoroutine(FadeOutAndDestroy());
        }
    }

    private IEnumerator FadeOutAndDestroy()
    {
        rb.isKinematic = true;
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            SetAlpha(alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        SetAlpha(0f);

        if (pool == null)
        {
            Destroy(gameObject);
        }
        else
        {
            Release();
        }
    }

    private void SetAlpha(float alpha)
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }
}
