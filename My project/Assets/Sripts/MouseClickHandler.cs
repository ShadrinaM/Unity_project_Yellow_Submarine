using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickHandler : MonoBehaviour
{
    public Transform submarineCenter; // ����� ���������
    public GameObject cylinderPrefab; // ������ ��������

    private Vector3? lastPoint = null; // ��������� ����� ����� (null �� ������� �����)

    void Update()
    {
        // ��������� ������� ����� ������ ����
        if (Input.GetMouseButtonDown(0))
        {
            // ������ ��� �� ������ � ���
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // ���� ��� �������� � ������ � �����������
            if (Physics.Raycast(ray, out hit))
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
                lastPoint = hitPoint;
            }
        }
    }

    void CreateCylinder(Vector3 startPoint, Vector3 endPoint)
    {
        // ��������� ����������� � �����
        Vector3 direction = endPoint - startPoint;
        float distance = direction.magnitude;

        // ������ �������
        GameObject cylinder = Instantiate(cylinderPrefab);

        // ������������� ������� (�������� ����� ��������� � �������� �������)
        cylinder.transform.position = startPoint + direction / 2;

        // ���������� �������, �� � ������ ��� Y
        cylinder.transform.up = direction.normalized;

        // ����������� ����� � ������ ��������
        cylinder.transform.localScale = new Vector3(3, distance / 2, 0.3f);

        // ������� ���������� ����������� �� ��������
        Collider collider = cylinder.GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true; // ������� �������������� ����� ������
        }
    }
}
