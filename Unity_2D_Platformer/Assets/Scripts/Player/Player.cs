using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb { get; private set; }
    public Animator animator { get; private set; }
    public PlayerInput input { get; private set; }


    private float gravityScale;
    private PlayerMovementStateMachine movementStateMachine;
    [SerializeField] private LayerMask whatIsGround;
    private void Awake()
    {
        movementStateMachine = new PlayerMovementStateMachine(this);

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        input = GetComponent<PlayerInput>();

        gravityScale = rb.gravityScale;
    }


    void Start()
    {
        movementStateMachine.ChangeState(movementStateMachine.idleState);
    }

    void Update()
    {
        rb.gravityScale = GetGravity();

        movementStateMachine.Update();
    }
    private void FixedUpdate()
    {
        movementStateMachine.FixedUpdate();
    }

    private void LateUpdate()
    {
        movementStateMachine.LateUpdate();
    }

    public bool IsOnGround()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, 0.5f + 0.02f, whatIsGround) == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void FlipPlayer(bool isLeft)
    {
        if (isLeft == true)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public bool IsPlayerFalling()
    {
        if(rb.velocity.y < 0.5f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public float GetGravity()
    {
        if (IsPlayerFalling() == true)
        {
            return 2f * gravityScale;
        }
        else if (IsOnGround() == false)
        {
            return gravityScale;
        }
        else
        {
            return 0f;
        }
    }
}
