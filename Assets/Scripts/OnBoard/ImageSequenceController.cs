using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ImageSequenceController : MonoBehaviour
{
    public Image[] imageSequence; // Array to hold all images in sequence
    public float transitionDuration = 1.0f; // Duration of fade transition
    public Image transitionOverlay; // Transition overlay image

    private CanvasGroup transitionCanvasGroup;
    private int currentImageIndex = 0;

    private void Start()
    {
        // Ensure only first image is visible initially
        SetImagesVisibility(false);
        imageSequence[0].gameObject.SetActive(true);

        // Setup transition overlay
        transitionCanvasGroup = transitionOverlay.GetComponent<CanvasGroup>();
        transitionCanvasGroup.alpha = 0f;
    }

    private void Update()
    {
        // Check for mouse click to advance sequence
        if (Input.GetMouseButtonDown(0))
        {
            AdvanceSequence();
        }
    }

    private void AdvanceSequence()
    {
        // Move to next image if available
        if (currentImageIndex < imageSequence.Length - 1)
        {
            StartCoroutine(TransitionToNextImage());
        }
    }

    private IEnumerator TransitionToNextImage()
    {
        // Fade to black
        while (transitionCanvasGroup.alpha < 1)
        {
            transitionCanvasGroup.alpha += Time.deltaTime / transitionDuration;
            yield return null;
        }

        // Hide current image and show next
        imageSequence[currentImageIndex].gameObject.SetActive(false);
        currentImageIndex++;
        imageSequence[currentImageIndex].gameObject.SetActive(true);

        // Fade back out
        while (transitionCanvasGroup.alpha > 0)
        {
            transitionCanvasGroup.alpha -= Time.deltaTime / transitionDuration;
            yield return null;
        }
    }

    private void SetImagesVisibility(bool visible)
    {
        // Set all images to same visibility state
        foreach (Image img in imageSequence)
        {
            img.gameObject.SetActive(visible);
        }
    }
}
