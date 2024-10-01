using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    public GameObject clickEffectPrefab;
    private Camera mainCamera;
    private HighlightableObject currentHighlightedObject;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        HandleHover();
        HandleClick();
    }

    void HandleHover()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            HighlightableObject highlightable = hit.collider.GetComponent<HighlightableObject>();

            if (highlightable != null && highlightable != currentHighlightedObject)
            {
                // De-highlight previously highlighted object
                if (currentHighlightedObject != null)
                {
                    currentHighlightedObject.DeHighlight();
                }

                // Highlight the new object
                currentHighlightedObject = highlightable;
                currentHighlightedObject.Highlight();
            }
        }
        else if (currentHighlightedObject != null)
        {
            // De-highlight when not hovering over any object
            currentHighlightedObject.DeHighlight();
            currentHighlightedObject = null;
        }
    }

    void HandleClick()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Spawn click effect
                SpawnClickEffect(hit.point);

                // Check if the hit object has a clickable component
                IClickable clickable = hit.collider.GetComponent<IClickable>();
                if (clickable != null)
                {
                    clickable.OnClick();
                }
            }
        }
    }

    void SpawnClickEffect(Vector3 position)
    {
        if (clickEffectPrefab != null)
        {
            GameObject effect = Instantiate(clickEffectPrefab, position, Quaternion.identity);
            effect.GetComponent<ClickEffect>().Play();
        }
    }
}

// Interface for clickable objects
public interface IClickable
{
    void OnClick();
}