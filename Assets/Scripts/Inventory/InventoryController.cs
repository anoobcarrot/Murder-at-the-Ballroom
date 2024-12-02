using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject guessingMenuPanel;
    [SerializeField] private KeyCode toggleInventoryKey = KeyCode.I;
    [SerializeField] private GameObject clickableInventoryObject;
    [SerializeField] private GameObject clickableGuessingMenuObject; // New clickable object for guessing menu
    [SerializeField] private Canvas canvas;

    private bool isInventoryOpen = false;
    private bool isGuessingMenuOpen = false;
    private Camera mainCamera;
    private GraphicRaycaster graphicRaycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;

    private void Start()
    {
        // Get the required components
        graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
        eventSystem = EventSystem.current;

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
        // Step 1: Check for UI elements using GraphicRaycaster
        if (IsPointerOverUIElement(out GameObject clickedUIObject))
        {
            if (clickedUIObject == clickableInventoryObject)
            {
                ToggleInventory();
                return;
            }

            if (clickedUIObject == clickableGuessingMenuObject)
            {
                ToggleGuessingMenu();
                return;
            }
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject == clickableInventoryObject)
            {
                ToggleInventory();
            }

            if (hit.collider.gameObject == clickableGuessingMenuObject)
            {
                ToggleGuessingMenu();
            }
        }
    }

    private bool IsPointerOverUIElement(out GameObject clickedObject)
    {
        // Create a PointerEventData for the current mouse position
        pointerEventData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };

        // Raycast using the GraphicRaycaster
        var results = new System.Collections.Generic.List<RaycastResult>();
        graphicRaycaster.Raycast(pointerEventData, results);

        // Check if any of the results match our target UI elements
        foreach (var result in results)
        {
            if (result.gameObject == clickableInventoryObject || result.gameObject == clickableGuessingMenuObject)
            {
                clickedObject = result.gameObject;
                return true;
            }
        }

        clickedObject = null;
        return false;
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