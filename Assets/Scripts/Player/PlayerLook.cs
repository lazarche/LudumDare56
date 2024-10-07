using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    Camera cam;
    private float xRotation = 0;


    private void Awake()
    {
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ProcessLook(Vector2 input)
    {
        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        xRotation -= (input.y * Time.deltaTime) * SensitivityManager.Instance.sensitivity;
        xRotation = Mathf.Clamp(xRotation, -80, 80);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.Rotate(Vector3.up * (input.x * Time.deltaTime) * SensitivityManager.Instance.sensitivity);
    }
}
