using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    [System.Serializable]
    private struct objInfos
    {
        public GameObject targetObject;
        public int count;
    }

    [SerializeField] private objInfos[] infos;
    private Dictionary<GameObject, IObjectPool<GameObject>> objectPools = new Dictionary<GameObject, IObjectPool<GameObject>>();


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        InitializeObjectPool();

    }
    private void InitializeObjectPool()
    {
        for (int i = 0; i < infos.Length; i++)
        {
            if (objectPools.ContainsKey(infos[i].targetObject))
            {
                Debug.LogFormat("{0} Already added", infos[i].targetObject);
                continue;
            }

            IObjectPool<GameObject> pool = new ObjectPool<GameObject>(() => CreateObject(infos[i].targetObject), OnGetObject, OnReleasObject, OnDestroyObject,
                true, infos[i].count);

            objectPools.Add(infos[i].targetObject, pool);

            for (int j = 0; j < infos[i].count; j++)
            {
                GameObject poolObj = CreateObject(infos[i].targetObject);
                poolObj.GetComponent<IPoolableObject>().Release();
            }
        }
    }
    private GameObject CreateObject(GameObject poolingObj)
    {
        IPoolableObject poolableObject = poolingObj.GetComponent<IPoolableObject>();
        if (poolableObject == null)
        {
            Debug.LogError($"GameObject {poolingObj.name} must implement IPoolableObject.");
            return null;
        }

        var obj = Instantiate(poolingObj);
        obj.GetComponent<IPoolableObject>().SetPool(objectPools[poolingObj]);
        return obj;
    }


    private void OnGetObject(GameObject obj)
    {
        obj.gameObject.SetActive(true);
    }
    private void OnReleasObject(GameObject obj)
    {
        obj.gameObject.SetActive(false);
    }
    private void OnDestroyObject(GameObject obj)
    {
        Destroy(obj);
    }

    public GameObject GetPoolableObject(GameObject targetObject, Vector2 pos, Quaternion rotation, Transform parent = null)
    {
        if (objectPools.ContainsKey(targetObject) == false)
        {
            IObjectPool<GameObject> pool = new ObjectPool<GameObject>(() => CreateObject(targetObject), OnGetObject, OnReleasObject, OnDestroyObject, true, 10);
            objectPools.Add(targetObject, pool);

            for (int j = 0; j < 10; j++)
            {
                GameObject poolObj = CreateObject(targetObject);
                poolObj.GetComponent<IPoolableObject>().pool.Release(poolObj);
            }
        }

        GameObject returnObj = objectPools[targetObject].Get();
        returnObj.GetComponent<IPoolableObject>().Initialize(pos, rotation, parent);
        return returnObj;
    }
}
