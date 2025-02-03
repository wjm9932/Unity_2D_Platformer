using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BounceTile : MonoBehaviour
{
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
            }
        }
    }

    private void BouncePlayer(Player player)
    {
        var forceDir = (Vector2)transform.right * 30f + Vector2.up * 75f;
        if (Mathf.Abs(player.rb.velocity.y) > 0)
            forceDir.y -= player.rb.velocity.y;

        player.rb.AddForce(forceDir, ForceMode2D.Impulse);
    }
}
