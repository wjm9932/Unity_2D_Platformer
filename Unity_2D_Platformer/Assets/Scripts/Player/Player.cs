using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Player : LivingEntity
{
    [Space(20)]
    [Header("Player Components")]

    [Header("Health")]
    [SerializeField] private IndicatorManager heartBar;
    [SerializeField] private int maxHearts;
    private int _heartCount;
    private int heartCount
    {
        set
        {
            _heartCount = Mathf.Clamp(value, 0, maxHearts);
            heartBar.UpdateCount(_heartCount);
        }
        get
        {
            return _heartCount;
        }
    }

    [Header("Dash Indicator")]
    [SerializeField] private IndicatorManager dash;
    [SerializeField] private int maxDashCount;
    private int _dashCount;
    public int dashCount
    {
        set
        {
            _dashCount = Mathf.Clamp(value, 0, maxDashCount);
            dash.UpdateCount(_dashCount);
        }
        get 
        {
            return _dashCount; 
        }
    }

    [Header("Movement  Type")]
    public MovementTypeSO movementType;


    #region LayerMask
    [Header("Layer")]
    public LayerMask enemyLayer;
    public LayerMask enemyCollisionBoxLayer;
    public LayerMask enemyHeadCollisionBoxLayer;
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

    public Rigidbody2D rb { get; private set; }
    public PlayerInput input { get; private set; }
    public float facingDir { get; private set; }
    private Vector2 currentCheckpointPosition;

    public GameObject a;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();
        movementStateMachine = new PlayerMovementStateMachine(this);

        heartBar.SetStartCount(maxHearts);
        dash.SetStartCount(maxDashCount);
    }

    protected override void Start()
    {
        base.Start();
        heartCount = maxHearts;
        dashCount = maxDashCount;
        currentCheckpointPosition = this.transform.position;

        movementStateMachine.ChangeState(movementStateMachine.runState);
        movementStateMachine.jsm.ChangeState(movementStateMachine.jsm.idleState);
    }

    void Update()
    {
        //Debug.Log(movementStateMachine.currentState);
        #region Timer
        lastOnGroundTime -= Time.deltaTime;
        lastOnWallTime -= Time.deltaTime;
        lastPressJumpTime -= Time.deltaTime;
        #endregion

        if (Input.GetKeyDown(KeyCode.R) == true && isDead == true)
        {
            RespawnPlayer(currentCheckpointPosition);
        }
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

        if (movementStateMachine.jsm.currentState == movementStateMachine.jsm.jumpFallingState && movementStateMachine.jsm.currentState != movementStateMachine.jsm.jumpAttackState)
        {
            var collider = Physics2D.OverlapBox(groundChecker.position, groundCheckSize, 0, enemyHeadCollisionBoxLayer);
            if (collider != null)
            {
                if (collider.transform.root.GetComponent<Enemy>().ApplyDamage(Mathf.Abs(rb.velocity.y) * 0.4f, this.gameObject) == true)
                {
                    movementStateMachine.jsm.ChangeState(movementStateMachine.jsm.jumpAttackState);
                }
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
    public override void Die()
    {
        base.Die();
        isDead = true;
        rb.isKinematic = true;
        GetComponent<Collider2D>().enabled = false;
        movementStateMachine.ChangeState(movementStateMachine.dieState);
    }

    public override bool ApplyDamage(float dmg, GameObject damager)
    {
        if (!(movementStateMachine.currentState is AttackState) && movementStateMachine.currentState != movementStateMachine.hitState)
        {
            if (base.ApplyDamage(dmg, damager) == true)
            {
                heartCount -= (int)dmg;


                if (heartCount <= 0)
                {
                    Die();
                }
                else
                {
                    movementStateMachine.ChangeState(movementStateMachine.hitState);
                }
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

    private void RespawnPlayer(Vector2 respawnPos)
    {
        transform.position = respawnPos;
        heartCount = maxHearts;
        isDead = false;
        rb.isKinematic = false;
        GetComponent<Collider2D>().enabled = true;
        movementStateMachine.ChangeState(movementStateMachine.runState);
        movementStateMachine.jsm.ChangeState(movementStateMachine.jsm.idleState);
    }

    public override void KillInstant()
    {
        heartCount = 0;
        base.KillInstant();
    }


    public void SetCheckPoint(Vector2 pos)
    {
        currentCheckpointPosition = pos;
    }

    public bool RecoverHealth()
    {
        return heartCount++ < maxHearts;
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
