using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement  Type")]
    [SerializeField] private MovementTypeSO movementType;

    public Rigidbody2D rb { get; private set; }
    [SerializeField] private Animator animator;
    public PlayerInput input { get; private set; }

    private bool isFacingRight;

    #region Jump Parameters
    private bool isJumping;
    private bool isJumpCut;
    private bool isJumpFalling;
    #endregion

    #region Timer
    private float lastOnGroundTime;
    private float lastPressJumpTime;
    #endregion

    #region Collision Check Parameteres
    [SerializeField] private LayerMask whatIsGround;

    [Header("Collision Check")]
    [SerializeField] private Transform groundChecker;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.49f,0.03f);
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();

    }
    // Start is called before the first frame update
    void Start()
    {
        isFacingRight = true;
        SetGravityScale(movementType.gravityScale);
    }

    // Update is called once per frame
    void Update()
    {
        lastOnGroundTime -= Time.deltaTime;
        lastPressJumpTime -= Time.deltaTime;

        if (input.movementInput.x != 0)
        {
            FlipPlayer(input.movementInput.x > 0);
        }
        
        if (input.isJump == true)
        {
            OnJumpInput();
        }

        if (input.isJumpCut == true)
        {
            OnJumpCutInput();
        }


        #region Collision Check
        if (!isJumping)
        {
            if (Physics2D.OverlapBox(groundChecker.position, groundCheckSize, 0, whatIsGround) == true) //checks if set box overlaps with ground
            {
                lastOnGroundTime = movementType.coyoteTime; //if so sets the lastGrounded to coyoteTime
            }
        }
        #endregion

        #region Jump Check

        if(isJumping == true && rb.velocity.y < 0f)
        {
            isJumping = false;
            isJumpFalling = true;
        }

        if(lastOnGroundTime > 0f)
        {
            isJumpCut = false;
            isJumpFalling = false;
        }

        if(CanJump() && lastPressJumpTime > 0f)
        {
            isJumping = true;
            isJumpCut = false;
            isJumpFalling = false;

            Jump();

            animator.SetTrigger("Jump");
        }

        #endregion

        #region Setting Gravity
        if (isJumpCut == true)
        {
            SetGravityScale(movementType.gravityScale * movementType.jumpCutGravityMult);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -movementType.maxFallSpeed));
        }
        else if((isJumping || isJumpFalling) && Mathf.Abs(rb.velocity.y) < movementType.jumpHangVelocityThreshold)
        {
            SetGravityScale(movementType.gravityScale * movementType.jumpHangGravityMult);
        }
        else if (rb.velocity.y < 0)
        {
            SetGravityScale(movementType.gravityScale * movementType.fallGravityMult);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -movementType.maxFallSpeed));
        }
        else
        {
            SetGravityScale(movementType.gravityScale);
        }
        #endregion
    }

    private void FixedUpdate()
    {
        Run(1f);
    }

    private void LateUpdate()
    {
        animator.SetBool("IsRun", input.movementInput.x != 0);
        animator.SetFloat("VelocityY", rb.velocity.y);

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

    private void Jump()
    {
        //Ensures we can't call Jump multiple times from one press
        lastPressJumpTime = 0;
        lastOnGroundTime = 0;

        #region Perform Jump
        //We increase the force applied if we are falling
        //This means we'll always feel like we jump the same amount 
        //(setting the player's Y velocity to 0 beforehand will likely work the same, but I find this more elegant :D)
        float force = movementType.jumpForce;
        if (rb.velocity.y < 0)
            force -= rb.velocity.y;

        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        #endregion
    }

    private void OnJumpInput()
    {
        lastPressJumpTime = movementType.jumpInputBufferTime;
    }

    private void OnJumpCutInput()
    {
        if(CanJumpCut() == true)
        {
            isJumpCut = true;
        }
    }

    private bool CanJumpCut()
    {
        if(isJumping == true && rb.velocity.y > 0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CanJump()
    {
        if(lastOnGroundTime > 0f && !isJumping)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void FlipPlayer(bool isMovingRight)
    {
        if(isMovingRight != this.isFacingRight)
        {
            if (isMovingRight == false)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                isFacingRight = false;
            }
            else
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                isFacingRight = true;
            }
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
        Gizmos.DrawWireCube(groundChecker.position, groundCheckSize);
        //Gizmos.color = Color.blue;
        //Gizmos.DrawWireCube(_frontWallCheckPoint.position, _wallCheckSize);
        //Gizmos.DrawWireCube(_backWallCheckPoint.position, _wallCheckSize);
    }
#endif
    #endregion
}
