using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTile : MonoBehaviour
{
    [SerializeField] private float moveDistance = 5f;
    [SerializeField] private float moveSpeed = 2f;

    private Vector3 startPos;
    private float timeElapsed;
    private Rigidbody2D rb;
    private Player player;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        startPos = transform.position;
        timeElapsed = 0f;
    }

    void FixedUpdate()
    {
        Vector3 prevPosition = rb.position;

        timeElapsed += Time.fixedDeltaTime;
        float offset = Mathf.Sin(timeElapsed * moveSpeed) * moveDistance;
        Vector3 newPosition = startPos + new Vector3(offset, 0, 0);

        rb.MovePosition(newPosition);


        Vector2 platformVelocity = (newPosition - prevPosition) / Time.fixedDeltaTime;

        if (player != null)
        {
            player.platformVelocity = platformVelocity;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<Player>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.platformVelocity = Vector2.zero;
            player = null;
        }
    }
}

