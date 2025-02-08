using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject[] items;
    [SerializeField] private GameObject enemySpawner;

    private Animator animator;
    private bool isTriggered = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Trigger()
    {
        if (isTriggered == true)
        {
            return;
        }

        isTriggered = true;
        animator.SetTrigger("Open");
        StartCoroutine(WaitForAnimationAndSpawn());
        SoundManager.Instance.PlaySoundEffect(SoundManager.InGameSoundEffectType.CHEST_OPEN, 0.4f);
    }

    private IEnumerator WaitForAnimationAndSpawn()
    {
        yield return null;

        yield return new WaitUntil(() => IsAnimationFinished());

        if (enemySpawner != null)
        {
            enemySpawner.GetComponent<EnemySpawner>().SpawnEnemy();
        }

        if(items.Length > 0)
        {
            int randItem = Random.Range(0, items.Length);
            if (ObjectPoolManager.Instance.GetPoolableObject(items[randItem], transform.position, items[randItem].transform.rotation) == null)
            {
                Instantiate(items[randItem], transform.position, items[randItem].transform.rotation);
            }
        }
    }

    public bool IsAnimationFinished()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        return stateInfo.normalizedTime >= 1f && !animator.IsInTransition(0);
    }
}
