using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider easeBar;
    [SerializeField] private Slider hpBar;

    private float lerpSpeed = 3f;
    public void UpdateHealthBar(float health, float maxHealth)
    {
        hpBar.value = health / maxHealth;
    }

    void Update()
    {
        easeBar.value = Mathf.Lerp(easeBar.value, hpBar.value, lerpSpeed * Time.deltaTime);
        this.transform.rotation = Camera.main.transform.rotation;
    }
}
