using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Patrol : IAction
{
    private Blackboard blackboard;
    private float stopDistance;
    private float patrolTime;
    private float timeElapsed;
    private Enemy owner;
    public Patrol(Blackboard blackBoard)
    {
        this.blackboard = blackBoard;
        stopDistance = blackboard.GetData<Enemy>("owner").patrolStopDistance;
        owner = blackboard.GetData<Enemy>("owner");
    }

    public void OnEnter()
    {
        blackboard.GetData<Enemy>("owner").healthBar.gameObject.SetActive(false);
        blackboard.GetData<Enemy>("owner").target = null;
        
        timeElapsed = 0f;
        patrolTime = Random.Range(1f, 6f);

        if (Random.value < 0.5f) Flip();

        blackboard.GetData<Enemy>("owner").spriteRenderer.color = Color.white;
        blackboard.GetData<Enemy>("owner").animHandler.animator.SetBool("IsPatrol", true);
    }

    public NodeState Execute()
    {
        timeElapsed += Time.deltaTime;

        if(timeElapsed >= patrolTime)
        {
            return NodeState.Success;
        }

        bool isWallHit = Physics2D.Raycast(owner.transform.position, owner.transform.right, owner.patrolStopDistance, owner.whatIsGround).collider != null;
        bool isEdgeMissing = Physics2D.Raycast(new Vector2(owner.transform.position.x + (owner.patrolStopDistance * owner.transform.right.x), owner.transform.position.y), Vector2.down, 3f, owner.whatIsGround).collider == null;

        if (isWallHit || isEdgeMissing)
        {
            Flip();
        }

        return NodeState.Running;
    }

    public void ExecuteInFixedUpdate()
    {
        Run();
    }
    public void OnExit()
    {
        blackboard.GetData<Enemy>("owner").animHandler.animator.SetBool("IsPatrol", false);
    }
    private void Flip()
    {
        float direction = blackboard.GetData<Enemy>("owner").transform.right.x;

        if (direction > 0)
        {
            blackboard.GetData<Enemy>("owner").transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            blackboard.GetData<Enemy>("owner").transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
    private void Run()
    {
        float targetSpeed = blackboard.GetData<Enemy>("owner").transform.right.x * blackboard.GetData<Enemy>("owner").movementType.patrolMaxSpeed;

        float accelAmount = blackboard.GetData<Enemy>("owner").movementType.patrolAccelAmount;

        float speedDif = targetSpeed - blackboard.GetData<Enemy>("owner").rb.velocity.x;
        float movement = speedDif * accelAmount;

        blackboard.GetData<Enemy>("owner").rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
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
