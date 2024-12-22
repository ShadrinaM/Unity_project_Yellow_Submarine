using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    // Префаб для создания объектов Botinock
    public GameObject botinockPrefab;

    // Время задержки перед созданием объектов
    public float delayTime = 4f;

    // Общее количество объектов для создания
    public int objectCount = 6;

    // Минимальное расстояние между объектами
    public float minDistance = 400f;

    //поворачивать?
    public bool Povorot = true;

    [Min(0)] public float mashtab = 1;

    public Transform Submarin;

    //слой камешков
    private LayerMask RockMask;

    // Список для хранения позиций созданных объектов
    private List<Vector3> spawnedPositions = new List<Vector3>();

    private bool hasSpawned = false;

    void Start()
    {
        RockMask = LayerMask.GetMask("ROCKS");
        // Запускаем корутину для создания объектов с задержкой
        StartCoroutine(SpawnObjectsWithDelay());
    }

    private IEnumerator SpawnObjectsWithDelay()
    {
        // Ждем указанное время
        yield return new WaitForSeconds(delayTime);

        // Если объекты уже были созданы, не создаем их снова
        if (hasSpawned)
        {
            yield break;
        }

        // Проверяем, что префаб задан
        if (botinockPrefab == null)
        {
            Debug.LogError("Botinock prefab is not assigned!");
            yield break;
        }

        // Получаем камеру
        Camera mainCamera = Camera.main;

        // Получаем координаты углов видимости камеры
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, mashtab, 0));

        Debug.Log($"B:{bottomLeft}, T:{topRight}");
        Debug.DrawLine(bottomLeft, topRight);

        // Создаем объекты в случайных точках в пределах видимости камеры
        for (int i = 0; i < objectCount; i++)
        {
            Vector3 spawnPosition = Vector3.zero;

            while (true)
            {
                // Генерируем случайные координаты в пределах видимости камеры
                float spawnX = Random.Range(bottomLeft.x + 30, topRight.x - 30);
                float spawnY = Random.Range(bottomLeft.y + 30, topRight.y - 30);

                spawnPosition = new Vector3(spawnX, spawnY, transform.position.z);
                Vector3 displacement = spawnPosition - transform.position;
                if (Physics.Raycast(transform.position, displacement, out RaycastHit hit, displacement.magnitude, RockMask))
                {
                    continue;
                }

                if ((spawnPosition - Submarin.position).magnitude > 50)
                {
                    break;
                }
            }

            // Создаем объект в найденной позиции
            GameObject Boshmak =  Instantiate(botinockPrefab, spawnPosition, Quaternion.identity);
            if (Povorot)
                Boshmak.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360)); //поворот ботиночка

            // Добавляем позицию в список созданных объектов
            spawnedPositions.Add(spawnPosition);
        }

        // Устанавливаем флаг, что объекты созданы
        hasSpawned = true;
    }
}
