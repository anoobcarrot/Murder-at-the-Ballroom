using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private KeyCode toggleKey = KeyCode.I;
    [SerializeField] private GameObject clickableObject; // Reference to the clickable object
    private bool isInventoryOpen = false;
    private Camera mainCamera;

    private void Start()
    {
        // Ensure inventory starts closed
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
        }

        // Get reference to main camera
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Check for key input
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleInventory();
        }

        // Check for mouse click
        if (Input.GetMouseButtonDown(0)) // Left mouse click
        {
            CheckClickableObject();
        }
    }

    private void CheckClickableObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Check if we hit the designated clickable object
            if (hit.collider.gameObject == clickableObject)
            {
                ToggleInventory();
            }
        }
    }

    public void ToggleInventory()
    {
        if (inventoryPanel != null)
        {
            isInventoryOpen = !isInventoryOpen;
            inventoryPanel.SetActive(isInventoryOpen);
            // Optional: Pause game when inventory is open
            // Time.timeScale = isInventoryOpen ? 0f : 1f;
            // Optional: Control cursor visibility and lock state
            // Cursor.visible = isInventoryOpen;
            // Cursor.lockState = isInventoryOpen ? CursorLockMode.Locked : CursorLockMode.None;
        }
        else
        {
            Debug.LogWarning("Inventory Panel reference is missing!");
        }
    }

    public void CloseInventory()
    {
        if (isInventoryOpen)
        {
            ToggleInventory();
        }
    }

    public bool IsInventoryOpen()
    {
        return isInventoryOpen;
    }
}