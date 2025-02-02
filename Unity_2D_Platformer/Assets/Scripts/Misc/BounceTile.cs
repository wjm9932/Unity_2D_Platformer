using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BounceTile : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Player>() != null)
        {
            BouncePlayer(collision.gameObject.GetComponent<Player>());
        }
    }

    private void BouncePlayer(Player player)
    {
        var forceDir = (Vector2)transform.right * 50f + Vector2.up * 30f;
        if (Mathf.Abs(player.rb.velocity.y) > 0)
            forceDir.y -= player.rb.velocity.y;

        player.rb.AddForce(forceDir, ForceMode2D.Impulse);
    }
}
