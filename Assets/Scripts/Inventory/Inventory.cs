using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<ItemSlot> items = new List<ItemSlot>();
    public int inventorySize = 20;

    public delegate void OnInventoryChanged();
    public OnInventoryChanged onInventoryChanged;

    public bool AddItem(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].item.itemName == item.itemName && items[i].quantity < items[i].item.maxStackSize)
            {
                items[i].quantity++;
                OnInventoryChangedSafe();
                return true;
            }
        }

        if (items.Count < inventorySize)
        {
            items.Add(new ItemSlot(item, 1));
            OnInventoryChangedSafe();
            return true;
        }

        Debug.Log("Inventory is full!");
        return false;
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
                    items.RemoveAt(i);
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