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
        blackboard.GetData<Boss>("owner").isGraceTime = false;
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

            var bullet = ObjectPoolManager.Instance.GetPoolableObject(blackboard.GetData<Boss>("owner").bulletPrefab, ownerPosition, Quaternion.identity);
            bullet.transform.right = direction;

            if(blackboard.GetData<Enemy>("owner").transform.position.x < blackboard.GetData<Enemy>("owner").target.transform.position.x)
            {
                bullet.GetComponent<SpriteRenderer>().flipY = false;
            }
            else
            {
                bullet.GetComponent<SpriteRenderer>().flipY = true;
            }

            bullet.GetComponent<Projectile>().SetTargetDistanceAndVelocity(ownerPosition, 100f, 25f, true);
            SoundManager.Instance.PlaySoundEffect(SoundManager.InGameSoundEffectType.BOSS_SHOOT, 0.15f);

            yield return new WaitForSeconds(0.5f);
        }

        blackboard.GetData<Boss>("owner").animHandler.animator.SetTrigger("ShootingFinished");
    }

    private void Teleportation()
    {
        Vector2 randomPos = blackboard.GetData<Boss>("owner").transform.position;

        randomPos.x = Random.Range(blackboard.GetData<Boss>("owner").bossRange[0].transform.position.x, blackboard.GetData<Boss>("owner").bossRange[1].transform.position.x);
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
