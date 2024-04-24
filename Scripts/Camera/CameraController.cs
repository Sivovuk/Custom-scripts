using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    [SerializeField] private Vector3 distance;
    [SerializeField] private Vector2 framingOffset;
    [SerializeField] private float minVerticalValue = -45;
    [SerializeField] private float maxVerticalValue = 65;

    [SerializeField] private float rotationSpeed = 5;

    float rotateX;
    float rotateY;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        rotateX += Input.GetAxis("Mouse Y") * rotationSpeed;
        rotateX = Mathf.Clamp(rotateX, minVerticalValue, maxVerticalValue);

        rotateY += Input.GetAxis("Mouse X");

        var targetRotation = Quaternion.Euler(rotateX, rotateY, 0);

        var focusPosition = followTarget.position + new Vector3(framingOffset.x, framingOffset.y);

        transform.position = focusPosition - targetRotation * distance;
        transform.rotation = targetRotation;
    }

    public Quaternion PlanerRotation => Quaternion.Euler(0, rotateY, 0);
}
