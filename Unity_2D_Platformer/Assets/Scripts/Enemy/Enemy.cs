using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : LivinEntity
{
    public float patrolPoint_1 { get; private set; }
    public float patrolPoint_2 { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
         var player = collision.gameObject.GetComponent<Player>();

        if(player != null)
        {
            player.ApplyDamage(dmg);
        }
    }

    public void SetPatrolPoints(float x1, float x2)
    {
        patrolPoint_1 = x1;
        patrolPoint_2 = x2;
    }
}
