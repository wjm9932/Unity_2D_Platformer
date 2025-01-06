using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Movement Type")]
public class EnemyMovementTypeSO : ScriptableObject
{
    //[Header("Gravity")]
    //[HideInInspector] public float gravityStrength; //Downwards force (gravity) needed for the desired jumpHeight and jumpTimeToApex.
    //[HideInInspector] public float gravityScale; //Strength of the player's gravity as a multiplier of gravity (set in ProjectSettings/Physics2D).
    //                                             //Also the value the player's rigidbody2D.gravityScale is set to.
    //[Space(5)]
    //public float fallGravityMult; //Multiplier to the player's gravityScale when falling.
    //public float maxFallSpeed; //Maximum fall speed (terminal velocity) of the player when falling.
    //[Space(5)]
    //public float fastFallGravityMult; //Larger multiplier to the player's gravityScale when they are falling and a downwards input is pressed.
    //                                  //Seen in games such as Celeste, lets the player fall extra fast if they wish.
    //public float maxFastFallSpeed; //Maximum fall speed(terminal velocity) of the player when performing a faster fall.

    //[Space(20)]

    [Header("Run")]
    public float runMaxSpeed; //Target speed we want the player to reach.
    public float runAcceleration; //The speed at which our player accelerates to max speed, can be set to runMaxSpeed for instant acceleration down to 0 for none at all
    [HideInInspector] public float runAccelAmount; //The actual force (multiplied with speedDiff) applied to the player.
    public float runDecceleration; //The speed at which our player decelerates from their current speed, can be set to runMaxSpeed for instant deceleration down to 0 for none at all
    [HideInInspector] public float runDeccelAmount; //Actual force (multiplied with speedDiff) applied to the player .

    [Header("Idle Time")]
    public float idleTime;

    private void OnValidate()
    {
        //Calculate are run acceleration & deceleration forces using formula: amount = ((1 / Time.fixedDeltaTime) * acceleration) / runMaxSpeed
        runAccelAmount = ((1 / Time.fixedDeltaTime) * runAcceleration) / runMaxSpeed;
        runDeccelAmount = ((1 / Time.fixedDeltaTime) * runDecceleration) / runMaxSpeed;

        #region Variable Ranges
        runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed);
        runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, runMaxSpeed);

        idleTime = Mathf.Max(0f, idleTime);

        #endregion
    }
}

