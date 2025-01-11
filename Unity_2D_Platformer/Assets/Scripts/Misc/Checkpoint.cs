using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private GameObject flag;
    [SerializeField] private Transform conqueredFlagPosition;
    [SerializeField] private Transform checkPointPosition;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            collision.GetComponent<Player>().SetCheckPoint(checkPointPosition.position);
            flag.transform.position = conqueredFlagPosition.position;
        }
    }
}
