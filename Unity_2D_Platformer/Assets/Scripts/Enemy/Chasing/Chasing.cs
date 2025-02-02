using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chasing : MonoBehaviour
{
    private CircleCollider2D circleCollider;
    private Player player;

    private float radius;
    private float minY;
    private float maxY;
    private float minVelocity = 8.5f;
    private float maxVelocity = 10f;
    public float currentVelocity { get; private set; } = 8.5f; 
    private float velocityLerpSpeed = 2f;

    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        radius = circleCollider.radius * transform.localScale.x;

        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        minY = bottomLeft.y;
        maxY = topRight.y;
    }

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.transform.position);

        float targetVelocity = Mathf.Max(minVelocity, distance * 1.5f);

        currentVelocity = Mathf.Lerp(currentVelocity, targetVelocity, Time.deltaTime * velocityLerpSpeed);
        currentVelocity = Mathf.Clamp(currentVelocity, minVelocity, maxVelocity);

        float newX = transform.position.x + currentVelocity * Time.deltaTime;

        float playerY = player.transform.position.y;
        float currentY = transform.position.y;
        float targetY = Mathf.Lerp(currentY, playerY, 1.5f * Time.deltaTime);

        targetY = Mathf.Clamp(targetY, minY + radius, maxY - radius);

        transform.position = new Vector2(newX, targetY);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            collision.GetComponent<Player>().KillInstant();
        }
    }
}
