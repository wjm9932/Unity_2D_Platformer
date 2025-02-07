using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject on;
    [SerializeField] private GameObject off;

    [SerializeField] private GameObject[] targets;

    [SerializeField] private bool resetable = true;
    [SerializeField] private bool resetFunction = true;
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
        if (on.activeSelf == true && resetable == true)
        {
            timeElapsed += Time.deltaTime;

            if (timeElapsed >= resetTimer)
            {
                if (resetFunction == true)
                {
                    TriggerSwitch();
                }
                else
                {
                    ToggleSwitchSprite();
                }
                timeElapsed = 0f;
            }
        }
    }

    private void TriggerSwitch()
    {
        foreach (GameObject target in targets)
        {
            target.GetComponent<IInteractable>().Trigger();
        }

        ToggleSwitchSprite();
    }

    public void Trigger()
    {
        if (Time.time >= lastActiveTime + timeBetActive && resetable == true && on.activeSelf == false)
        {
            TriggerSwitch();
            lastActiveTime = Time.time;
            SoundManager.Instance.PlaySoundEffect(SoundManager.InGameSoundEffectType.SWITCH_TRIGGER, 0.35f);
        }
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
