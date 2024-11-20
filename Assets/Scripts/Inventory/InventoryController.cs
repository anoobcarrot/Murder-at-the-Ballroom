using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject guessingMenuPanel;
    [SerializeField] private KeyCode toggleInventoryKey = KeyCode.I;
    [SerializeField] private GameObject clickableInventoryObject;
    [SerializeField] private GameObject clickableGuessingMenuObject; // New clickable object for guessing menu

    private bool isInventoryOpen = false;
    private bool isGuessingMenuOpen = false;
    private Camera mainCamera;

    private void Start()
    {
        // Ensure panels start closed
        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);

        if (guessingMenuPanel != null)
            guessingMenuPanel.SetActive(false);

        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Check for inventory key input
        if (Input.GetKeyDown(toggleInventoryKey))
        {
            ToggleInventory();
        }

        // Check for mouse click
        if (Input.GetMouseButtonDown(0)) // Left mouse click
        {
            CheckClickableObjects();
        }
    }

    private void CheckClickableObjects()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // Check if we hit the inventory clickable object
            if (hit.collider.gameObject == clickableInventoryObject)
            {
                ToggleInventory();
            }

            // Check if we hit the guessing menu clickable object
            if (hit.collider.gameObject == clickableGuessingMenuObject)
            {
                ToggleGuessingMenu();
            }
        }
    }

    public void ToggleInventory()
    {
        if (inventoryPanel != null)
        {
            // Close guessing menu if open when opening inventory
            if (isGuessingMenuOpen)
                ToggleGuessingMenu();

            isInventoryOpen = !isInventoryOpen;
            inventoryPanel.SetActive(isInventoryOpen);
        }
        else
        {
            Debug.LogWarning("Inventory Panel reference is missing!");
        }
    }

    public void ToggleGuessingMenu()
    {
        if (guessingMenuPanel != null)
        {
            // Close inventory if open when opening guessing menu
            if (isInventoryOpen)
                ToggleInventory();

            isGuessingMenuOpen = !isGuessingMenuOpen;
            guessingMenuPanel.SetActive(isGuessingMenuOpen);
        }
        else
        {
            Debug.LogWarning("Guessing Menu Panel reference is missing!");
        }
    }

    public void CloseInventory()
    {
        if (isInventoryOpen)
        {
            ToggleInventory();
        }
    }

    public void CloseGuessingMenu()
    {
        if (isGuessingMenuOpen)
        {
            ToggleGuessingMenu();
        }
    }

    public bool IsInventoryOpen()
    {
        return isInventoryOpen;
    }

    public bool IsGuessingMenuOpen()
    {
        return isGuessingMenuOpen;
    }
}