using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Patrol Point")]
    [SerializeField] private Transform[] wayPoints = new Transform[2];

    [Header("Enemy info")]
    [SerializeField] private TargetEnemiesInfo[] info;

    [System.Serializable]
    private struct TargetEnemiesInfo
    {
        public GameObject targetEnemy;
        public int targetCount;
    }

    void Start()
    {
        for (int i = 0; i < info.Length; i++)
        {
            for (int j = 0; j < info[i].targetCount; j++)
            {
                SpawnEnemy(info[i].targetEnemy);
            }
        }
    }

    void Update()
    {
        
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        Vector2 spawnPosition = GetSpawnPosition();

        Enemy enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity).GetComponent<Enemy>();
        //enemy.SetNavMeshArea(areaMask);
        //enemies.Add(enemy);

        //enemy.onDeath += () => { enemies.Remove(enemy); };
        //enemy.onDeath += () => { StartCoroutine(SpawnEnemyAfterDelay(enemyPrefab, 5f)); };
    }

    private Vector2 GetSpawnPosition()
    {
        return new Vector2(Random.Range(wayPoints[0].position.x, wayPoints[1].position.x), transform.position.y);
    }
}
