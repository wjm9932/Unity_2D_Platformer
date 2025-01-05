using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivinEntity : MonoBehaviour
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

    public virtual bool ApplyDamage(float dmg)
    {
        if(canBeDamage == false)
        {
            return false;
        }
        else
        {
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
            // 스프라이트 깜빡이기
            isVisible = !isVisible;
            Color color = spriteRenderer.color;
            color.a = isVisible ? 1f : 0.5f; // 1f: 보임, 0.5f: 반투명
            spriteRenderer.color = color;

            // 깜빡임 간격 설정
            yield return new WaitForSeconds(0.1f);

            elapsedTime += 0.1f;
        }

        // 무적 시간 종료 후 알파 값을 원래대로 복원
        Color finalColor = spriteRenderer.color;
        finalColor.a = 1f;
        spriteRenderer.color = finalColor;
    }
}
