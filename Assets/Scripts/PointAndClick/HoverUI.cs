using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoverUI : MonoBehaviour
{
    public TextMeshProUGUI interactText;
    public float offset = 50f;

    private RectTransform rectTransform;
    private Camera mainCamera;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        mainCamera = Camera.main;
    }

    public void Setup(Vector3 worldPosition)
    {
        UpdatePosition(worldPosition);
    }

    public void UpdatePosition(Vector3 worldPosition)
    {
        if (mainCamera != null)
        {
            Vector2 screenPoint = mainCamera.WorldToScreenPoint(worldPosition);
            screenPoint.x += offset;
            rectTransform.position = screenPoint;
        }
    }
}