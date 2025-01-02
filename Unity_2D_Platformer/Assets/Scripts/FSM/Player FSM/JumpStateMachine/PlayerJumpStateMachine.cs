using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerJumpStateMachine : StateMachine
{
    public Player owner { get; private set; }
    public IdleState idleState { get; private set; }
    public JumpState jumpState { get; private set; }
    public JumpFallingState jumpFallingState { get; private set; }
    public SlideState slideState { get; private set; }
    public WallJumpState wallJumpState { get; private set; }
    public FallingState fallingState { get; private set; }

    public bool isJumpCut;
    public float facingDir;
    public PlayerJumpStateMachine(Player owner)
    {
        this.owner = owner;

        idleState = new IdleState(this);
        jumpState = new JumpState(this);
        jumpFallingState = new JumpFallingState(this);
        slideState = new SlideState(this);
        wallJumpState = new WallJumpState(this);
        fallingState = new FallingState(this);

    }
}
