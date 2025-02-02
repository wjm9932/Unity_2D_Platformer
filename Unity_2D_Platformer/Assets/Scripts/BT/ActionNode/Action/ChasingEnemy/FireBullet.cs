using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : IAction
{
    private Blackboard blackboard;

    private readonly float fireInterval;
    private float timeElapsed;
    private int bulletCount;
    private int currentFireBulletCount;
    public FireBullet(Blackboard blackBoard)
    {
        this.blackboard = blackBoard;
        fireInterval = 0.8f;
    }

    public void OnEnter()
    {
        bulletCount = Random.Range(3, 6);
        currentFireBulletCount = 0;
        timeElapsed = 0f;
    }

    public NodeState Execute()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= fireInterval)
        {
            Fire();
        }

        if (currentFireBulletCount >= bulletCount)
        {
            return NodeState.Success;
        }
        return NodeState.Running;
    }

    public void ExecuteInFixedUpdate()
    {
    }
    public void OnExit()
    {
    }

    public void OnAnimationEnterEvent()
    {
    }
    public void OnAnimationTransitionEvent()
    {

    }
    public void OnAnimationExitEvent()
    {
    }
    private void Fire()
    {
        timeElapsed = 0f;
        ++currentFireBulletCount;

        var transform = blackboard.GetData<ChasingEnemy>("owner").transform;
        var bullet = ObjectPoolManager.Instance.GetPoolableObject(blackboard.GetData<ChasingEnemy>("owner").bulletPrefab, transform.position, Quaternion.identity).GetComponent<Projectile>();
        bullet.SetTargetDistanceAndVelocity(blackboard.GetData<ChasingEnemy>("owner").transform.position, 100f, blackboard.GetData<ChasingEnemy>("owner").chasing.currentVelocity + 15f);
    }
}
