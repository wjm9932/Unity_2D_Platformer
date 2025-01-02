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
    private bool isWallJumping;
    #endregion

    #region Slide
    private bool isSlide;
    private bool slideDir;
    #endregion

    #region Timer
    private float lastOnGroundTime;
    private float lastPressJumpTime;
    private float lastOnWallTime;
    private float wallJumpStartTime;
    #endregion

    #region Collision Check Parameteres
    [SerializeField] private LayerMask whatIsGround;

    [Header("Collision Check")]
    [SerializeField] private Transform groundChecker;
    [SerializeField] private Vector2 groundCheckSize = new Vector2();
    [SerializeField] private Transform wallCollisionChecker;
    [SerializeField] private Vector2 wallCollisionCheckerSize = new Vector2();
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
        lastOnWallTime -= Time.deltaTime;
        lastPressJumpTime -= Time.deltaTime;

        if (input.moveInput.x != 0)
        {
            FlipPlayer(input.moveInput.x > 0);
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

            if (Physics2D.OverlapBox(wallCollisionChecker.position, wallCollisionCheckerSize, 0, whatIsGround))
            {
                lastOnWallTime = movementType.coyoteTime;
                slideDir = isFacingRight;
            }
        }
        #endregion

        #region Jump Check
        if (isJumping == true && rb.velocity.y < 0f)
        {
            isJumping = false;
            isJumpFalling = true;
        }
     
        if (lastOnGroundTime > 0f)
        {
            isJumpCut = false;
            isJumpFalling = false;
        }

        if (CanJump() == true)
        {
            isJumping = true;
            isJumpCut = false;
            isJumpFalling = false;

            Jump();

            animator.SetTrigger("Jump");
        }
        else if(CanWallJump() == true)
        {
            isJumping = true;
            isJumpCut = false;
            isJumpFalling = false;

            WallJump(slideDir ? -1 : 1);

            animator.SetTrigger("Jump");

        }
        #endregion

        #region Slide Check
        if (CanSlide() == true)
        {
            isSlide = true;
        }
        else
        {
            isSlide = false;
        }
        #endregion

        #region Setting Gravity
        if (isSlide == true)
        {
            SetGravityScale(0f);
        }
        else if (rb.velocity.y < 0 && input.moveInput.y < 0)
        {
            SetGravityScale(movementType.gravityScale * movementType.fastFallGravityMult);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -movementType.maxFastFallSpeed));
        }
        else if (isJumpCut == true)
        {
            SetGravityScale(movementType.gravityScale * movementType.jumpCutGravityMult);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -movementType.maxFallSpeed));
        }
        else if ((isJumping || isJumpFalling) && Mathf.Abs(rb.velocity.y) < movementType.jumpHangVelocityThreshold)
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
        if (isSlide == true)
        {
            Slide();
        }
        else
        {
            Run(1f);
        }
    }

    private void LateUpdate()
    {
        animator.SetBool("IsSlide", isSlide);
        animator.SetBool("IsRun", input.moveInput.x != 0);
        animator.SetFloat("VelocityY", rb.velocity.y);
    }
    private void Run(float lerpAmount)
    {
        float targetSpeed = input.moveInput.x * movementType.runMaxSpeed;
        targetSpeed = Mathf.Lerp(rb.velocity.x, targetSpeed, lerpAmount);

        float accelAmount;

        if (lastOnGroundTime > 0f)
        {
            accelAmount = (Mathf.Abs(targetSpeed) > 0.01f) ? movementType.runAccelAmount : movementType.runDeccelAmount;
        }
        else
        {
            accelAmount = (Mathf.Abs(targetSpeed) > 0.01f) ? movementType.runAccelAmount * movementType.accelInAir : movementType.runDeccelAmount * movementType.deccelInAir;
        }

        //Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
        if ((isJumping || isJumpFalling) && Mathf.Abs(rb.velocity.y) < movementType.jumpHangVelocityThreshold)
        {
            accelAmount *= movementType.jumpHangAccelerationMult;
            targetSpeed *= movementType.jumpHangMaxSpeedMult;
        }

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

    private void WallJump(int dir)
    {
        lastPressJumpTime = 0;
        lastOnGroundTime = 0;

        Vector2 force = new Vector2(movementType.wallJumpForce.x, movementType.wallJumpForce.y);
        force.x *= dir; //apply force in opposite direction of wall

        if (Mathf.Sign(rb.velocity.x) != Mathf.Sign(force.x))
            force.x -= rb.velocity.x;

        if (rb.velocity.y < 0) //checks whether player is falling, if so we subtract the velocity.y (counteracting force of gravity). This ensures the player always reaches our desired jump force or greater
            force.y -= rb.velocity.y;

        //Unlike in the run we want to use the Impulse mode.
        //The default mode will apply are force instantly ignoring masss
        rb.AddForce(force, ForceMode2D.Impulse);

    }
    private void Slide()
    {
        float speedDif = movementType.slideSpeed - rb.velocity.y;

        float movement = speedDif * movementType.slideAccelAmount;
        //So, we clamp the movement here to prevent any over corrections (these aren't noticeable in the Run)
        //The force applied can't be greater than the (negative) speedDifference * by how many times a second FixedUpdate() is called. For more info research how force are applied to rigidbodies.
        //movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));
        rb.AddForce(movement * Vector2.up);
    }

    private void Attack()
    {

    }

    private void OnJumpInput()
    {
        lastPressJumpTime = movementType.jumpInputBufferTime;
    }

    private void OnJumpCutInput()
    {
        if (CanJumpCut() == true)
        {
            isJumpCut = true;
        }
    }

    private bool CanJumpCut()
    {
        if (isJumping == true && rb.velocity.y > 0f)
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
        if (lastOnGroundTime > 0f && !isJumping && lastPressJumpTime > 0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CanWallJump()
    {
        if (lastPressJumpTime > 0f && lastOnWallTime > 0f && lastOnGroundTime <= 0f && !isJumping)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool CanSlide()
    {
        if (lastOnWallTime > 0f && !isJumping && lastOnGroundTime <= 0f && slideDir == isFacingRight && input.moveInput.x != 0)
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
        if (isMovingRight != isFacingRight)
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

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallCollisionChecker.position, wallCollisionCheckerSize);
    }
#endif
    #endregion
}
