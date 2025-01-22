using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Heart : Item
{
    private void OnEnable()
    {
        enableTime = Time.time;
    }

    protected override void Update()
    {
        base.Update();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            if (collision.GetComponent<Player>().RecoverHealth() == true)
            {
                if (pool != null)
                {
                    Release();
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
