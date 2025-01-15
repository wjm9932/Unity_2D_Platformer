using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Heart : MonoBehaviour, IPoolableObject
{
    public IObjectPool<GameObject> pool { get; private set; }

    private Color originColor;
    private const float existingTime = 10f;
    private float enableTime;
    private SpriteRenderer spriteRenderer;
    private void OnEnable()
    {
        enableTime = Time.time;
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originColor = spriteRenderer.color;
    }

    private void Update()
    {
        if(pool != null)
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

    public void Initialize(Vector2 position, Quaternion rotation, Transform parent = null)
    {
        spriteRenderer.color = originColor;
        transform.position = position;
        transform.rotation = rotation;
    }
    public void Release()
    {
        pool.Release(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            if (collision.GetComponent<Player>().RecoverHealth() == true)
            {
                if (pool != null)
                {
                    Release();
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
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
