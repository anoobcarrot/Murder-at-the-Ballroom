using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public GameObject itemSlotPrefab;
    public Transform itemsParent;

    private Inventory inventory;

    void Start()
    {
        inventory = GetComponent<Inventory>();
        if (inventory == null)
        {
            Debug.LogError("InventoryUI: No Inventory component found on this GameObject.");
            return;
        }

        inventory.onInventoryChanged += UpdateUI;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (itemsParent == null)
        {
            Debug.LogError("InventoryUI: Items Parent is not assigned.");
            return;
        }

        // Clear existing slots
        foreach (Transform child in itemsParent)
        {
            Destroy(child.gameObject);
        }

        // Create new item slots
        foreach (ItemSlot slot in inventory.items)
        {
            if (itemSlotPrefab == null)
            {
                Debug.LogError("InventoryUI: Item Slot Prefab is not assigned.");
                return;
            }

            GameObject slotGO = Instantiate(itemSlotPrefab, itemsParent);

            Image iconImage = slotGO.transform.Find("Icon")?.GetComponent<Image>();
            TextMeshProUGUI quantityText = slotGO.transform.Find("Quantity")?.GetComponent<TextMeshProUGUI>();

            if (iconImage == null)
            {
                Debug.LogError("InventoryUI: Icon Image component not found in item slot prefab.");
            }
            else
            {
                iconImage.sprite = slot.item.itemIcon;
            }

            if (quantityText == null)
            {
                Debug.LogError("InventoryUI: TextMeshProUGUI Quantity component not found in item slot prefab.");
            }
            else
            {
                quantityText.text = slot.quantity > 1 ? slot.quantity.ToString() : "";
            }
        }
    }
}