using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : LivinEntity
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
         var player = collision.gameObject.GetComponent<Player>();

        if(player != null)
        {
            player.ApplyDamage(dmg);
        }
    }
}
