using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Arrow : MonoBehaviour, IPoolableObject
{
    public IObjectPool<GameObject> pool { get; private set; }

    [SerializeField] private float velocity;
    private Rigidbody2D rb;
    private Vector2 startPosition;
    private float targetDistance;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(Vector2.Distance(transform.position, startPosition) >= targetDistance)
        {
            pool.Release(this.gameObject);
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

        rb.velocity = velocity * transform.right;
    }
    public void SetTargetDistance(float distance)
    {
        targetDistance = distance;
    }
    public void Release()
    {
        pool.Release(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            collision.GetComponent<Player>().ApplyDamage(1, this.gameObject);
            pool.Release(gameObject);
        }
    }
}
