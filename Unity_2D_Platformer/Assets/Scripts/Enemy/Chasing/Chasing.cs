using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chasing : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb.velocity = rb.transform.right * 8.5f;
    }

    void Update()
    {
        
    }
}
