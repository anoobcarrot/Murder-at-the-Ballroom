using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemDescriptionDisplay : MonoBehaviour
{
    public static ItemDescriptionDisplay Instance { get; private set; }

    [Header("UI Elements")]
    [SerializeField] private Image itemDescriptionImage;
    [SerializeField] private TMP_Text itemDescriptionNameText;
    [SerializeField] private TMP_Text itemClueText;

    private void Awake()
    {
        // Simple singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Hide description elements initially
        HideDescription();
    }

    public void DisplayItemInfo(Item item)
    {
        if (item == null)
        {
            HideDescription();
            return;
        }

        // Show and update the description elements
        itemDescriptionImage.gameObject.SetActive(true);
        itemDescriptionNameText.gameObject.SetActive(true);
        itemClueText.gameObject.SetActive(true);

        itemDescriptionImage.sprite = item.itemIcon;
        itemDescriptionNameText.text = item.itemName;
        itemClueText.text = item.itemClue;
    }

    public void HideDescription()
    {
        itemDescriptionImage.gameObject.SetActive(false);
        itemDescriptionNameText.gameObject.SetActive(false);
        itemClueText.gameObject.SetActive(false);
    }
}
