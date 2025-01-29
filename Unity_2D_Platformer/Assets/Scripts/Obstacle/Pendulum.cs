using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour, IInteractable
{
    [SerializeField] private float swingAngle;
    [SerializeField] private float speed;
    [SerializeField] private bool isTurnOn;
    private float timeElapsed;

    private void Start()
    {
        timeElapsed = 0f;
    }

    void Update()
    {
        if(isTurnOn)
        {
            timeElapsed += Time.deltaTime;
            float angle = Mathf.Cos(timeElapsed * speed) * swingAngle;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
    public void Trigger()
    {
        isTurnOn = !isTurnOn;
    }
}
