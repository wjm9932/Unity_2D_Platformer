using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObjectHandler : MonoBehaviour
{
    [SerializeField] private float minSpawnInterval;
    [SerializeField] private float maxSpawnInterval;

    [SerializeField] private GameObject fallingObjectPrefab;

    private float timeElapsed = 0f;
    private float spawnTimer = 0f;
    private void Start()
    {
        spawnTimer = SetRandomSpawnTime();
    }

    public void DropProjectile(Vector2 spawnPosition, float velocity = 0f)
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= spawnTimer)
        {
            SpawnBullet(spawnPosition, velocity);

            timeElapsed = 0f;
            spawnTimer = SetRandomSpawnTime();
        }
    }
    private void SpawnBullet(Vector2 spawnPosition, float velocity)
    {
        var bullet = ObjectPoolManager.Instance.GetPoolableObject(fallingObjectPrefab, spawnPosition, fallingObjectPrefab.transform.rotation).GetComponent<Projectile>();
        if (bullet != null)
        {
            bullet.SetTargetDistanceAndVelocity(150f, velocity);
        }
    }
    private float SetRandomSpawnTime()
    {
        return Random.Range(minSpawnInterval, maxSpawnInterval);
    }
}
