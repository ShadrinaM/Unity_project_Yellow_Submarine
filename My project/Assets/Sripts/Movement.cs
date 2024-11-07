using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody rb;
    public Transform targetPosition;
    public float speed = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // ������������ ����������� � ������� �������
        Vector3 direction = (targetPosition.position - transform.position).normalized;

        // ������������ ����� ��������� �� ������ ��������
        Vector3 newPosition = transform.position + direction * speed * Time.fixedDeltaTime;

        // ������������ �����������, ����� �� ������ ���� ����
        if (Vector3.Distance(newPosition, targetPosition.position) < speed * Time.fixedDeltaTime)
        {
            newPosition = targetPosition.position;
        }

        // ���������� ������ � ������� MovePosition
        rb.MovePosition(newPosition);
    }
}
