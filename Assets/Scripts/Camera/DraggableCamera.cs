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
    private bool isActuallyDragging = false; // New flag to differentiate a click from a drag
    private Quaternion initialRotation;
    private const float dragThreshold = 5f; // Minimum pixel movement to start dragging

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
            isActuallyDragging = false; // Reset actual dragging flag
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            isActuallyDragging = false; // Reset when mouse is released
        }

        if (isDragging)
        {
            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 deltaMouse = currentMousePosition - lastMousePosition;

            // Check if drag threshold is exceeded to treat it as an actual drag
            if (!isActuallyDragging && deltaMouse.magnitude > dragThreshold)
            {
                isActuallyDragging = true;
            }

            if (isActuallyDragging)
            {
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
            }

            lastMousePosition = currentMousePosition;
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



