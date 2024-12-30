using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement  Type")]
    [SerializeField] private MovementTypeSO movementType;

    public Rigidbody2D rb { get; private set; }
    public Animator animator { get; private set; }
    public PlayerInput input { get; private set; }

    #region Jump Parameters
    private bool isJumping;
    private bool isJumpCut;
    private bool isJumpFalling;
    private float jumpBufferTime;
    #endregion

    #region Timer
    private float lastOnGroundTime;
    #endregion

    #region Collision Check Parameteres
    [SerializeField] private LayerMask whatIsGround;

    [Header("Collision Check")]
    [SerializeField] private Transform groundCheckPosition;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.49f,0.03f);
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        input = GetComponent<PlayerInput>();
    }
    // Start is called before the first frame update
    void Start()
    {
        SetGravityScale(movementType.gravityScale);
    }

    // Update is called once per frame
    void Update()
    {
        lastOnGroundTime -= Time.deltaTime;


        if (input.movementInput.x != 0)
        {
            FlipPlayer(input.movementInput.x < 0);
        }

        if(input.isJump == true)
        {
            OnJumpInput();
        }

        if(input.isJumpCut == true)
        {

        }

        #region Collision Check
        if(!isJumping)
        {
            if (Physics2D.OverlapBox(groundCheckPosition.position, groundCheckSize, 0, whatIsGround) == true) //checks if set box overlaps with ground
            {
                lastOnGroundTime = movementType.coyoteTime; //if so sets the lastGrounded to coyoteTime
            }
        }
        #endregion
        if (rb.velocity.y < 0)
        {
            //Higher gravity if falling
            SetGravityScale(movementType.gravityScale * movementType.fallGravityMult);
            //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -movementType.maxFallSpeed));
        }
        else
        {
            SetGravityScale(movementType.gravityScale);
        }
    }

    private void FixedUpdate()
    {
        Run(1f);
    }


    private void Run(float lerpAmount)
    {
        float targetSpeed = input.movementInput.x * movementType.runMaxSpeed;
        targetSpeed = Mathf.Lerp(rb.velocity.x, targetSpeed, lerpAmount);

        float accelAmount;

        if(lastOnGroundTime > 0f)
        {
            accelAmount = (Mathf.Abs(targetSpeed) > 0.01f) ? movementType.runAccelAmount : movementType.runDeccelAmount;
        }
        else
        {
            accelAmount = (Mathf.Abs(targetSpeed) > 0.01f) ? movementType.runAccelAmount * movementType.accelInAir : movementType.runDeccelAmount * movementType.deccelInAir;
        }

        ////Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
        //if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
        //{
        //    accelRate *= Data.jumpHangAccelerationMult;
        //    targetSpeed *= Data.jumpHangMaxSpeedMult;
        //}

        float speedDif = targetSpeed - rb.velocity.x;
        float movement = speedDif * accelAmount;

        rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    private void OnJumpInput()
    {
        jumpBufferTime = movementType.jumpInputBufferTime;
    }

    private void FlipPlayer(bool isLeft)
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
    private void SetGravityScale(float scale)
    {
        rb.gravityScale = scale;
    }

    #region EDITOR METHODS
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundCheckPosition.position, groundCheckSize);
        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireCube(_frontWallCheckPoint.position, _wallCheckSize);
        //Gizmos.DrawWireCube(_backWallCheckPoint.position, _wallCheckSize);
    }
#endif
    #endregion
}
