using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour, IPoolableObject
{
    public IObjectPool<GameObject> pool { get; private set; }

    private float velocity;
    private float targetDistance;
    private Rigidbody2D rb;
    private Vector2 startPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, startPosition) >= targetDistance)
        {
            Release();
        }
    }

    public void SetPool(IObjectPool<GameObject> pool)
    {
        this.pool = pool;
    }

    public void Initialize(Vector2 position, Quaternion rotation, Transform parent = null)
    {
        startPosition = position;
        transform.position = position;
        transform.rotation = rotation;
    }

    public void SetTargetDistanceAndVelocity(float distance, float velocity)
    {
        targetDistance = distance;
        rb.velocity = velocity * transform.right;
    }

    public void Release()
    {
        pool.Release(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(this.gameObject);
            pool.Release(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            if(collision.GetComponent<Player>().TakeDamage(this.gameObject) == true)
            {
                pool.Release(gameObject);
            }
        }
    }
}
