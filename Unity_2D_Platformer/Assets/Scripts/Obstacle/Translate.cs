using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translate : MonoBehaviour, IInteractable
{
    [SerializeField] private bool isTurnOn;
    [SerializeField] private float moveDistance = 5f;
    [SerializeField] private float moveSpeed = 2f;

    private Vector3 startPos;
    private float timeElapsed;

    void Start()
    {
        startPos = transform.position;
        timeElapsed = 0f;
    }

    void Update()
    {
        if(isTurnOn)
        {
            timeElapsed += Time.deltaTime;
            float offset = Mathf.Sin(timeElapsed * moveSpeed) * moveDistance;
            transform.position = startPos + new Vector3(offset, 0, 0);
        }
    }

    public void Trigger()
    {
        isTurnOn = !isTurnOn;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 leftPoint = transform.position - new Vector3(moveDistance, 0, 0);
        Vector3 rightPoint = transform.position + new Vector3(moveDistance, 0, 0);
        Gizmos.DrawLine(leftPoint, rightPoint);
    }
#endif
}
