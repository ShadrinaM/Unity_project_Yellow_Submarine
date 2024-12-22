using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickHandler : MonoBehaviour
{
    public Transform submarineCenter; // ����� ���������
    public GameObject cylinderPrefab; // ������ ��������

    private Vector3? lastPoint = null; // ��������� ����� ����� (null �� ������� �����)
    private bool isClickEnabled = false; // ����, ����������� �� ��, ��� ����� ����� ������������
    private List <Material> materials = new List<Material>();
    private List <GameObject> cylinders = new List<GameObject>();
    private LayerMask Hitmack;
    //���� ��������
    private LayerMask RockMask;

    void Start()
    {
        RockMask = LayerMask.GetMask("ROCKS");
        Hitmack = LayerMask.GetMask("Default");
        // ��������� ����� ������� ���������
        StartCoroutine(EnableClicksAfterDelay(4f)); // �������� ����� ����� 4 �������
    }

    void Update()
    {
        // ���������, ����� �� ������������ �����
        if (!isClickEnabled) return;

        // ��������� ������� ����� ������ ����
        if (Input.GetMouseButtonDown(0))
        {
            // ������ ��� �� ������ � ���
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // ���� ��� �������� � ������ � �����������
            if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Water")))
            {
                Vector3 hitPoint = hit.point; // �����, ���� ����� ���

                if (lastPoint == null)
                {
                    // ���� ��� ������ ����, ������ �� ���������
                    CreateCylinder(submarineCenter.position, hitPoint);
                }
                else
                {
                    // ���� ��� �� ������ ����, ������ �� ��������� �����
                    CreateCylinder(lastPoint.Value, hitPoint);
                }

                // ��������� ������� ����� ��� ���������
                
            }
        }

    }

    IEnumerator EnableClicksAfterDelay(float delay)
    {
        // ��� ��������� ���������� ������
        yield return new WaitForSeconds(delay);
        // ��������� ������������ �����
        isClickEnabled = true;
    }

    void CreateCylinder(Vector3 startPoint, Vector3 endPoint)
    {
        // ��������� ����������� � �����
        Vector3 direction = endPoint - startPoint;
        float distance = direction.magnitude;

        Vector3 displacement = startPoint - endPoint;
        if (Physics.Raycast(endPoint, displacement, out RaycastHit hit, 5000, RockMask))
        {
            return;
        }

        // ������ �������
        GameObject cylinder = Instantiate(cylinderPrefab);
        materials.Add(cylinder.GetComponent<MeshRenderer>().material);
        cylinders.Add(cylinder);

        // ������������� ������� (�������� ����� ��������� � �������� �������)
        cylinder.transform.position = startPoint + direction / 2;

        // ���������� �������, �� � ������ ��� Y
        cylinder.transform.up = direction.normalized;

        // ����������� ����� � ������ ��������
        cylinder.transform.localScale = new Vector3(3, distance / 2, 0.3f);

        if (Physics.SphereCast(startPoint, 2f, direction, out hit, distance, Hitmack))
        {
            if (hit.transform.TryGetComponent<Chest>(out Chest Chest))
            {
                Colorize(Color.green);
                Debug.Log("������!");
            }
            else if(hit.transform.TryGetComponent<Boots>(out Boots Boots))
            {
                foreach (var cyl in cylinders)
                    Destroy(cyl);
                Colorize(Color.red);
                materials.Clear();
                cylinders.Clear();
                lastPoint = null;
                Debug.Log("�������!");
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
