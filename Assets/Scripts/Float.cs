using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float : MonoBehaviour
{
    [SerializeField] private float floatSpeed = 0.2f; // Speed of floating
    [SerializeField] private float floatHeight = 2f; // Height of the float

    private Vector2 originalPosition;

    void Start()
    {
        // Save the object's original position
        originalPosition = transform.position;
    }

    void Update()
    {
        // Calculate new position using a sine wave
        float newY = originalPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(originalPosition.x, newY, transform.position.z);
    }
}