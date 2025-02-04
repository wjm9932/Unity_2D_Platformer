using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolableSoundObject : MonoBehaviour, IPoolableObject
{
    public IObjectPool<GameObject> pool { get; private set; }
    public AudioSource audioSource { get; private set; }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(audioSource.isPlaying == false)
        {
            Release();
        }
    }

    public void SetPool(IObjectPool<GameObject> pool)
    {
        this.pool = pool;
    }

    public void Release()
    {
        pool.Release(gameObject);
    }
}
