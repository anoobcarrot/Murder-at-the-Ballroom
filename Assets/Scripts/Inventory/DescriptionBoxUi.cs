using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DescriptionBoxUI : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public Button addToInventoryButton;

    private Action onAddToInventory;

    public void SetupDescription(Item item, Action onAddToInventoryCallback)
    {
        Debug.Log("Setting up description for item: " + item.itemName);

        if (itemNameText != null)
        {
            itemNameText.text = item.itemName;
            Debug.Log("Item name set: " + item.itemName);
        }
        else
        {
            Debug.LogError("itemNameText is null");
        }

        if (itemDescriptionText != null)
        {
            itemDescriptionText.text = item.itemDescription;
            Debug.Log("Item description set: " + item.itemDescription);
        }
        else
        {
            Debug.LogError("itemDescriptionText is null");
        }

        onAddToInventory = onAddToInventoryCallback;

        if (addToInventoryButton != null)
        {
            addToInventoryButton.onClick.AddListener(OnAddToInventoryClicked);
            Debug.Log("Add to inventory button listener added");
        }
        else
        {
            Debug.LogError("addToInventoryButton is null");
        }
    }

    private void OnAddToInventoryClicked()
    {
        Debug.Log("Add to inventory button clicked");
        onAddToInventory?.Invoke();
    }

    private void OnDestroy()
    {
        if (addToInventoryButton != null)
            addToInventoryButton.onClick.RemoveListener(OnAddToInventoryClicked);
    }
}