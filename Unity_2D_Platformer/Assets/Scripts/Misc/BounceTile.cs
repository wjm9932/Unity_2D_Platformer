using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceTile : MonoBehaviour
{
    [SerializeField] Vector2 bounceForce;
    private float validYpos;
    private void Awake()
    {
        validYpos = (GetComponent<BoxCollider2D>().offset.y + GetComponent<BoxCollider2D>().size.y / 2) * transform.localScale.y;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            if (player.transform.position.y - player.playerFootOffset >= transform.position.y + validYpos)
            {
                BouncePlayer(collision.gameObject.GetComponent<Player>());
                SoundManager.Instance.PlaySoundEffect(SoundManager.InGameSoundEffectType.BOUNCE_PAD, 0.3f);
            }
        }
    }

    private void BouncePlayer(Player player)
    {
        var forceDir = Vector2.right * bounceForce.x + Vector2.up * bounceForce.y;
        if (Mathf.Abs(player.rb.velocity.y) > 0)
            forceDir.y -= player.rb.velocity.y;

        player.rb.AddForce(forceDir, ForceMode2D.Impulse);
    }
}
