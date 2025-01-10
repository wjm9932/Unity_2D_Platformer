using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    public enum ObjectType
    { 
        ARROW,
    }

    [System.Serializable]
    private struct ObjectInfo
    {
        public ObjectType objectType;
        public GameObject prefab;
        public int count;
    }

    [SerializeField] private GameObject[] testObj;


    [SerializeField] private ObjectInfo[] objectInfos;

    private Dictionary<ObjectType, GameObject> prefabs = new Dictionary<ObjectType, GameObject>();
    private Dictionary<ObjectType, IObjectPool<GameObject>> objectPools = new Dictionary<ObjectType, IObjectPool<GameObject>>();

    private Dictionary<GameObject, GameObject> test = new Dictionary<GameObject, GameObject>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        InitializeObjectPool();

        for(int i = 0; i < testObj.Length; i++)
        {
            test.Add(testObj[i], testObj[i]);
        }
    }

    public GameObject GetTestObj(GameObject test)
    {
        return this.test[test];
    }

    private void InitializeObjectPool()
    {
        for(int i = 0; i < objectInfos.Length; i++)
        {
            ObjectType currentType = objectInfos[i].objectType;
            IObjectPool<GameObject> pool = new ObjectPool<GameObject>(() => CreateObject(currentType), OnGetObject, OnReleasObject, OnDestroyObject, 
                true, objectInfos[i].count);

            if (prefabs.ContainsKey(objectInfos[i].objectType))
            {
                Debug.LogFormat("{0} Already added", objectInfos[i].objectType);
                return;
            }

            prefabs.Add(objectInfos[i].objectType, objectInfos[i].prefab);
            objectPools.Add(objectInfos[i].objectType, pool);

            for (int j = 0; j < objectInfos[i].count; j++)
            {
                GameObject poolObj = CreateObject(objectInfos[i].objectType);
                poolObj.GetComponent<IPoolableObject>().pool.Release(poolObj);
            }

        }
    }
    private GameObject CreateObject(ObjectType objectType)
    {
        GameObject obj = Instantiate(prefabs[objectType]);
        obj.GetComponent<IPoolableObject>().SetPool(objectPools[objectType]);

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

    public GameObject GetPoolableObject(ObjectType objectType)
    {
        if(objectPools.ContainsKey(objectType) == false)
        {
            Debug.LogError("There is no target object type  in object pool");
            return null;
        }

        return objectPools[objectType].Get();
    }
}
