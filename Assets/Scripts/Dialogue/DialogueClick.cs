using UnityEngine;
using DialogueEditor;

public class DialogueClick : MonoBehaviour
{
    [SerializeField] private NPCConversation Conversation;
    [SerializeField] private string dialogueKey = "DialogueTriggered"; // Public key for PlayerPrefs
    [SerializeField] private bool isDialoguePressed;

    private void Start()
    {
        // Check if the dialogue has already been triggered
        Debug.LogWarning("isDialoguePressed is currently [" + isDialoguePressed + "] BEFORE setting it.");
        Debug.LogWarning("PlayerPrefs isDialoguePressed is currently [" + PlayerPrefs.GetInt(dialogueKey, 0) + "] BEFORE setting it.");

        PlayerPrefs.SetInt(dialogueKey, 0);

        Debug.LogWarning("isDialoguePressed is currently [" + isDialoguePressed + "] AFTER setting it.");
        Debug.LogWarning("PlayerPrefs isDialoguePressed is currently [" + PlayerPrefs.GetInt(dialogueKey, 0) + "] AFTER setting it.");

        isDialoguePressed = PlayerPrefs.GetInt(dialogueKey, 0) == 1;
    }

    private void OnMouseOver()
    {
        Debug.Log("Mouse is hovering over" + gameObject.name);

        if (Input.GetMouseButtonDown(0) && !isDialoguePressed)
        {
            Debug.Log("You have pressed on" + gameObject.name);



            ConversationManager.Instance.StartConversation(Conversation);
            isDialoguePressed = true;

            // Save the state to PlayerPrefs so it won't trigger again
            PlayerPrefs.SetInt(dialogueKey, 1);
            PlayerPrefs.Save();
        }
    }
}

