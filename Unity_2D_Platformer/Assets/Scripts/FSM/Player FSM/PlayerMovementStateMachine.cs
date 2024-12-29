using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementStateMachine : StateMachine
{
    public Player owner { get; private set; }

    public IdleState idleState { get; private set; }
    public RunState runState { get; private set; }
    public JumpState jumpState { get; private set; }
    public PlayerMovementStateMachine(Player player)
    {
        this.owner = player;

        idleState = new IdleState(this);
        runState = new RunState(this);
        jumpState = new JumpState(this);
    }
}
