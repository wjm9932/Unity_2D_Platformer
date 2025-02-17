using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashItem : Item
{
    protected override void Update()
    {
        base.Update();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            if (collision.GetComponent<Player>().RecoverDashCount() == true)
            {
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
}
