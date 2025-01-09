using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LivingEntity : MonoBehaviour
{
    [Header("LivingEntity Components")]
    [Header("Grace Period")]
    [SerializeField] protected float timeBetDamaged;



    [Header("Damage")]
    [SerializeField] public float dmg;

    [Header("Sprite Renderer")]
    [SerializeField] public SpriteRenderer spriteRenderer;

    [Header("Animation Handler")]
    [SerializeField] private AnimationHandler _animHandler;
    public AnimationHandler animHandler { get { return _animHandler; } }
    public event Action onDeath;

    public int hitDir { get; private set; }
    public bool isDead { get; protected set; }

    protected float lastTimeDamaged;
    protected bool canBeDamaged
    {
        get { return Time.time >= lastTimeDamaged + timeBetDamaged; }
    }
    private void Awake()
    {
        isDead = false;
    }

    void Start()
    {

    }

    public virtual bool ApplyDamage(float dmg, LivingEntity damager)
    {
        if (canBeDamaged == false)
        {
            return false;
        }
        else
        {

            hitDir = damager.transform.position.x < transform.position.x ? 1 : -1;
            lastTimeDamaged = Time.time;

            StartCoroutine(StartGracePeriod());

            return true;
        }
    }

    protected IEnumerator StartGracePeriod()
    {
        float elapsedTime = 0f;
        bool isVisible = true;

        while (elapsedTime < timeBetDamaged)
        {
            isVisible = !isVisible;
            Color color = spriteRenderer.color;
            color.a = isVisible ? 1f : 0.5f;
            spriteRenderer.color = color;

            yield return new WaitForSeconds(0.1f);

            elapsedTime += 0.1f;
        }

        Color finalColor = spriteRenderer.color;
        finalColor.a = 1f;
        spriteRenderer.color = finalColor;
    }
    public virtual void Die()
    {
        onDeath?.Invoke();
        isDead = true;
    }

    public virtual void KillInstant()
    {
        Die();
    }

    public abstract void OnAnimationEnterEvent();
    public abstract void OnAnimationTransitionEvent();
    public abstract void OnAnimationExitEvent();
}
