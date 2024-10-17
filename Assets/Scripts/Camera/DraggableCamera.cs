using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableCamera : MonoBehaviour
{
    [Header("Drag Settings")]
    public float dragSpeed = 2f;
    public bool invertDrag = false;

    [Header("Horizontal Angle Constraints")]
    public float minHorizontalAngle = -90f;
    public float maxHorizontalAngle = 90f;

    private Vector3 lastMousePosition;
    private float rotationY = 0f;
    private bool isDragging = false;
    private Quaternion initialRotation;

    private void Start()
    {
        initialRotation = transform.rotation;
        rotationY = initialRotation.eulerAngles.y;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 deltaMouse = Input.mousePosition - lastMousePosition;
            float mouseX = deltaMouse.x * dragSpeed * Time.deltaTime;

            if (invertDrag)
            {
                mouseX = -mouseX;
            }

            // Calculate rotation
            rotationY += mouseX;

            // Clamp horizontal rotation
            float clampedRotationY = Mathf.Clamp(rotationY - initialRotation.eulerAngles.y, minHorizontalAngle, maxHorizontalAngle);
            rotationY = clampedRotationY + initialRotation.eulerAngles.y;

            // Apply rotation to the camera (only around Y-axis)
            transform.rotation = Quaternion.Euler(initialRotation.eulerAngles.x, rotationY, initialRotation.eulerAngles.z);

            lastMousePosition = Input.mousePosition;
        }
    }

    public void SetDragSpeed(float speed)
    {
        dragSpeed = speed;
    }

    public void ToggleInvertDrag(bool invert)
    {
        invertDrag = invert;
    }
}
