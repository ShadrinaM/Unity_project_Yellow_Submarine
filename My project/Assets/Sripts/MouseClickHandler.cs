using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickHandler : MonoBehaviour
{
    public Transform submarineCenter; // Центр субмарины
    public GameObject cylinderPrefab; // Префаб цилиндра

    private Vector3? lastPoint = null; // Последняя точка клика (null до первого клика)

    void Update()
    {
        // Проверяем нажатие левой кнопки мыши
        if (Input.GetMouseButtonDown(0))
        {
            // Создаём луч из экрана в мир
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Если луч попадает в объект с коллайдером
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 hitPoint = hit.point; // Точка, куда попал луч

                if (lastPoint == null)
                {
                    // Если это первый клик, строим от субмарины
                    CreateCylinder(submarineCenter.position, hitPoint);
                }
                else
                {
                    // Если это не первый клик, строим от последней точки
                    CreateCylinder(lastPoint.Value, hitPoint);
                }

                // Сохраняем текущую точку как последнюю
                lastPoint = hitPoint;
            }
        }
    }

    void CreateCylinder(Vector3 startPoint, Vector3 endPoint)
    {
        // Вычисляем направление и длину
        Vector3 direction = endPoint - startPoint;
        float distance = direction.magnitude;

        // Создаём цилиндр
        GameObject cylinder = Instantiate(cylinderPrefab);

        // Устанавливаем позицию (середина между начальной и конечной точками)
        cylinder.transform.position = startPoint + direction / 2;

        // Направляем цилиндр, но с учётом оси Y
        cylinder.transform.up = direction.normalized;

        // Настраиваем длину и ширину цилиндра
        cylinder.transform.localScale = new Vector3(3, distance / 2, 0.3f);

        // Убираем физическое воздействие от цилиндра
        Collider collider = cylinder.GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true; // Убираем взаимодействие через физику
        }
    }
}
