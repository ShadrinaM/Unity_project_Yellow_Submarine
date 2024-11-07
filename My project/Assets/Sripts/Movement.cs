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
        // Рассчитываем направление к целевой позиции
        Vector3 direction = (targetPosition.position - transform.position).normalized;

        // Рассчитываем новое положение на основе скорости
        Vector3 newPosition = transform.position + direction * speed * Time.fixedDeltaTime;

        // Ограничиваем перемещение, чтобы не пройти мимо цели
        if (Vector3.Distance(newPosition, targetPosition.position) < speed * Time.fixedDeltaTime)
        {
            newPosition = targetPosition.position;
        }

        // Перемещаем объект с помощью MovePosition
        rb.MovePosition(newPosition);
    }
}
