using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObjectHandler 
{
    private float minSpawnInterval;
    private float maxSpawnInterval;

    private float velocity = 0f;

    private float timeElapsed = 0f;
    private float spawnTimer = 0f;

    public FallingObjectHandler(float minSpawnInterval, float maxSpawnInterval, float velocity = 0f)
    {
        this.minSpawnInterval = minSpawnInterval;
        this.maxSpawnInterval = maxSpawnInterval;
        this.velocity = velocity;
        this.timeElapsed = 0f;
        this.spawnTimer = SetRandomSpawnTime();
    }

    public void TrySpawnProjectile(GameObject fallingObject, Vector2 spawnPosition)
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= spawnTimer)
        {
            SpawnProjectile(fallingObject, spawnPosition, velocity);

            timeElapsed = 0f;
            spawnTimer = SetRandomSpawnTime();
        }
    }
    private void SpawnProjectile(GameObject fallingObjectPrefab, Vector2 spawnPosition, float velocity)
    {
        var bullet = ObjectPoolManager.Instance.GetPoolableObject(fallingObjectPrefab, spawnPosition, fallingObjectPrefab.transform.rotation);

        if (bullet.GetComponent<Projectile>() != null)
        {
            bullet.GetComponent<Projectile>().SetTargetDistanceAndVelocity(150f, velocity);
        }
    }

    private float SetRandomSpawnTime()
    {
        return Random.Range(minSpawnInterval, maxSpawnInterval);
    }
}
