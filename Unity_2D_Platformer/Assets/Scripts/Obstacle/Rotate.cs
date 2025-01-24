using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour, ISwitchable
{
    [SerializeField] private float rotationSpeed;
    public bool isTurnOn { get; private set; }
    
    private void Start()
    {
        isTurnOn = true;
    }

    void Update()
    {
        if (isTurnOn)
        {
            transform.Rotate(0, 0, Time.deltaTime * rotationSpeed);
        }
    }

    public void Trigger()
    {
        isTurnOn = !isTurnOn;
    }
}
