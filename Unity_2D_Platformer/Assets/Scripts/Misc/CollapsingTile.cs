using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using System.Collections;

public class CollapsingTile : MonoBehaviour
{
    public float collapseDelay = 0.5f;
    public float respawnDelay = 3f;

    [SerializeField] private bool isRespawnTile = true;

    private Dictionary<Vector3Int, TileBase> removedTiles = new Dictionary<Vector3Int, TileBase>();
    private HashSet<Vector3Int> activeTiles = new HashSet<Vector3Int>();
    private Tilemap tilemap;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 contactPoint = collision.GetContact(0).point;
            Vector3Int tilePosition = tilemap.WorldToCell(contactPoint);

            HashSet<Vector3Int> connectedTiles = GetConnectedTiles(tilePosition);

            if (!HasActiveTiles(connectedTiles))
            {
                StartCoroutine(CollapseTiles(connectedTiles));
            }
        }
    }

    private IEnumerator CollapseTiles(HashSet<Vector3Int> connectedTiles)
    {
        foreach (var position in connectedTiles)
        {
            activeTiles.Add(position);
            if (tilemap.HasTile(position))
            {
                removedTiles[position] = tilemap.GetTile(position);
            }
        }

        yield return FadeTiles(connectedTiles);

        foreach (var position in connectedTiles)
        {
            tilemap.SetTile(position, null);
        }

        if(isRespawnTile == true)
        {
            yield return new WaitForSeconds(respawnDelay);
            yield return StartCoroutine(RespawnTiles(connectedTiles));
        }

        foreach (var position in connectedTiles)
        {
            activeTiles.Remove(position);
        }
    }

    private HashSet<Vector3Int> GetConnectedTiles(Vector3Int startTilePosition)
    {
        HashSet<Vector3Int> connectedTiles = new HashSet<Vector3Int>();
        Queue<Vector3Int> toProcess = new Queue<Vector3Int>();

        connectedTiles.Add(startTilePosition);
        toProcess.Enqueue(startTilePosition);

        Vector3Int[] directions = new Vector3Int[]
        {
            Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right
        };

        while (toProcess.Count > 0)
        {
            Vector3Int current = toProcess.Dequeue();

            foreach (var direction in directions)
            {
                Vector3Int neighbor = current + direction;

                if (!connectedTiles.Contains(neighbor) && tilemap.HasTile(neighbor))
                {
                    connectedTiles.Add(neighbor);
                    toProcess.Enqueue(neighbor);
                }
            }
        }

        return connectedTiles;
    }

    private bool HasActiveTiles(HashSet<Vector3Int> tiles)
    {
        foreach (var position in tiles)
        {
            if (activeTiles.Contains(position))
            {
                return true; 
            }
        }
        return false;
    }

    private IEnumerator FadeTiles(HashSet<Vector3Int> tiles)
    {
        float elapsed = 0f;

        while (elapsed < collapseDelay)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / collapseDelay);

            foreach (var position in tiles)
            {
                if (tilemap.HasTile(position))
                {
                    tilemap.SetTileFlags(position, TileFlags.None);
                    Color tileColor = tilemap.GetColor(position);
                    tileColor.a = alpha;
                    tilemap.SetColor(position, tileColor);
                }
            }

            yield return null;
        }
    }

    private IEnumerator RespawnTiles(HashSet<Vector3Int> tiles)
    {
        foreach (var position in tiles)
        {
            if (removedTiles.TryGetValue(position, out TileBase tile))
            {
                tilemap.SetTile(position, tile);
                tilemap.SetTileFlags(position, TileFlags.None);
                tilemap.SetColor(position, new Color(1f, 1f, 1f, 0f)); 
                removedTiles.Remove(position);
            }
        }

        float elapsed = 0f;
        float fadeInDuration = 0.1f;

        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsed / fadeInDuration);

            foreach (var position in tiles)
            {
                if (tilemap.HasTile(position))
                {
                    Color tileColor = tilemap.GetColor(position);
                    tileColor.a = alpha;
                    tilemap.SetColor(position, tileColor);
                }
            }

            yield return null;
        }
    }
}
