using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTile : MonoBehaviour
{
    [SerializeField] private float moveDistance = 5f;
    [SerializeField] private float moveSpeed = 2f;

    private Vector3 startPos;
    private float timeElapsed;

    private void Awake()
    {
    }

    void Start()
    {
        startPos = transform.position;
        timeElapsed = 0f;
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        float offset = Mathf.Sin(timeElapsed * moveSpeed) * moveDistance;
        transform.position = startPos + new Vector3(offset, 0, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(null);
        }
    }
}

