using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<LivingEntity>() != null)
        {
            collision.gameObject.GetComponent<LivingEntity>().KillInstant();
        }
    }
}
