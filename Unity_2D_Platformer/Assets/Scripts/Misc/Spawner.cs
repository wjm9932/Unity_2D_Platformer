using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Spawner : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject targetObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Trigger()
    {
        if(ObjectPoolManager.Instance.GetPoolableObject(targetObject, transform.position, targetObject.transform.rotation) == null)
        {
            Instantiate(targetObject, transform.position, targetObject.transform.rotation);
        }
    }
}
