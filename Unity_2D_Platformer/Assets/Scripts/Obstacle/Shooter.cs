using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private GameObject rangedPrefabs;
    // Start is called before the first frame update
    void Start()
    {
        var test = ObjectPoolManager.Instance.GetTestObj(rangedPrefabs);
        Debug.Log(test);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
