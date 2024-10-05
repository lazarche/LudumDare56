using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    Camera cam;
    private float xRotation = 0;

    public float xSensitivity = 30f;
    private float ySensitivity = 30f;

    private void Awake()
    {
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ProcessLook(Vector2 input)
    {
        xRotation -= (input.y * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80, 80);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.Rotate(Vector3.up * (input.x * Time.deltaTime) * xSensitivity);
    }
}
