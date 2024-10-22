using UnityEngine;
using DialogueEditor;

public class DialogueClick : MonoBehaviour
{
    [SerializeField] private NPCConversation Conversation;
    [SerializeField] private string dialogueKey = "DialogueTriggered"; // Public key for PlayerPrefs
    private bool isDialoguePressed;

    private void Start()
    {
        // Check if the dialogue has already been triggered
        isDialoguePressed = PlayerPrefs.GetInt(dialogueKey, 0) == 1;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !isDialoguePressed)
        {
            ConversationManager.Instance.StartConversation(Conversation);
            isDialoguePressed = true;

            // Save the state to PlayerPrefs so it won't trigger again
            PlayerPrefs.SetInt(dialogueKey, 1);
            PlayerPrefs.Save();
        }
    }
}

