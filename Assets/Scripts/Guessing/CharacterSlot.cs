using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CharacterSlot : MonoBehaviour, IPointerClickHandler
{
    public Image characterImage;
    public TextMeshProUGUI characterName;
    public GameObject selectedPanel;
    public System.Action<CharacterSlot> OnSlotClicked;

    private bool isSelected = false;

    private void Awake()
    {
        if (selectedPanel != null)
            selectedPanel.SetActive(false);
    }

    public void SetCharacterInfo(string name, Sprite portrait)
    {
        characterName.text = name;
        characterImage.sprite = portrait;
    }

    public void ShowSelectedPanel(bool show)
    {
        if (selectedPanel != null)
        {
            selectedPanel.SetActive(show);
            isSelected = show;
        }
    }

    public bool IsSelected()
    {
        return isSelected;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnSlotClicked?.Invoke(this);
    }
}