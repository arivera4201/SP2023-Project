using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] Camera playerCam;
    private float xRot;
    public float sensitivity = 10.0f;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void CameraMove(Vector2 input)
    {
        xRot -= (input.y * Time.deltaTime) * sensitivity;
        xRot = Mathf.Clamp(xRot, -90f, 90f);
        playerCam.transform.localRotation = Quaternion.Euler(xRot, 0, 0);
        transform.Rotate(Vector3.up * (input.x * Time.deltaTime) * sensitivity);
    }
}
