using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class PushableObject : MonoBehaviour, IPoolableObject
{
    public IObjectPool<GameObject> pool { get; private set; }

    private Rigidbody2D pushingRigidbody;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (pushingRigidbody != null)
        {
            rb.velocity = new Vector2(pushingRigidbody.velocity.x, rb.velocity.y);
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

        pushingRigidbody = otherRb;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() == null && collision.gameObject.GetComponent<PushableObject>() == null) return;

        var otherRb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (otherRb == null) return;

        if (otherRb == pushingRigidbody)
        {
            pushingRigidbody = null;
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }
}
