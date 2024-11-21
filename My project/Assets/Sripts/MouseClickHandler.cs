using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickHandler : MonoBehaviour
{
    public Transform submarineCenter; // ����� ���������
    public GameObject cylinderPrefab; // ������ ��������

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
                CreateCylinder(submarineCenter.position, hitPoint); // ������ �������
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
        cylinder.transform.position = startPoint + direction / 2; // ������������� ������� (�������� ����� �������)
        cylinder.transform.rotation = Quaternion.LookRotation(direction); // ���������� ������� � ����
        cylinder.transform.localScale = new Vector3(8, distance / 2, 8); // ��������� ������
    }

}
