using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private KeyCode toggleKey = KeyCode.I;

    private bool isInventoryOpen = false;

    private void Start()
    {
        // Ensure inventory starts closed
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
        }
    }

    private void Update()
    {
        // Check for input
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleInventory();
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
         //   Cursor.visible = isInventoryOpen;
          //  Cursor.lockState = isInventoryOpen ? CursorLockMode.None : CursorLockMode.Locked;
        }
        else
        {
            Debug.LogWarning("Inventory Panel reference is missing!");
        }
    }

    // Public method to close inventory from other scripts if needed
    public void CloseInventory()
    {
        if (isInventoryOpen)
        {
            ToggleInventory();
        }
    }

    // Public method to check if inventory is open from other scripts
    public bool IsInventoryOpen()
    {
        return isInventoryOpen;
    }
}
