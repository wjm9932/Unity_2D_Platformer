using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    #region Time Calculator
    public static float CalculateTimeByDashForce(float dashForce, float decelerationFactor)
    {
        if (decelerationFactor <= 0f)
        {
            Debug.LogError("acceleration value is less than 0");
            return -1f;
        }

        float impulseForce = dashForce;

        float time = 0f;

        float velocity = impulseForce;
        float totalDistance = 0f;

        while (velocity > 0.1f)
        {
            float decelerationForce = velocity * (1 / Time.fixedDeltaTime) * decelerationFactor;
            float accelerationDecel = decelerationForce;
            velocity -= accelerationDecel * Time.fixedDeltaTime;

            float distanceThisFrame = velocity * Time.fixedDeltaTime;
            totalDistance += distanceThisFrame;

            time += Time.fixedDeltaTime;
        }

        return time;
    }

    public static float CalculateTimeForDistance(float distance, float decelerationFactor)
    {
        if (decelerationFactor <= 0f)
        {
            Debug.LogError("acceleration value is less than 0");
            return -1f;
        }

        float obstacleDistance = distance;

        float time = 0f;

        float velocity = 0f;
        float totalDistance = 0f;
        float requiredImpulse = 0f;
        float step = 0.1f;

        while (totalDistance < obstacleDistance)
        {
            time = 0f;
            totalDistance = 0f;
            velocity = requiredImpulse;

            while (velocity > 0.1f)
            {

                float decelerationForce = velocity * (1 / Time.fixedDeltaTime) * decelerationFactor;
                float accelerationDecel = decelerationForce;
                velocity -= accelerationDecel * Time.fixedDeltaTime;

                float distanceThisFrame = velocity * Time.fixedDeltaTime;
                totalDistance += distanceThisFrame;

                time += Time.fixedDeltaTime;
            }

            if (totalDistance >= obstacleDistance)
            {
                break;
            }

            requiredImpulse += step;
        }

        return time;
    }
    #endregion

    public static float CalculateDashDistanceByDashForce(float dashForce, float decelerationFactor)
    {
        float impulseForce = dashForce;

        float velocity = impulseForce;
        float totalDistance = 0f;

        while (velocity > 0.1f)
        {
            float decelerationForce = velocity * (1 / Time.fixedDeltaTime) * decelerationFactor;
            float accelerationDecel = decelerationForce;
            velocity -= accelerationDecel * Time.fixedDeltaTime;


            float distanceThisFrame = velocity * Time.fixedDeltaTime;
            totalDistance += distanceThisFrame;
        }
        return totalDistance;
    }

    public static float CalculateRequiredImpulseForDistance(float distance, float decelerationFactor)
    {
        float obstacleDistance = distance;


        float velocity = 0f;
        float totalDistance = 0f;
        float requiredImpulse = 0f;
        float step = 0.1f;

        while (totalDistance < obstacleDistance)
        {
            totalDistance = 0f;
            velocity = requiredImpulse;

            while (velocity > 0.1f)
            {

                float decelerationForce = velocity * (1 / Time.fixedDeltaTime) * decelerationFactor;
                float accelerationDecel = decelerationForce;
                velocity -= accelerationDecel * Time.fixedDeltaTime;

                float distanceThisFrame = velocity * Time.fixedDeltaTime;
                totalDistance += distanceThisFrame;

            }

            if (totalDistance >= obstacleDistance)
            {
                break;
            }

            requiredImpulse += step;
        }

        return requiredImpulse;
    }
}
