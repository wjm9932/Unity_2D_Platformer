using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LivingEntity : MonoBehaviour
{
    [Header("LivingEntity Components")]
    [Header("Grace Period")]
    [SerializeField] protected float timeBetDamaged;

    [Header("Health Point")]
    [SerializeField] private float hp;

    [Header("Damage")]
    [SerializeField] public float dmg;

    [Header("Sprite Renderer")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Animation Handler")]
    [SerializeField] private AnimationHandler _animHandler;
    public AnimationHandler animHandler { get { return _animHandler; } }

    public int hitDir {get; private set;}

    protected float lastTimeDamaged;
    protected bool canBeDamage
    {
        get { return Time.time >= lastTimeDamaged + timeBetDamaged; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual bool ApplyDamage(float dmg, GameObject damager)
    {
        if(canBeDamage == false)
        {
            return false;
        }
        else
        {
            Vector2 damagerPos = new Vector2(damager.transform.position.x, damager.transform.position.y).normalized;
            Vector2 pos = new Vector2(transform.position.x, transform.position.y).normalized;

            hitDir = Vector2.Dot(damagerPos - pos, transform.right) < 0f ? -1 : 1;

            lastTimeDamaged = Time.time;
            hp -= dmg;

            StartCoroutine(StartGracePeriod());

            return true;
        }
    }

    private IEnumerator StartGracePeriod()
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

    public abstract void OnAnimationEnterEvent();
    public abstract void OnAnimationTransitionEvent();
    public abstract void OnAnimationExitEvent();
}
