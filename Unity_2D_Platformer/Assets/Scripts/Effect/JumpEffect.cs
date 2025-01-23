using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class JumpEffect : MonoBehaviour, IPoolableObject
{
    public IObjectPool<GameObject> pool { get; private set; }

    private Animator animator;

    private void OnEnable()
    {
        
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(IsAnimationFinished() == true)
        {
            Release();
        }
    }

    public void SetPool(IObjectPool<GameObject> pool)
    {
        this.pool = pool;
    }

    public void Initialize(Vector2 position, Quaternion rotation, Transform parent = null)
    {
        transform.position = position;
        transform.rotation = rotation;
    }
    public void Release()
    {
        pool.Release(gameObject);
    }
    public bool IsAnimationFinished()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        return stateInfo.normalizedTime >= 1f && !animator.IsInTransition(0);
    }
}
