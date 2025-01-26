using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour, IInteractable
{
    [SerializeField] private float swingAngle;
    [SerializeField] private float speed;
    public bool isTurnOn { get; private set; }
    private float timeElapsed;

    private void Start()
    {
        timeElapsed = 0f;
        isTurnOn = true;
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
