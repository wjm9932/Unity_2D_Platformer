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

    [Header("Jump Effect")]
    public GameObject jumpEffectPrefab;
    public GameObject doubleJumpEffectPrefab;
    public Transform jumpEffectTransform;
    public Transform doubleJumpEffectTransform;

    #region LayerMask
    [Header("Layer")]
    public LayerMask enemyLayer;
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
    public float playerFootOffset { get; private set; }
    private Vector2 currentCheckpointPosition;

    [HideInInspector] public bool isDashing = false;

    public float speedLimit { get; private set; }

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
        speedLimit = 1f;
        heartCount = maxHearts;
        dashCount = maxDashCount;
        currentCheckpointPosition = this.transform.position;
        playerFootOffset = (GetComponent<BoxCollider2D>().size.y / 2) * transform.lossyScale.y;
        movementStateMachine.ChangeState(movementStateMachine.runState);
        movementStateMachine.jsm.ChangeState(movementStateMachine.jsm.idleState);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) == true && isDead == true)
        {
            RespawnPlayer(currentCheckpointPosition);
        }

        #region Timer
        lastOnGroundTime -= Time.deltaTime;
        lastOnWallTime -= Time.deltaTime;
        lastPressJumpTime -= Time.deltaTime;
        #endregion

        #region Collision Check
        if (movementStateMachine.jsm.currentState != movementStateMachine.jsm.jumpState && movementStateMachine.jsm.currentState != movementStateMachine.jsm.wallJumpState)
        {
            if (Physics2D.OverlapBox(groundChecker.position, groundCheckSize, 0, whatIsGround) == true)
            {
                lastOnGroundTime = movementType.coyoteTime;
            }

            if (Physics2D.OverlapBox(wallCollisionChecker.position, wallCollisionCheckerSize, 0, whatIsGround))
            {
                lastOnWallTime = movementType.wallJumpCoyoteTime;
                facingDir = transform.localRotation.y < 0 ? -1f : 1f;
            }
        }

        if(CanJumpAttack() == true)
        {
            var collider = Physics2D.OverlapBox(groundChecker.position, groundCheckSize, 0, enemyHeadCollisionBoxLayer);
            if (collider != null)
            {
                var enemy = collider.GetComponentInParent<Enemy>();
                if (enemy.TakeDamage(Mathf.Abs(rb.velocity.y) * 0.4f, this.gameObject) == true)
                {
                    if(enemy.GetComponent<Boss>() != null)
                    {
                        enemy.GetComponent<Boss>().isHardAttack = true;
                    }
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

    private bool CanJumpAttack()
    {
        return (movementStateMachine.jsm.currentState == movementStateMachine.jsm.jumpFallingState || movementStateMachine.jsm.currentState == movementStateMachine.jsm.doubleJumpFallingState) && movementStateMachine.jsm.currentState != movementStateMachine.jsm.jumpAttackState;
    }
    public bool TakeDamage(GameObject damager, bool isHardAttack = false)
    {
        if (!CanTakeDamage(isHardAttack))
        {
            return false;
        }

        if (!base.ApplyDamage(dmg, damager))
        {
            return false;
        }

        heartCount--;

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

    private bool CanTakeDamage(bool isHardAttack)
    {
        if (movementStateMachine.currentState == movementStateMachine.hitState || isDashing)
        {
            return false;
        }

        if (!isHardAttack && movementStateMachine.currentState is AttackState)
        {
            return false;
        }

        return true;
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
        if (heartCount < maxHearts)
        {
            heartCount++;
            return true;
        }
        return false;
    }

    public bool RecoverDashCount()
    {
        if (dashCount < maxDashCount)
        {
            dashCount++;
            return true;
        }
        return false;
    }

    public void SetSpeedLimit(float limit)
    {
        speedLimit = 1f - limit;
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
