using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Patrol Point")]
    [SerializeField] private Transform[] _wayPoints = new Transform[2];
    public Transform[] wayPoints
    {
        get { return _wayPoints; }
    }

    [Header("Enemy info")]
    [SerializeField] private TargetEnemiesInfo[] info;

    [Header("Spawn Control")]
    [SerializeField] private bool enableContinuousSpawn = true;
    [SerializeField] private bool enableStartSpawn = true;

    [Header("Drop Item (Optional)")]
    [SerializeField] private GameObject dropItem;

    [System.Serializable]
    private struct TargetEnemiesInfo
    {
        public GameObject targetEnemy;
        public int targetCount;
    }

    void Start()
    {
        if (enableStartSpawn == true)
        {
            for (int i = 0; i < info.Length; i++)
            {
                for (int j = 0; j < info[i].targetCount; j++)
                {
                    SpawnEnemy(info[i].targetEnemy);
                }
            }
        }
    }

    public void SpawnRandomEnemy(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 spawnPosition = GetSpawnPosition();
            int randEnemyIndex = Random.Range(0, info.Length);

            Enemy enemy = Instantiate(info[randEnemyIndex].targetEnemy, spawnPosition, Quaternion.identity).GetComponent<Enemy>();
            enemy.SetPatrolPoints(_wayPoints[0].position.x, _wayPoints[1].position.x, transform.position.y);
        }
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        Vector2 spawnPosition = GetSpawnPosition();

        Enemy enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity).GetComponent<Enemy>();
        enemy.SetPatrolPoints(_wayPoints[0].position.x, _wayPoints[1].position.x, transform.position.y);

        if(dropItem != null)
        {
            enemy.AddDropItem(dropItem);
        }

        if (enableContinuousSpawn == true)
        {
            enemy.onDeath += () => { StartCoroutine(SpawnEnemyAfterDelay(enemyPrefab, 3f)); };
        }
    }

    private Vector2 GetSpawnPosition()
    {
        return new Vector2(Random.Range(_wayPoints[0].position.x, _wayPoints[1].position.x), transform.position.y);
    }

    private IEnumerator SpawnEnemyAfterDelay(GameObject enemyPrefab, float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnEnemy(enemyPrefab);
    }
}
