using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightableObject : MonoBehaviour
{
    public Color highlightColor = Color.yellow;
    public float highlightIntensity = 0.5f;

    private Renderer objectRenderer;
    private Material originalMaterial;
    private Material highlightMaterial;

    private void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        originalMaterial = objectRenderer.material;

        // Create a new material for highlighting
        highlightMaterial = new Material(originalMaterial);
        highlightMaterial.EnableKeyword("_EMISSION");
    }

    public void Highlight()
    {
        objectRenderer.material = highlightMaterial;
        highlightMaterial.SetColor("_EmissionColor", highlightColor * highlightIntensity);
    }

    public void DeHighlight()
    {
        objectRenderer.material = originalMaterial;
    }

    private void OnDestroy()
    {
        // Clean up the created material
        Destroy(highlightMaterial);
    }
}