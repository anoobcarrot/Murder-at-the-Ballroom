using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

// Update the ItemInUICanvas script to be simpler
public class ItemInUICanvas : MonoBehaviour, IPointerClickHandler
{
    public Image slotImage;
    public Image itemImage;
    public Image selectedPanel;
    public TextMeshProUGUI itemLabel;

    private Item currentItem;
    private int currentQuantity;

    private void Awake()
    {
        itemImage.gameObject.SetActive(false);
        itemLabel.gameObject.SetActive(false);
        selectedPanel.gameObject.SetActive(false);
    }

    public void UpdateItem(Item item, int quantity)
    {
        currentItem = item;
        currentQuantity = quantity;

        if (item != null)
        {
            // First enable the GameObjects and components
            if (itemImage != null)
            {
                itemImage.gameObject.SetActive(true);
                itemImage.enabled = true;
                itemImage.sprite = item.itemIcon;
            }

            if (itemLabel != null)
            {
                itemLabel.gameObject.SetActive(true);
                itemLabel.enabled = true;
                itemLabel.text = $"{item.itemName} ({quantity})";
            }

            // Disable the slot image last
            if (slotImage != null)
            {
                slotImage.enabled = false;
            }

            // Force a UI update
            Canvas.ForceUpdateCanvases();

            // Debug output to check states
            Debug.Log($"Item Image Active: {itemImage.gameObject.activeSelf}, Enabled: {itemImage.enabled}");
            Debug.Log($"Item Label Active: {itemLabel.gameObject.activeSelf}, Enabled: {itemLabel.enabled}");
        }
        else
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        currentItem = null;
        currentQuantity = 0;
        itemImage.gameObject.SetActive(false);
        itemLabel.gameObject.SetActive(false);
        slotImage.enabled = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && currentItem != null)
        {
            ItemDescriptionDisplay.Instance.DisplayItemInfo(currentItem);
            selectedPanel.gameObject.SetActive(true);
        }
    }
}

