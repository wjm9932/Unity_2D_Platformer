using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpell : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Animator animator;
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(IsAnimationFinished() == true)
        {
            Destroy(this.gameObject.transform.root.gameObject);
        }
    }

    private void DetermineSpellAttack(int isAttack)
    {
        if (isAttack == 1)
        {
            boxCollider.enabled = true;
        }
        else
        {
            boxCollider.enabled = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            collision.GetComponent<Player>().TakeDamage(this.gameObject, true);
        }
    }

    private bool IsAnimationFinished()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.normalizedTime >= 1f;
    }
}
