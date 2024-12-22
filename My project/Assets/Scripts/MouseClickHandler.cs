using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickHandler : MonoBehaviour
{
    public Transform submarineCenter; // Центр субмарины
    public GameObject cylinderPrefab; // Префаб цилиндра

    private Vector3? lastPoint = null; // Последняя точка клика (null до первого клика)
    private bool isClickEnabled = false; // Флаг, указывающий на то, что клики можно обрабатывать
    private List <Material> materials = new List<Material>();
    private List <GameObject> cylinders = new List<GameObject>();
    private LayerMask Hitmack;
    //слой камешков
    private LayerMask RockMask;

    void Start()
    {
        RockMask = LayerMask.GetMask("ROCKS");
        Hitmack = LayerMask.GetMask("Default");
        // Сохраняем время запуска программы
        StartCoroutine(EnableClicksAfterDelay(4f)); // Включаем клики через 4 секунды
    }

    void Update()
    {
        // Проверяем, можно ли обрабатывать клики
        if (!isClickEnabled) return;

        // Проверяем нажатие левой кнопки мыши
        if (Input.GetMouseButtonDown(0))
        {
            // Создаём луч из экрана в мир
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Если луч попадает в объект с коллайдером
            if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Water")))
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
                
            }
        }

    }

    IEnumerator EnableClicksAfterDelay(float delay)
    {
        // Ждём указанное количество секунд
        yield return new WaitForSeconds(delay);
        // Разрешаем обрабатывать клики
        isClickEnabled = true;
    }

    void CreateCylinder(Vector3 startPoint, Vector3 endPoint)
    {
        // Вычисляем направление и длину
        Vector3 direction = endPoint - startPoint;
        float distance = direction.magnitude;

        Vector3 displacement = startPoint - endPoint;
        if (Physics.Raycast(endPoint, displacement, out RaycastHit hit, 5000, RockMask))
        {
            return;
        }

        // Создаём цилиндр
        GameObject cylinder = Instantiate(cylinderPrefab);
        materials.Add(cylinder.GetComponent<MeshRenderer>().material);
        cylinders.Add(cylinder);

        // Устанавливаем позицию (середина между начальной и конечной точками)
        cylinder.transform.position = startPoint + direction / 2;

        // Направляем цилиндр, но с учётом оси Y
        cylinder.transform.up = direction.normalized;

        // Настраиваем длину и ширину цилиндра
        cylinder.transform.localScale = new Vector3(3, distance / 2, 0.3f);

        if (Physics.SphereCast(startPoint, 2f, direction, out hit, distance, Hitmack))
        {
            if (hit.transform.TryGetComponent<Chest>(out Chest Chest))
            {
                Colorize(Color.green);
                Debug.Log("ПОБЕДА!");
            }
            else if(hit.transform.TryGetComponent<Boots>(out Boots Boots))
            {
                foreach (var cyl in cylinders)
                    Destroy(cyl);
                Colorize(Color.red);
                materials.Clear();
                cylinders.Clear();
                lastPoint = null;
                Debug.Log("НЕУДАЧА!");
            }
        }
        else
        {
            lastPoint = endPoint;
        }        
    }

    void Colorize(Color color)
    {
        foreach (var material in materials)
            material.color = color;
    }
}
