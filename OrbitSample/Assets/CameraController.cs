using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float sensitivty = 10f;

    void Update()
    {
        float cameraSize = Camera.main.orthographicSize;
        cameraSize += -Input.GetAxis("Mouse ScrollWheel") * sensitivty;
        Camera.main.orthographicSize = cameraSize;
    }
}
