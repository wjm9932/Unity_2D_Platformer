using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    public Enemy owner { get; private set; }
    public EnemyIdleState enemyIdleState { get; private set; }
    public PatrolState patrolState { get; private set; }
    public TrackState trackState { get; private set; }
    public EnemyHitState enemyHitState { get; private set; }
    public EnemyDieState enemyDieState { get; private set; }


    public EnemyStateMachine(Enemy owner)
    {
        this.owner = owner;

        enemyIdleState = new EnemyIdleState(this);
        patrolState = new PatrolState(this);
        trackState = new TrackState(this);
        enemyHitState = new EnemyHitState(this);
        enemyDieState = new EnemyDieState(this);
    }
}
