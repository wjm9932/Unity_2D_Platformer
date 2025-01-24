using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translate : MonoBehaviour
{
    [SerializeField] private float moveDistance = 5f; // �̵� �Ÿ�
    [SerializeField] private float moveSpeed = 2f;    // �̵� �ӵ�

    private Vector3 startPos;

    void Start()
    {
        // ���� ��ġ ����
        startPos = transform.position;
    }

    void Update()
    {
        // ���� �Լ��� �պ� �̵� ���
        float offset = Mathf.Sin(Time.time * moveSpeed) * moveDistance;
        transform.position = startPos + new Vector3(offset, 0, 0); // X������ �պ�
    }
}
