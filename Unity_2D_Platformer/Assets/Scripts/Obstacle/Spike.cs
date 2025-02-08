using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private bool isKillInstant = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<LivingEntity>() != null)
        {
            if (isKillInstant == true || collision.gameObject.GetComponent<Player>() == null)
            {
                collision.gameObject.GetComponent<LivingEntity>().KillInstant();
            }
            else
            {
                collision.gameObject.GetComponent<Player>().TakeDamage(gameObject, true, false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<LivingEntity>() != null)
        {
            if (isKillInstant == true || collision.gameObject.GetComponent<Player>() == null)
            {
                collision.gameObject.GetComponent<LivingEntity>().KillInstant();
            }
            else
            {
                collision.gameObject.GetComponent<Player>().TakeDamage(gameObject, true, false);
            }
        }
    }
}
