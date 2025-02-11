using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour, IPoolableObject
{
    public IObjectPool<GameObject> pool { get; private set; }

    private float targetDistance;
    private Rigidbody2D rb;
    private Vector2 startPosition;
    private bool isHardAttack;

    private void Awake()
    {
        isHardAttack = false;
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

    public void SetTargetDistanceAndVelocity(Vector2 startPosition, float distance, float velocity, bool isHardAttack = false)
    {
        this.startPosition = startPosition;
        targetDistance = distance;
        rb.velocity = velocity * transform.right;
        this.isHardAttack = isHardAttack;
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
            if(collision.GetComponent<Player>().TakeDamage(this.gameObject, isHardAttack) == true)
            {
                pool.Release(gameObject);
            }
        }
        else if(collision.GetComponent<PushableObject>() != null)
        {
            pool.Release(gameObject);
        }
    }
}
