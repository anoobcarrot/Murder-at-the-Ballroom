using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<ItemSlot> items = new List<ItemSlot>();
    public int inventorySize = 20;
    public delegate void OnInventoryChanged();
    public OnInventoryChanged onInventoryChanged;
    [SerializeField] private ItemInUICanvas itemInUICanvasPrefab;
    [SerializeField] private GridLayoutGroup inventoryGrid;
    private List<ItemInUICanvas> uiSlots = new List<ItemInUICanvas>();

    private void Start()
    {
        InitializeEmptySlots();
    }

    private void InitializeEmptySlots()
    {
        // Create all inventory slots at start
        for (int i = 0; i < inventorySize; i++)
        {
            ItemInUICanvas itemUI = Instantiate(itemInUICanvasPrefab, inventoryGrid.transform);
            uiSlots.Add(itemUI);
        }
    }

    public bool AddItem(Item item)
    {
        // Try to stack with existing items
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].item.itemName == item.itemName && items[i].quantity < items[i].item.maxStackSize)
            {
                items[i].quantity++;
                UpdateUISlot(i);
                OnInventoryChangedSafe();
                return true;
            }
        }

        // Add to new slot if inventory isn't full
        if (items.Count < inventorySize)
        {
            int newItemIndex = items.Count;
            items.Add(new ItemSlot(item, 1));

            UpdateUISlot(newItemIndex);
            OnInventoryChangedSafe();
            return true;
        }

        Debug.Log("Inventory is full!");
        return false;
    }

    private void UpdateUISlot(int index)
    {
        if (index < uiSlots.Count)
        {
            ItemInUICanvas itemUI = uiSlots[index];
            ItemSlot slot = items[index];
            itemUI.UpdateItem(slot.item, slot.quantity);
        }
    }

    public void RemoveItem(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].item.itemName == item.itemName)
            {
                items[i].quantity--;
                if (items[i].quantity <= 0)
                {
                    uiSlots[i].ClearSlot();
                    items.RemoveAt(i);

                    // Shift all remaining items one slot to the left
                    for (int j = i; j < items.Count; j++)
                    {
                        UpdateUISlot(j);
                    }

                    // Clear the now-empty last slot
                    if (items.Count < uiSlots.Count)
                    {
                        uiSlots[items.Count].ClearSlot();
                    }
                }
                else
                {
                    UpdateUISlot(i);
                }
                OnInventoryChangedSafe();
                return;
            }
        }
    }

    private void OnInventoryChangedSafe()
    {
        if (onInventoryChanged != null)
        {
            onInventoryChanged.Invoke();
        }
        else
        {
            Debug.LogWarning("Inventory: onInventoryChanged event is null.");
        }
    }
}

[System.Serializable]
public class ItemSlot
{
    public Item item;
    public int quantity;

    public ItemSlot(Item item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}
