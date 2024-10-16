using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableCamera : MonoBehaviour
{
    [Header("Drag Settings")]
    public float dragSpeed = 2f;
    public bool invertDrag = false;

    [Header("Movement Constraints")]
    public float minX = -10f;
    public float maxX = 10f;

    [Header("Angle Settings")]
    [Range(0, 90)] public float cameraAngle = 45f;

    private Vector3 dragOrigin;
    private Camera mainCamera;
    private float yPosition;
    private float zPosition;

    void Start()
    {
        mainCamera = Camera.main;

        // Set initial camera angle
        transform.rotation = Quaternion.Euler(cameraAngle, 0, 0);

        // Store initial Y and Z positions
        yPosition = transform.position.y;
        zPosition = transform.position.z;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(0)) return;

        Vector3 pos = mainCamera.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(pos.x * dragSpeed, 0, 0);

        if (invertDrag)
        {
            move = -move;
        }

        Vector3 newPosition = transform.position + move;

        // Clamp the X position
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);

        // Maintain Y and Z positions
        newPosition.y = yPosition;
        newPosition.z = zPosition;

        transform.position = newPosition;

        dragOrigin = Input.mousePosition;
    }

    public void SetCameraAngle(float angle)
    {
        cameraAngle = Mathf.Clamp(angle, 0, 90);
        transform.rotation = Quaternion.Euler(cameraAngle, 0, 0);
    }
}
