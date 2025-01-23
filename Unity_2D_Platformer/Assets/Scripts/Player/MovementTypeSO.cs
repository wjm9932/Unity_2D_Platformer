using UnityEngine;

[CreateAssetMenu(menuName = "Movement Type")] //Create a new playerData object by right clicking in the Project Menu then Create/Player/Player Data and drag onto the player
public class MovementTypeSO : ScriptableObject
{
    [Header("Gravity")]
    [HideInInspector] public float gravityStrength; //Downwards force (gravity) needed for the desired jumpHeight and jumpTimeToApex.
    [HideInInspector] public float gravityScale; //Strength of the player's gravity as a multiplier of gravity (set in ProjectSettings/Physics2D).
                                                 //Also the value the player's rigidbody2D.gravityScale is set to.
    [Space(5)]
    public float fallGravityMult; //Multiplier to the player's gravityScale when falling.
    public float maxFallSpeed; //Maximum fall speed (terminal velocity) of the player when falling.
    [Space(5)]
    public float fastFallGravityMult; //Larger multiplier to the player's gravityScale when they are falling and a downwards input is pressed.
                                      //Seen in games such as Celeste, lets the player fall extra fast if they wish.
    public float maxFastFallSpeed; //Maximum fall speed(terminal velocity) of the player when performing a faster fall.

    [Space(20)]

    [Header("Run")]
    public float runMaxSpeed; //Target speed we want the player to reach.
    [SerializeField] private float runAcceleration; //The speed at which our player accelerates to max speed, can be set to runMaxSpeed for instant acceleration down to 0 for none at all
    [HideInInspector] public float runAccelAmount; //The actual force (multiplied with speedDiff) applied to the player.
    [SerializeField] private float runDecceleration; //The speed at which our player decelerates from their current speed, can be set to runMaxSpeed for instant deceleration down to 0 for none at all
    [HideInInspector] public float runDeccelAmount; //Actual force (multiplied with speedDiff) applied to the player .
    [Space(5)]
    [Range(0f, 1)] public float accelInAir; //Multipliers applied to acceleration rate when airborne.
    [Range(0f, 1)] public float deccelInAir;

    [Space(20)]

    [Header("Jump")]
    [SerializeField] private float jumpHeight; //Height of the player's jump
    [SerializeField] private float jumpTimeToApex; //Time between applying the jump force and reaching the desired jump height. These values also control the player's gravity and jump force.
    [HideInInspector] public float jumpForce; //The actual force applied (upwards) to the player when they jump.

    [Header("Both Jumps")]
    public float jumpCutGravityMult; //Multiplier to increase gravity if the player releases thje jump button while still jumping
    [Range(0f, 1)] public float jumpHangGravityMult; //Reduces gravity while close to the apex (desired max height) of the jump
    public float jumpHangVelocityThreshold; //Speeds (close to 0) where the player will experience extra "jump hang". The player's velocity.y is closest to 0 at the jump's apex (think of the gradient of a parabola or quadratic function)
    [Space(0.5f)]
    public float jumpHangAccelerationMult;
    public float jumpHangMaxSpeedMult;

    [Header("Wall Jump")]
    public Vector2 wallJumpForce; //The actual force (this time set by us) applied to the player when wall jumping.

    [Header("Dobule Jump")]
    public Vector2 doubleJumpForce; //The actual force (this time set by us) applied to the player when wall jumping.

    [Header("Jump Attack Bounce")]
    [SerializeField] private float bounceHeight;
    [HideInInspector] public float bounceForce;

    [Space(20)]

    [Header("Slide")]
    public float slideSpeed;
    public float slideAccel;
    [HideInInspector] public float slideAccelAmount;

    [Header("Jump Assists")]
    [Range(0.01f, 0.5f)] public float coyoteTime; //Grace period after falling off a platform, where you can still jump
    [Range(0.01f, 0.5f)] public float jumpInputBufferTime; //Grace period after pressing jump where a jump will be automatically performed once the requirements (eg. being grounded) are met.
    [Range(0.01f, 0.5f)] public float wallJumpCoyoteTime;

    [Header("Attack Assit")]
    [Range(0.01f, 0.5f)] public float attackBufferTime;

    [Header("Knockback")]
    public Vector2 knockbackForce;

    [Header("Dash")]
    public float dashForce;
    [Range(0.01f, 0.5f)] public float dashInputBufferTime;


    //Unity Callback, called when the inspector updates
    private void OnValidate()
    {
        //Calculate gravity strength using the formula (gravity = 2 * jumpHeight / timeToJumpApex^2) 
        gravityStrength = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);

        //Calculate the rigidbody's gravity scale (ie: gravity strength relative to unity's gravity value, see project settings/Physics2D)
        gravityScale = gravityStrength / Physics2D.gravity.y;

        //Calculate are run acceleration & deceleration forces using formula: amount = ((1 / Time.fixedDeltaTime) * acceleration) / runMaxSpeed
        runAccelAmount = ((1 / Time.fixedDeltaTime) * runAcceleration) / runMaxSpeed;
        runDeccelAmount = ((1 / Time.fixedDeltaTime) * runDecceleration) / runMaxSpeed;

        //Calculate jumpForce using the formula (initialJumpVelocity = gravity * timeToJumpApex)
        jumpForce = Mathf.Abs(gravityStrength) * jumpTimeToApex;
        bounceForce = Mathf.Sqrt(2 * Mathf.Abs(gravityStrength) * bounceHeight);

        #region Variable Ranges
        runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed);
        runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, runMaxSpeed);

        slideAccel = Mathf.Clamp(slideAccel, 1f, Mathf.Abs(slideSpeed));

        if (slideSpeed == 0f)
        {
            slideAccelAmount = 1 / Time.fixedDeltaTime;
        }
        else
        {
            slideAccelAmount = ((1 / Time.fixedDeltaTime) * slideAccel) / Mathf.Abs(slideSpeed);
        }
        #endregion
    }
}