using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : IAction
{
    private Blackboard blackboard;
    private float timeNotSeeingPlayer;
    private Enemy owner;
    private bool isObstructed;
    public Track(Blackboard blackBoard)
    {
        this.blackboard = blackBoard;
        owner = blackboard.GetData<Enemy>("owner");
    }

    public void OnEnter()
    {
        isObstructed = false;
        timeNotSeeingPlayer = 0f;
        blackboard.GetData<Enemy>("owner").animHandler.animator.SetBool("IsTrack", true);
    }

    public NodeState Execute()
    {
        Flip(blackboard.GetData<Enemy>("owner").transform.position.x < blackboard.GetData<Enemy>("owner").target.transform.position.x);

        timeNotSeeingPlayer = (!IsTargetOnSight() || isObstructed) ? timeNotSeeingPlayer + Time.deltaTime : 0f;

        if (timeNotSeeingPlayer >= 3f || blackboard.GetData<Enemy>("owner").target.isDead == true)
        {
            blackboard.GetData<Enemy>("owner").target = null;
            return NodeState.Failure;
        }
        if(Vector2.Distance(owner.transform.position, owner.target.transform.position) <= blackboard.GetData<Enemy>("owner").trackStopDistance)
        {
            return NodeState.Success;
        }

        return NodeState.Running;
    }

    public void ExecuteInFixedUpdate()
    {
        bool isWallHit = Physics2D.Raycast(owner.transform.position, owner.transform.right, owner.patrolStopDistance , owner.whatIsGround).collider != null;
        bool isEdgeMissing = Physics2D.Raycast(new Vector2(owner.transform.position.x + (owner.patrolStopDistance * owner.transform.right.x), owner.transform.position.y), Vector2.down, 3f, owner.whatIsGround).collider == null;

        isObstructed = isWallHit || isEdgeMissing; 

        if (isObstructed == true)
        {
            DeaccelVelocity();
        }
        else
        {
            Run();
        }
    }
    public void OnExit()
    {
        blackboard.GetData<Enemy>("owner").animHandler.animator.SetBool("IsTrack", false);
    }

    private bool IsTargetOnSight()
    {
        var owner = blackboard.GetData<Enemy>("owner");

        RaycastHit2D hit = Physics2D.Raycast(owner.transform.position, owner.transform.right, 10f, owner.targetLayer);

        return hit.collider != null;
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
    private void Run()
    {
        float targetSpeed = blackboard.GetData<Enemy>("owner").transform.right.x * blackboard.GetData<Enemy>("owner").movementType.trackMaxSpeed;


        float speedDiff = targetSpeed - blackboard.GetData<Enemy>("owner").rb.velocity.x;
        float accelAmount = (targetSpeed > Mathf.Abs(blackboard.GetData<Enemy>("owner").rb.velocity.x)) ? blackboard.GetData<Enemy>("owner").movementType.trackAccelAmount : blackboard.GetData<Enemy>("owner").movementType.trackDeccelAmount;
        float movement = speedDiff * accelAmount;

        blackboard.GetData<Enemy>("owner").rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }
    private void DeaccelVelocity()
    {
        float speedDiff = 0f - blackboard.GetData<Enemy>("owner").rb.velocity.x;
        float movement = speedDiff * owner.movementType.trackDeccelAmount;

        blackboard.GetData<Enemy>("owner").rb.AddForce(movement * Vector2.right);
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
}