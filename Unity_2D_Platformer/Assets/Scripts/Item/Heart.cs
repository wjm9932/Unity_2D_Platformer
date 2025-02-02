using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Heart : Item
{
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
