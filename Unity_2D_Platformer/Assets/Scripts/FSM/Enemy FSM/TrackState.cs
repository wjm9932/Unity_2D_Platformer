using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackState : IState
{
    private EnemyStateMachine sm;
    public TrackState(EnemyStateMachine enemyStateMachine)
    {
        sm = enemyStateMachine;
    }
    public void Enter()
    {
        if (sm.owner.target == null)
        {
            Debug.LogError("There is no target");
            return;
        }

        sm.owner.animHandler.animator.SetBool("IsTrack", true);
    }
    public void Update()
    {
        if (!IsTargetOnWayPoints() || sm.owner.target.isDead == true)
        {
            sm.ChangeState(sm.patrolState);
            return;
        }

        Flip(sm.owner.transform.position.x < sm.owner.target.transform.position.x);
    }
    public void FixedUpdate()
    {
        if(Mathf.Abs(sm.owner.target.transform.position.x - sm.owner.transform.position.x) >= 0.5f)
        {
            Run(1f);
        }
        else
        {
            Run(0f);
        }
    }
    public void LateUpdate()
    {

    }
    public void Exit()
    {
        sm.owner.animHandler.animator.SetBool("IsTrack", false);
    }

    public virtual void OnAnimationEnterEvent()
    {

    }
    public virtual void OnAnimationExitEvent()
    {
    }
    public virtual void OnAnimationTransitionEvent()
    {
    }

    private bool IsTargetOnWayPoints()
    {
        float minX = Mathf.Min(sm.owner.patrolPoint_1, sm.owner.patrolPoint_2);
        float maxX = Mathf.Max(sm.owner.patrolPoint_1, sm.owner.patrolPoint_2);

        return minX <= sm.owner.target.transform.position.x && sm.owner.target.transform.position.x <= maxX;
    }

    private void Flip(bool isMovingRight)
    {
        if (isMovingRight && sm.owner.transform.localRotation.eulerAngles.y != 0)
        {
            sm.owner.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (!isMovingRight && sm.owner.transform.localRotation.eulerAngles.y != 180)
        {
            sm.owner.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void Run(float lerpAmount)
    {
        float targetSpeed = sm.owner.transform.right.x * sm.owner.movementType.trackMaxSpeed * lerpAmount;

        float accelAmount = sm.owner.movementType.trackAccelAmount;

        float speedDif = targetSpeed - sm.owner.rb.velocity.x;
        float movement = speedDif * accelAmount;

        sm.owner.rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }
}