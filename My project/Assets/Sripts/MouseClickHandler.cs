using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickHandler : MonoBehaviour
{
    public Transform submarineCenter; // Центр субмарины
    public GameObject cylinderPrefab; // Префаб цилиндра

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
                CreateCylinder(submarineCenter.position, hitPoint); // Создаём цилиндр
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
        cylinder.transform.position = startPoint + direction / 2; // Устанавливаем позицию (середина между точками)
        cylinder.transform.rotation = Quaternion.LookRotation(direction); // Направляем цилиндр к цели
        cylinder.transform.localScale = new Vector3(8, distance / 2, 8); // Увеличена ширина
    }

}
