using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Item
{
    protected override void Update()
    {
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            collision.GetComponent<Player>().AcquireKey();
            if (pool != null)
            {
                Release();
            }
            else
            {
                Destroy(gameObject);
            }
            SoundManager.Instance.PlaySoundEffect(SoundManager.InGameSoundEffectType.ITEM_ACQUIRE, 0.4f);
        }
    }
}

