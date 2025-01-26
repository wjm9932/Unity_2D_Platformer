using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translate : MonoBehaviour, IInteractable
{
    public bool isTurnOn { get; private set; }

    [SerializeField] private float moveDistance = 5f;
    [SerializeField] private float moveSpeed = 2f;

    private Vector3 startPos;
    private float timeElapsed;

    void Start()
    {
        isTurnOn = true;
        startPos = transform.position;
        timeElapsed = 0f;
    }

    void Update()
    {
        if(isTurnOn)
        {
            timeElapsed += Time.deltaTime;
            float offset = Mathf.Sin(timeElapsed * moveSpeed) * moveDistance;
            transform.position = startPos + new Vector3(offset, 0, 0);
        }
    }

    public void Trigger()
    {
        isTurnOn = !isTurnOn;
    }
}
