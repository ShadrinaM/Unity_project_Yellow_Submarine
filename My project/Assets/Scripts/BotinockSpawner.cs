using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    // ������ ��� �������� �������� Botinock
    public GameObject botinockPrefab;

    // ����� �������� ����� ��������� ��������
    public float delayTime = 4f;

    // ����� ���������� �������� ��� ��������
    public int objectCount = 6;

    // ����������� ���������� ����� ���������
    public float minDistance = 400f;

    //������������?
    public bool Povorot = true;

    [Min(0)] public float mashtab = 1;

    public Transform Submarin;

    //���� ��������
    private LayerMask RockMask;

    // ������ ��� �������� ������� ��������� ��������
    private List<Vector3> spawnedPositions = new List<Vector3>();

    private bool hasSpawned = false;

    void Start()
    {
        RockMask = LayerMask.GetMask("ROCKS");
        // ��������� �������� ��� �������� �������� � ���������
        StartCoroutine(SpawnObjectsWithDelay());
    }

    private IEnumerator SpawnObjectsWithDelay()
    {
        // ���� ��������� �����
        yield return new WaitForSeconds(delayTime);

        // ���� ������� ��� ���� �������, �� ������� �� �����
        if (hasSpawned)
        {
            yield break;
        }

        // ���������, ��� ������ �����
        if (botinockPrefab == null)
        {
            Debug.LogError("Botinock prefab is not assigned!");
            yield break;
        }

        // �������� ������
        Camera mainCamera = Camera.main;

        // �������� ���������� ����� ��������� ������
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, mashtab, 0));

        Debug.Log($"B:{bottomLeft}, T:{topRight}");
        Debug.DrawLine(bottomLeft, topRight);

        // ������� ������� � ��������� ������ � �������� ��������� ������
        for (int i = 0; i < objectCount; i++)
        {
            Vector3 spawnPosition = Vector3.zero;

            while (true)
            {
                // ���������� ��������� ���������� � �������� ��������� ������
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

            // ������� ������ � ��������� �������
            GameObject Boshmak =  Instantiate(botinockPrefab, spawnPosition, Quaternion.identity);
            if (Povorot)
                Boshmak.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360)); //������� ���������

            // ��������� ������� � ������ ��������� ��������
            spawnedPositions.Add(spawnPosition);
        }

        // ������������� ����, ��� ������� �������
        hasSpawned = true;
    }
}
