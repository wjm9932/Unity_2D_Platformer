using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : LivinEntity
{
    [Space(20)]
    [Header("Enemy Components")]

    [Header("Movement  Type")]
    public EnemyMovementTypeSO movementType;
    public float patrolPoint_1 { get; private set; }
    public float patrolPoint_2 { get; private set; }

    public Rigidbody2D rb { get; private set; }
    public float stopDistance { get; private set; }

    private EnemyStateMachine enemyStateMachine;
    private void Awake()
    {
        stopDistance = 0.1f;
        rb = GetComponent<Rigidbody2D>();
        enemyStateMachine = new EnemyStateMachine(this);
    }

    void Start()
    {
        enemyStateMachine.ChangeState(enemyStateMachine.patrolState);
    }

    // Update is called once per frame
    void Update()
    {
        enemyStateMachine.Update();
    }

    private void FixedUpdate()
    {
        enemyStateMachine.FixedUpdate();
    }

    public override bool ApplyDamage(float dmg, GameObject damager)
    {
        if (base.ApplyDamage(dmg, damager) == false)
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {

            player.ApplyDamage(dmg, this.gameObject);
        }
    }

    public void SetPatrolPoints(float x1, float x2)
    {
        patrolPoint_1 = x1;
        patrolPoint_2 = x2;
    }
}
