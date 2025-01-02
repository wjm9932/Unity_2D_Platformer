using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementStateMachine : StateMachine
{
    public Player owner { get; private set; }
    public PlayerJumpStateMachine jsm { get; private set; }
    public RunState runState { get; private set; }
    public PlayerMovementStateMachine(Player player)
    {
        this.owner = player;
        this.jsm = new PlayerJumpStateMachine(player);

        runState = new RunState(this);
    }
}
