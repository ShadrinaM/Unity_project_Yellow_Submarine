using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // �������� �������� (����� ��������� � ����������)
    public Vector3 rotationSpeed = new Vector3(50, 0, 0);

    void Update()
    {
        // �������� ������� ������ ����������� ���
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
