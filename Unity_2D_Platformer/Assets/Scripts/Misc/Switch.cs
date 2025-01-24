using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] private GameObject on;
    [SerializeField] private GameObject off;

    [SerializeField] private GameObject[] targets;

    [SerializeField] private float resetTimer;

    private float timeBetActive = 0.5f;
    private float lastActiveTime;

    private float timeElapsed;

    private void Awake()
    {
        lastActiveTime -= timeBetActive;
    }

    private void Update()
    {
        if(on.activeSelf == true)
        {
            timeElapsed += Time.deltaTime;

            if(timeElapsed >= resetTimer)
            {
                Trigger();
                timeElapsed = 0f;
            }
        }
    }

    public void TriggerSwitch()
    {
        if(Time.time >= lastActiveTime + timeBetActive)
        {
            Trigger();
            lastActiveTime = Time.time;
        }
    }

    private void Trigger()
    {
        foreach (GameObject target in targets)
        {
            target.GetComponent<ISwitchable>().Trigger();
        }
        ToggleSwitchSprite();
    }

    private void ToggleSwitchSprite()
    {
        if (on.activeSelf == true)
        {
            on.SetActive(false);
            off.SetActive(true);

            timeElapsed = 0f;
        }
        else
        {
            on.SetActive(true);
            off.SetActive(false);
        }
    }

}
