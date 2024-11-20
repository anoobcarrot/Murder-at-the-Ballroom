using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class CharacterGuessingPanel : MonoBehaviour
{
    [System.Serializable]
    public class CharacterData
    {
        public string name;
        public Sprite portrait;
    }

    public CharacterSlot characterSlotPrefab;
    public Transform slotsParent;
    public List<CharacterData> charactersData = new List<CharacterData>();
    [SerializeField] private int killerIndex = 0;
    public int maxGuesses = 3;
    public TextMeshProUGUI guessesRemainingText;
    public TextMeshProUGUI resultText;
    public GameObject gameOverPanel;
    public Button confirmButton;

    private List<CharacterSlot> characterSlots = new List<CharacterSlot>();
    private int remainingGuesses;
    private CharacterSlot currentlySelectedSlot = null;

    void Start()
    {
        // Set up confirm button listener
        if (confirmButton != null)
        {
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(ConfirmGuess);
        }
        else
        {
            Debug.LogError("Confirm button not assigned in CharacterGuessingPanel!");
        }

        CreateCharacterSlots();
        InitializeGame();
    }

    void CreateCharacterSlots()
    {
        foreach (CharacterSlot slot in characterSlots)
        {
            if (slot != null)
                Destroy(slot.gameObject);
        }
        characterSlots.Clear();

        for (int i = 0; i < charactersData.Count; i++)
        {
            CharacterSlot newSlot = Instantiate(characterSlotPrefab, slotsParent);
            newSlot.SetCharacterInfo(charactersData[i].name, charactersData[i].portrait);
            newSlot.OnSlotClicked += HandleSlotClicked;
            characterSlots.Add(newSlot);
        }
    }

    void InitializeGame()
    {
        killerIndex = Mathf.Clamp(killerIndex, 0, charactersData.Count - 1);
        remainingGuesses = maxGuesses;
        UpdateGuessesText();

        gameOverPanel.SetActive(false);
        resultText.text = "";

        if (confirmButton != null)
        {
            confirmButton.gameObject.SetActive(false);
        }

        currentlySelectedSlot = null;

        foreach (var slot in characterSlots)
        {
            slot.ShowSelectedPanel(false);
        }
    }

    void HandleSlotClicked(CharacterSlot clickedSlot)
    {
        if (remainingGuesses <= 0) return;

        Debug.Log("Slot clicked"); // Debug log

        // If clicking the same slot that's already selected, deselect it
        if (currentlySelectedSlot == clickedSlot)
        {
            clickedSlot.ShowSelectedPanel(false);
            currentlySelectedSlot = null;
            if (confirmButton != null)
            {
                confirmButton.gameObject.SetActive(false);
            }
            Debug.Log("Deselected slot"); // Debug log
            return;
        }

        // If another slot was selected, deselect it
        if (currentlySelectedSlot != null)
        {
            currentlySelectedSlot.ShowSelectedPanel(false);
        }

        // Select the new slot
        currentlySelectedSlot = clickedSlot;
        clickedSlot.ShowSelectedPanel(true);
        if (confirmButton != null)
        {
            confirmButton.gameObject.SetActive(true);
        }
        Debug.Log("Selected new slot"); // Debug log
    }

    public void ConfirmGuess()
    {
        Debug.Log("ConfirmGuess called"); // Debug log

        if (currentlySelectedSlot == null || remainingGuesses <= 0)
        {
            Debug.Log("No slot selected or no guesses remaining");
            return;
        }

        remainingGuesses--;
        UpdateGuessesText();

        int guessIndex = characterSlots.IndexOf(currentlySelectedSlot);
        Debug.Log($"Guessed index: {guessIndex}, Killer index: {killerIndex}"); // Debug log

        if (guessIndex == killerIndex)
        {
            GameOver(true);
        }
        else if (remainingGuesses <= 0)
        {
            GameOver(false);
        }
        else
        {
            resultText.text = "Wrong guess! Try again.";
            currentlySelectedSlot.ShowSelectedPanel(false);
            currentlySelectedSlot = null;
            if (confirmButton != null)
            {
                confirmButton.gameObject.SetActive(false);
            }
        }
    }

    void UpdateGuessesText()
    {
        if (guessesRemainingText != null)
        {
            guessesRemainingText.text = $"Guesses Remaining: {remainingGuesses}";
        }
    }

    void GameOver(bool won)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (confirmButton != null)
        {
            confirmButton.gameObject.SetActive(false);
        }

        if (resultText != null)
        {
            resultText.text = won ?
                "Congratulations! You found the killer!" :
                $"Game Over! The killer was {charactersData[killerIndex].name}!";
        }

        // Show killer's selected panel at the end
        characterSlots[killerIndex].ShowSelectedPanel(true);
    }

    public void RestartGame()
    {
        InitializeGame();
    }
}