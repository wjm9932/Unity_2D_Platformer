using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObjectHandler : MonoBehaviour
{
    [Header("Interval")]
    [SerializeField] private float minSpawnInterval;
    [SerializeField] private float maxSpawnInterval;

    [Header("Prefab")]
    [SerializeField] private GameObject fallingObjectPrefab;

    [Header("Velocity")]
    [SerializeField] private float velocity = 0f;


    private float timeElapsed = 0f;
    private float spawnTimer = 0f;
    private void Start()
    {
        spawnTimer = SetRandomSpawnTime();
    }

    public void DropProjectile(Vector2 spawnPosition)
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= spawnTimer)
        {
            SpawnProjectile(spawnPosition, velocity);

            timeElapsed = 0f;
            spawnTimer = SetRandomSpawnTime();
        }
    }
    private void SpawnProjectile(Vector2 spawnPosition, float velocity)
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
