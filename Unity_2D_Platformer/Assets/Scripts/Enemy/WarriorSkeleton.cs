using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSkeleton : Enemy
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {

    }
    private void FixedUpdate()
    {

    }

    public override void Die()
    {
        base.Die();
    }

    public override bool ApplyDamage(float dmg, GameObject damager)
    {
        if (base.ApplyDamage(dmg, damager) == false)
        {
            return false;
        }
        else
        {
            hp -= dmg;
            target = damager.GetComponent<LivingEntity>();

            if (hp <= 0f)
            {
                Die();
            }
            else
            {
            }

            return true;
        }
    }
}
