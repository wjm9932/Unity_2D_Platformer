using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTargetTrigger : MonoBehaviour
{
    [SerializeField] private GameObject enemy;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            enemy.GetComponent<ITargetHandler>().SetTarget(collision.GetComponent<Player>());
            gameObject.SetActive(false);
        }
    }

}
