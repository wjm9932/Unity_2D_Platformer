using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Item : MonoBehaviour, IPoolableObject
{
    public IObjectPool<GameObject> pool { get; private set; }

    protected SpriteRenderer spriteRenderer;
    protected Color originColor;

    protected const float existingTime = 10f;
    protected float enableTime;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originColor = spriteRenderer.color;
    }
    private void OnEnable()
    {
        enableTime = Time.time;
        spriteRenderer.color = originColor;
    }
    protected virtual void Update()
    {
        if (pool != null)
        {
            if (Time.time >= enableTime + existingTime)
            {
                StartCoroutine(FadeOut(1f));
            }
        }
    }

    public void SetPool(IObjectPool<GameObject> pool)
    {
        this.pool = pool;
    }

    public void Release()
    {
        pool.Release(gameObject);
    }

    private IEnumerator FadeOut(float duration)
    {
        Color originalColor = spriteRenderer.color;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(originalColor.a, 0f, elapsed / duration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // 알파 값을 0으로 확실히 설정
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        Release();
    }
}
