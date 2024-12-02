using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement; // Add this for scene management

public class CharacterGuessingPanel : MonoBehaviour
{
    [System.Serializable]
    public class CharacterData
    {
        public string name;
        public Sprite portrait;
    }

    [Header("Scene Names")]
    public string winSceneName = "WinScene"; // Name of the win scene
    public string loseSceneName = "LoseScene"; // Name of the lose scene

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

        // If clicking the same slot that's already selected, deselect it
        if (currentlySelectedSlot == clickedSlot)
        {
            clickedSlot.ShowSelectedPanel(false);
            currentlySelectedSlot = null;
            if (confirmButton != null)
            {
                confirmButton.gameObject.SetActive(false);
            }
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
    }

    public void ConfirmGuess()
    {
        if (currentlySelectedSlot == null || remainingGuesses <= 0)
        {
            return;
        }

        remainingGuesses--;
        UpdateGuessesText();

        int guessIndex = characterSlots.IndexOf(currentlySelectedSlot);

        if (guessIndex == killerIndex)
        {
            // Transition to Win Scene
            SceneManager.LoadScene(winSceneName);
        }
        else if (remainingGuesses <= 0)
        {
            // Transition to Lose Scene
            SceneManager.LoadScene(loseSceneName);
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

    // Removed GameOver method as scene transitions replace its functionality

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}