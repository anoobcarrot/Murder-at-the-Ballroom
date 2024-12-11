using UnityEngine;
using UnityEngine.EventSystems;
using DialogueEditor;
using UnityEngine.UI;

public class DialogueClick : MonoBehaviour
{
    [SerializeField] private NPCConversation Conversation;
    [SerializeField] private Canvas dialogueCanvas; // Reference to your Dialogue Canvas

    private static bool isDialogueActive = false;
    private Camera mainCamera;

    // Store references to the scripts under the MainCamera to disable/re-enable
    private MonoBehaviour[] mainCameraScripts;

    private void Start()
    {
        // Get the main camera
        mainCamera = Camera.main;

        // Get all MonoBehaviour scripts attached to objects under MainCamera
        if (mainCamera != null)
        {
            mainCameraScripts = mainCamera.GetComponentsInChildren<MonoBehaviour>();
        }
    }

    private void OnMouseOver()
    {
        // Only process the interaction if dialogue is not active
        if (Input.GetMouseButtonDown(0) && !isDialogueActive)
        {
            Debug.Log("You have pressed on " + gameObject.name);

            // Start the conversation
            ConversationManager.Instance.StartConversation(Conversation);

            // Mark dialogue as active
            isDialogueActive = true;

            // Register callback for when the dialogue ends
            ConversationManager.OnConversationEnded += EndDialogue;

            // Disable interaction for all objects outside of Dialogue Canvas
            SetUIInteractable(false);

            // Disable all scripts under the MainCamera
            DisableMainCameraScripts(true);

            // Disable 3D/2D object interactions (colliders)
            DisableObjectInteractions(true);
        }
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        Debug.Log("Dialogue ended");

        // Enable interaction for all objects after dialogue ends
        SetUIInteractable(true);

        // Re-enable all scripts under the MainCamera
        DisableMainCameraScripts(false);

        // Enable 3D/2D object interactions (colliders)
        DisableObjectInteractions(false);

        // Unsubscribe to prevent memory leaks
        ConversationManager.OnConversationEnded -= EndDialogue;
    }

    private void Update()
    {
        if (isDialogueActive)
        {
            // Raycast from the mouse position to detect interactable elements
            RaycastInteractableObjects();
        }
    }

    private void RaycastInteractableObjects()
    {
        // Create a ray from the mouse position
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Disable interaction for everything not under the Dialogue Canvas
            if (!hit.transform.IsChildOf(dialogueCanvas.transform))
            {
                DisableInteractions(hit.collider.gameObject);
            }
        }

        // Raycast for UI elements using EventSystem
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var raycastResults = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        foreach (var result in raycastResults)
        {
            GameObject hitObject = result.gameObject;
            if (!hitObject.transform.IsChildOf(dialogueCanvas.transform))
            {
                DisableInteractions(hitObject);
            }
        }
    }

    private void DisableInteractions(GameObject obj)
    {
        // Disable interaction scripts (e.g., buttons, colliders)
        if (obj.GetComponent<Button>() != null)
        {
            obj.GetComponent<Button>().interactable = false; // Disable UI Button
        }

        // Disable other UI components (Toggle, Slider, etc.)
        if (obj.GetComponent<Toggle>() != null)
        {
            obj.GetComponent<Toggle>().interactable = false; // Disable UI Toggle
        }

        if (obj.GetComponent<Slider>() != null)
        {
            obj.GetComponent<Slider>().interactable = false; // Disable UI Slider
        }

        // Disable Image interaction
        if (obj.GetComponent<Image>() != null)
        {
            obj.GetComponent<Image>().raycastTarget = false; // Disable interaction for Image
        }

        // Disable RawImage interaction
        if (obj.GetComponent<RawImage>() != null)
        {
            obj.GetComponent<RawImage>().raycastTarget = false; // Disable interaction for RawImage
        }

        // Disable 3D/2D Collider interaction
        if (obj.GetComponent<Collider>() != null)
        {
            obj.GetComponent<Collider>().enabled = false; // Disable Collider (interaction)
        }

        if (obj.GetComponent<Collider2D>() != null)
        {
            obj.GetComponent<Collider2D>().enabled = false; // Disable 2D Collider (interaction)
        }
    }

    private void SetUIInteractable(bool isActive)
    {
        // Find all canvases and disable interactions for all except the Dialogue Canvas
        foreach (var canvas in FindObjectsOfType<Canvas>())
        {
            // Skip the Dialogue Canvas
            if (canvas != dialogueCanvas)
            {
                // Disable all interactable UI components in other canvases
                foreach (var uiElement in canvas.GetComponentsInChildren<MonoBehaviour>())
                {
                    if (uiElement is Button button)
                    {
                        button.interactable = isActive; // Enable/disable UI buttons
                    }
                    else if (uiElement is Toggle toggle)
                    {
                        toggle.interactable = isActive; // Enable/disable Toggle
                    }
                    else if (uiElement is Slider slider)
                    {
                        slider.interactable = isActive; // Enable/disable Slider
                    }
                    // Add Image and RawImage components
                    else if (uiElement is Image image)
                    {
                        image.raycastTarget = isActive; // Enable/disable interaction for Image
                    }
                    else if (uiElement is RawImage rawImage)
                    {
                        rawImage.raycastTarget = isActive; // Enable/disable interaction for RawImage
                    }
                }
            }
        }
    }

    private void DisableMainCameraScripts(bool shouldDisable)
    {
        // Disable or enable all MonoBehaviour scripts under MainCamera
        if (mainCameraScripts != null)
        {
            foreach (var script in mainCameraScripts)
            {
                script.enabled = !shouldDisable; // Disable all scripts if shouldDisable is true
            }
        }
    }

    private void DisableObjectInteractions(bool disable)
    {
        // Disable all colliders (3D and 2D) when dialogue is active
        Collider[] colliders = FindObjectsOfType<Collider>();
        foreach (var collider in colliders)
        {
            collider.enabled = !disable; // Disable colliders when dialogue is active
        }

        Collider2D[] colliders2D = FindObjectsOfType<Collider2D>();
        foreach (var collider in colliders2D)
        {
            collider.enabled = !disable; // Disable 2D colliders when dialogue is active
        }
    }
}

