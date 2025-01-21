using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObjectHandler : MonoBehaviour
{
    [SerializeField] private GameObject fallingObjectPrefab;
    private float timeElapsed = 0f;
    private float spawnTimer = 0f;
    private void Start()
    {
        spawnTimer = SetRandomSpawnTime();
    }

    public void DropBullets(Vector2 spawnPosition)
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= spawnTimer)
        {
            SpawnBullet(spawnPosition);

            timeElapsed = 0f;
            spawnTimer = SetRandomSpawnTime();
        }
    }
    private void SpawnBullet(Vector2 spawnPosition)
    {
        var bullet = ObjectPoolManager.Instance.GetPoolableObject(fallingObjectPrefab, spawnPosition, fallingObjectPrefab.transform.rotation).GetComponent<Projectile>();
        bullet.SetTargetDistanceAndVelocity(150f, 10f);
    }
    private float SetRandomSpawnTime()
    {
        return Random.Range(0.1f, 0.5f);
    }
}
