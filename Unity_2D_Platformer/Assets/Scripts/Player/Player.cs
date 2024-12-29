using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb { get; private set; }
    public Animator animator { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }
    public PlayerInput input { get; private set; }


    private float gravityScale;
    private PlayerMovementStateMachine movementStateMachine;
    [SerializeField] private LayerMask whatIsGround; 

    private void Awake()
    {
        movementStateMachine = new PlayerMovementStateMachine(this);

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        input = GetComponent<PlayerInput>();

        gravityScale = rb.gravityScale;
    }


    void Start()
    {
        movementStateMachine.ChangeState(movementStateMachine.idleState);
    }

    void Update()
    {
        rb.gravityScale = IsOnGround() ? 0 : gravityScale;

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
            //rb.velocity = new Vector2(rb.velocity.x, 0);
            return true;
        }
        else
        {
            return false;
        }
    }
}
