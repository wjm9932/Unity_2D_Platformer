using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translate : MonoBehaviour
{
    [SerializeField] private float moveDistance = 5f; // 이동 거리
    [SerializeField] private float moveSpeed = 2f;    // 이동 속도

    private Vector3 startPos;

    void Start()
    {
        // 시작 위치 저장
        startPos = transform.position;
    }

    void Update()
    {
        // 사인 함수로 왕복 이동 계산
        float offset = Mathf.Sin(Time.time * moveSpeed) * moveDistance;
        transform.position = startPos + new Vector3(offset, 0, 0); // X축으로 왕복
    }
}
