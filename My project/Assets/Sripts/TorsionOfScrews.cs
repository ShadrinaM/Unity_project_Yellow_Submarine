using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Скорость вращения (можно настроить в инспекторе)
    public Vector3 rotationSpeed = new Vector3(50, 0, 0);

    void Update()
    {
        // Вращение объекта вокруг собственной оси
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
