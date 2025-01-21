using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : IAction
{
    private Blackboard blackboard;
    private int shootingCount;
    private bool isTeleportingFinished;
    public Shoot(Blackboard blackBoard)
    {
        this.blackboard = blackBoard;
    }

    public void OnEnter()
    {
        isTeleportingFinished = false;
        shootingCount = Random.Range(3, 6);

        blackboard.GetData<Boss>("owner").StartCoroutine(ShootBullets());
    }

    public NodeState Execute()
    {
        Flip(blackboard.GetData<Enemy>("owner").transform.position.x < blackboard.GetData<Enemy>("owner").target.transform.position.x);

        if (isTeleportingFinished == true)
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
        blackboard.GetData<Boss>("owner").isVulnerable = false;
        blackboard.GetData<Boss>("owner").rb.isKinematic = false;
    }
    public void OnAnimationEnterEvent()
    {
    }
    public void OnAnimationTransitionEvent()
    {
        Teleportation();
    }
    public void OnAnimationExitEvent()
    {
        isTeleportingFinished = true;
    }

    private IEnumerator ShootBullets()
    {
        yield return new WaitForSeconds(0.8f);

        for (int i = 0; i < shootingCount; i++)
        {

            var ownerPosition = blackboard.GetData<Boss>("owner").transform.position;
            var targetPosition = blackboard.GetData<Boss>("owner").target.transform.position;
            var direction = (targetPosition - ownerPosition).normalized;

            var bullet = ObjectPoolManager.Instance.GetPoolableObject(blackboard.GetData<Boss>("owner").bulletPrefab, ownerPosition, Quaternion.identity).GetComponent<Projectile>();
            bullet.transform.right = direction;
            bullet.SetTargetDistanceAndVelocity(100f, 30f);

            yield return new WaitForSeconds(0.5f);
        }

        blackboard.GetData<Boss>("owner").animHandler.animator.SetTrigger("ShootingFinished");
    }

    private void Teleportation()
    {
        Vector2 randomPos = blackboard.GetData<Boss>("owner").transform.position;

        randomPos.x = Random.Range(blackboard.GetData<Boss>("owner").patrolPoint_1, blackboard.GetData<Boss>("owner").patrolPoint_2);
        randomPos.y = blackboard.GetData<Boss>("owner").yPos;

        blackboard.GetData<Boss>("owner").transform.position = randomPos;
    }
    private void Flip(bool isMovingRight)
    {
        if (isMovingRight && blackboard.GetData<Enemy>("owner").transform.localRotation.eulerAngles.y != 0)
        {
            blackboard.GetData<Enemy>("owner").transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (!isMovingRight && blackboard.GetData<Enemy>("owner").transform.localRotation.eulerAngles.y != 180)
        {
            blackboard.GetData<Enemy>("owner").transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
