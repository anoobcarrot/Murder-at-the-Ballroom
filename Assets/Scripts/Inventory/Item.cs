using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;
    public string itemDescription;
    public Sprite itemIcon;
    public int maxStackSize = 1;

    public Item(string name, string description, Sprite icon, int stackSize = 1)
    {
        itemName = name;
        itemDescription = description;
        itemIcon = icon;
        maxStackSize = stackSize;
    }
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemScriptableObject : ScriptableObject
{
    public Item item;
}
