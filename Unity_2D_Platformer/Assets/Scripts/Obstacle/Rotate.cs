using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour, IInteractable
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool isTurnOn;

    private void Start()
    {
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
