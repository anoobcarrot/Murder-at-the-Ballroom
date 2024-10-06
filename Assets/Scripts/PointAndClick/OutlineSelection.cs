using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class OutlineSelection : MonoBehaviour
{
    private Transform highlight;
    private RaycastHit raycastHit;

    public Inventory playerInventory;
    public GameObject descriptionBoxPrefab;
    public Canvas uiCanvas; // Reference to the UI Canvas
    private GameObject currentDescriptionBox;

    void Update()
    {
        HandleHighlight();
        HandleSelection();
    }

    void HandleHighlight()
    {
        if (highlight != null)
        {
            highlight.gameObject.GetComponent<Outline>().enabled = false;
            highlight = null;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit))
        {
            highlight = raycastHit.transform;
            if (highlight.CompareTag("Interactable"))
            {
                Outline outline = highlight.gameObject.GetComponent<Outline>();
                if (outline == null)
                {
                    outline = highlight.gameObject.AddComponent<Outline>();
                    outline.OutlineColor = Color.blue;
                    outline.OutlineWidth = 2.0f;
                }
                outline.enabled = true;
            }
            else
            {
                highlight = null;
            }
        }
    }

    void HandleSelection()
    {
        if (Input.GetMouseButtonDown(0) && highlight != null)
        {
            Debug.Log("Item clicked: " + highlight.gameObject.name);
            ShowItemDescription(highlight.gameObject);
        }
    }

    private void ShowItemDescription(GameObject obj)
    {
        ItemPickup itemPickup = obj.GetComponent<ItemPickup>();
        if (itemPickup != null && descriptionBoxPrefab != null && uiCanvas != null)
        {
            Debug.Log("Showing description for item: " + itemPickup.itemScriptableObject.item.itemName);
            if (currentDescriptionBox != null)
            {
                Destroy(currentDescriptionBox);
            }

            // Instantiate the description box as a child of the canvas
            currentDescriptionBox = Instantiate(descriptionBoxPrefab, uiCanvas.transform);
            Debug.Log("Description box instantiated: " + (currentDescriptionBox != null));
            DescriptionBoxUI descriptionBoxUI = currentDescriptionBox.GetComponent<DescriptionBoxUI>();
            if (descriptionBoxUI != null)
            {
                descriptionBoxUI.SetupDescription(itemPickup.itemScriptableObject.item, () => TryPickUpItem(obj));
                Debug.Log("Description box setup complete");
            }
            else
            {
                Debug.LogError("DescriptionBoxUI component not found on descriptionBoxPrefab");
            }
        }
        else
        {
            Debug.LogError("ItemPickup component not found, descriptionBoxPrefab not assigned, or uiCanvas not assigned");
        }
    }

    private void TryPickUpItem(GameObject obj)
    {
        ItemPickup itemPickup = obj.GetComponent<ItemPickup>();
        if (itemPickup != null && playerInventory != null)
        {
            Item item = itemPickup.itemScriptableObject.item;
            if (playerInventory.AddItem(item))
            {
                Destroy(obj);
                if (currentDescriptionBox != null)
                {
                    Destroy(currentDescriptionBox);
                    currentDescriptionBox = null;
                }
            }
            else
            {
                Debug.Log("Inventory is full!");
            }
        }
    }
}