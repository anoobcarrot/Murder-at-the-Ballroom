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
    public GameObject hoverUIPrefab;
    private GameObject currentHoverUI;


    void Update()
    {
        HandleHighlightAndHover();
        HandleSelection();
    }

    void HandleHighlightAndHover()
    {
        if (highlight != null)
        {
            highlight.gameObject.GetComponent<Outline>().enabled = false;
            highlight = null;
            HideHoverUI();
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
                ShowHoverUI(raycastHit.point);
            }
            else
            {
                highlight = null;
                HideHoverUI();
            }
        }
        else
        {
            HideHoverUI();
        }
    }

    void HandleSelection()
    {
        if (Input.GetMouseButtonDown(0) && highlight != null)
        {
            ClickableCamera clickableCamera = highlight.GetComponent<ClickableCamera>();
            if (clickableCamera != null)
            {
                Debug.Log("Activating camera: " + highlight.gameObject.name);
                clickableCamera.ActivateCamera();
            }
            else
            {
                ItemPickup itemPickup = highlight.GetComponent<ItemPickup>();
                if (itemPickup != null)
                {
                    Debug.Log("Showing item description: " + highlight.gameObject.name);
                    ShowItemDescription(highlight.gameObject);
                }
                else
                {
                    Debug.LogWarning("Clicked object is neither a camera nor an item: " + highlight.gameObject.name);
                }
            }
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

            currentDescriptionBox = Instantiate(descriptionBoxPrefab, uiCanvas.transform);
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

    private void ShowHoverUI(Vector3 worldPosition)
    {
        if (currentHoverUI == null && hoverUIPrefab != null && uiCanvas != null)
        {
            currentHoverUI = Instantiate(hoverUIPrefab, uiCanvas.transform);
            HoverUI hoverUI = currentHoverUI.GetComponent<HoverUI>();
            if (hoverUI != null)
            {
                hoverUI.Setup(worldPosition);
            }
        }
        else if (currentHoverUI != null)
        {
            HoverUI hoverUI = currentHoverUI.GetComponent<HoverUI>();
            if (hoverUI != null)
            {
                hoverUI.UpdatePosition(worldPosition);
            }
        }
    }

    private void HideHoverUI()
    {
        if (currentHoverUI != null)
        {
            Destroy(currentHoverUI);
            currentHoverUI = null;
        }
    }
}