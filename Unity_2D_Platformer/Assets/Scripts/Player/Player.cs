using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Player : LivingEntity
{
    [Space(20)]
    [Header("Player Components")]

    [Header("Movement  Type")]
    public MovementTypeSO movementType;

    public Rigidbody2D rb { get; private set; }
    public PlayerInput input { get; private set; }

    #region LayerMask
    [Header("Layer")]
    public LayerMask enemyLayer;
    public LayerMask enemyCollisionBoxLayer;
    public LayerMask whatIsGround;
    #endregion

    #region Collision Check Parameteres
    [Header("Collision Check")]
    public Transform groundChecker;
    public Vector2 groundCheckSize = new Vector2();
    public Transform wallCollisionChecker;
    public Vector2 wallCollisionCheckerSize = new Vector2();
    #endregion

    #region Attack Root
    [Header("Attack Root")]
    public Transform attackRoot;
    public float attackRange;
    #endregion

    #region State Machine
    private PlayerMovementStateMachine movementStateMachine;
    #endregion

    #region Timer
    [HideInInspector] public float lastOnGroundTime;
    [HideInInspector] public float lastPressJumpTime;
    [HideInInspector] public float lastOnWallTime;
    #endregion

    public float facingDir { get; private set; }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();
        movementStateMachine = new PlayerMovementStateMachine(this);
    }

    void Start()
    {
        movementStateMachine.ChangeState(movementStateMachine.runState);
        movementStateMachine.jsm.ChangeState(movementStateMachine.jsm.idleState);
    }

    void Update()
    {
        #region Timer
        lastOnGroundTime -= Time.deltaTime;
        lastOnWallTime -= Time.deltaTime;
        lastPressJumpTime -= Time.deltaTime;
        #endregion

        #region Collision Check
        if (movementStateMachine.jsm.currentState != movementStateMachine.jsm.jumpState && movementStateMachine.jsm.currentState != movementStateMachine.jsm.wallJumpState)
        {
            if (Physics2D.OverlapBox(groundChecker.position, groundCheckSize, 0, whatIsGround) == true) //checks if set box overlaps with ground
            {
                lastOnGroundTime = movementType.coyoteTime; //if so sets the lastGrounded to coyoteTime
            }

            if (Physics2D.OverlapBox(wallCollisionChecker.position, wallCollisionCheckerSize, 0, whatIsGround))
            {
                lastOnWallTime = movementType.wallJumpCoyoteTime;
                facingDir = transform.localRotation.y < 0 ? -1f : 1f;
            }
        }
        #endregion

        movementStateMachine.Update();
        movementStateMachine.jsm.Update();
    }

    private void FixedUpdate()
    {
        movementStateMachine.FixedUpdate();
        movementStateMachine.jsm.FixedUpdate();
    }

    private void LateUpdate()
    {
        movementStateMachine.LateUpdate();
        movementStateMachine.jsm.LateUpdate();

        animHandler.animator.SetBool("IsRun", input.moveInput.x != 0);
        animHandler.animator.SetFloat("VelocityY", rb.velocity.y);
    }

    public override void OnAnimationEnterEvent()
    {
        movementStateMachine.OnAnimationEnterEvent();
    }
    public override void OnAnimationTransitionEvent()
    {
        movementStateMachine.OnAnimationTransitionEvent();
    }
    public override void OnAnimationExitEvent()
    {
        movementStateMachine.OnAnimationExitEvent();
    }
    public void SetGravityScale(float scale)
    {
        rb.gravityScale = scale;
    }

    public override bool ApplyDamage(float dmg, LivingEntity damager)
    {
        if(!(movementStateMachine.currentState is AttackState) && movementStateMachine.currentState != movementStateMachine.hitState)
        {
            if(base.ApplyDamage(dmg, damager) == true)
            {
                movementStateMachine.ChangeState(movementStateMachine.hitState);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    

    #region EDITOR METHODS
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundChecker.position, groundCheckSize);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallCollisionChecker.position, wallCollisionCheckerSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackRoot.position, attackRange);
    }
#endif
    #endregion
}
