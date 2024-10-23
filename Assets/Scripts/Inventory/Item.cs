using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName = "Unamed Item.";
    public string itemDescription = "No Description.";
    public string itemClue = "No Clue.";
    public Sprite itemIcon = null;
    public int maxStackSize = 1;

    public Item(string name, string description, string clue, Sprite icon, int stackSize = 1)
    {
        itemName = name;
        itemDescription = description;
        itemClue = clue;
        itemIcon = icon;
        maxStackSize = stackSize;
    }
}