using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PushableObject : MonoBehaviour, IPoolableObject
{
    public IObjectPool<GameObject> pool { get; private set; }

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetPool(IObjectPool<GameObject> pool)
    {
        this.pool = pool;
    }

    public void Initialize(Vector2 position, Quaternion rotation, Transform parent = null)
    {
        transform.position = position;
        transform.rotation = rotation;
    }
    public void Release()
    {
        pool.Release(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            foreach (var contact in collision.contacts)
            {
                if (Mathf.Abs(contact.normal.y) > 0f)
                {
                    rb.velocity = Vector2.zero;
                    return;
                }
            }

            rb.velocity = new Vector2(player.rb.velocity.x, rb.velocity.y);
            player.SetSpeedLimit(0.4f);
        }
        else if (this.gameObject.layer == collision.gameObject.layer)
        {
            rb.velocity = new Vector2(collision.gameObject.GetComponent<Rigidbody2D>().velocity.x, rb.velocity.y);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        rb.velocity = new Vector2(0f, rb.velocity.y);

        var player = collision.gameObject.GetComponent<Player>();
        
        if (player != null)
        {
            player.SetSpeedLimit(0f);
        }
    }

}
