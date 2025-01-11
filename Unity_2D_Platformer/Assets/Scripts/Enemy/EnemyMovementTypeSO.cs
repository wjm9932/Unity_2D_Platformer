using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Movement Type")]
public class EnemyMovementTypeSO : ScriptableObject
{
    [Header("Patrol")]
    public float patrolMaxSpeed;
    [SerializeField] private float patrolAcceleration; 
    [HideInInspector] public float patrolAccelAmount; 
    [SerializeField] private float patrolDecceleration;
    [HideInInspector] public float patrolDeccelAmount;

    [Header("Track")]
    public float trackMaxSpeed;
    [SerializeField] private float trackAcceleration;
    [HideInInspector] public float trackAccelAmount;
    [SerializeField] private float trackDecceleration;
    [HideInInspector] public float trackDeccelAmount;

    [Header("Idle Time")]
    public float idleTime;

    [Header("Knockback")]
    public Vector2 knockbackForce;

    private void OnValidate()
    {
        //Calculate are run acceleration & deceleration forces using formula: amount = ((1 / Time.fixedDeltaTime) * acceleration) / runMaxSpeed
        patrolAccelAmount = ((1 / Time.fixedDeltaTime) * patrolAcceleration) / patrolMaxSpeed;
        patrolDeccelAmount = ((1 / Time.fixedDeltaTime) * patrolDecceleration) / patrolMaxSpeed;

        trackAccelAmount = ((1 / Time.fixedDeltaTime) * trackAcceleration) / trackMaxSpeed;
        trackDeccelAmount = ((1 / Time.fixedDeltaTime) * trackDecceleration) / trackMaxSpeed;

        #region Variable Ranges
        patrolAcceleration = Mathf.Clamp(patrolAcceleration, 0.01f, patrolMaxSpeed);
        patrolDecceleration = Mathf.Clamp(patrolDecceleration, 0.01f, patrolMaxSpeed);

        trackAcceleration = Mathf.Clamp(trackAcceleration, 0.01f, trackMaxSpeed);
        trackDecceleration = Mathf.Clamp(trackDecceleration, 0.01f, trackMaxSpeed);

        idleTime = Mathf.Max(0f, idleTime);

        #endregion
    }
}

