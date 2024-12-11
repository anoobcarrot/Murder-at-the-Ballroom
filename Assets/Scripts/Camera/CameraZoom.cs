using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;


public class CameraZoom : MonoBehaviour
{
    [SerializeField] private float zoomSpeed = 2f; // Slower zoom speed
    [SerializeField] private float zoomFactor = 2f; // Distance multiplier for zooming in
    [SerializeField] private LayerMask zoomLayer;

    [SerializeField] private KeyCode startKey = KeyCode.Mouse0;

    private Camera cam;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    [SerializeField] private bool isZoomedIn = false;
    [SerializeField] private bool zoomEnabled = true; // New flag to control zoom functionality
    [SerializeField] public bool isZooming = false; // New flag to indicate if zoom is in progress

    public bool isZoomedInMirror = false;
    public bool IsZoomedIn { get { return isZoomedIn; } }

    void Start()
    {
        cam = GetComponent<Camera>();
        originalPosition = cam.transform.position;
        originalRotation = cam.transform.rotation;

        // Ensure all buttons in the scene are registered for click events
        RegisterButtonListeners();
    }

    void Update()
    {
        // Continuously check if any button is pressed and perform zoom out
        if (!zoomEnabled || isZooming) return; // Skip zoom logic if zoom is disabled or zoom is in progress

        if (Input.GetKeyDown(startKey) && !isZoomedIn)
        {
            ZoomInToObject();
        }
        else if (Input.GetKeyDown(startKey) && isZoomedIn)
        {
            // ZoomOut();
        }

        // Continuously check for button press to zoom out
        if (Input.GetMouseButtonDown(0)) // Assuming Mouse0 (Left-click) triggers zoom out
        {
            CheckButtonPress();
        }
    }

    public void ZoomInToObject()
    {
        if (isZoomedIn || !zoomEnabled || isZooming) return;

        // Perform a raycast to see what the player clicked on
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, zoomLayer))
        {
            Vector3 direction = hit.point - cam.transform.position;
            float distance = direction.magnitude;
            direction.Normalize();

            RaycastHit[] hits = Physics.RaycastAll(cam.transform.position, direction, distance, ~zoomLayer);
            bool isHit = false;

            foreach (RaycastHit obstacleHit in hits)
            {
                if (obstacleHit.transform != hit.transform)
                {
                    isHit = true;
                    break;
                }
            }

            if (!isHit)
            {
                StartCoroutine(ZoomToPosition(hit.transform.position));
                isZoomedIn = true;
            }
        }
    }

    public void ZoomOut()
    {
        if (!isZoomedIn || !zoomEnabled || isZooming) return;
        StartCoroutine(ZoomOutCoroutine());
    }

    private IEnumerator ZoomToPosition(Vector3 targetPosition)
    {
        isZooming = true;
        float progress = 0f;
        Vector3 startPosition = cam.transform.position;
        Quaternion startRotation = cam.transform.rotation;
        Vector3 direction = (targetPosition - cam.transform.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(direction, cam.transform.up);

        while (progress < 1f)
        {
            progress += Time.deltaTime * zoomSpeed;
            float smoothProgress = Mathf.SmoothStep(0f, 1f, progress);
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition - direction * zoomFactor, smoothProgress);
            cam.transform.position = newPosition;
            cam.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, smoothProgress);

            yield return null;
        }

        cam.transform.position = targetPosition - direction * zoomFactor;
        cam.transform.rotation = targetRotation;

        isZoomedIn = true;
        isZooming = false;
    }

    private IEnumerator ZoomOutCoroutine()
    {
        isZooming = true;
        float progress = 0f;
        Vector3 startPosition = cam.transform.position;
        Quaternion startRotation = cam.transform.rotation;

        while (progress < 1f)
        {
            progress += Time.deltaTime * zoomSpeed;
            float smoothProgress = Mathf.SmoothStep(0f, 1f, progress);

            Vector3 newPosition = Vector3.Lerp(startPosition, originalPosition, smoothProgress);
            Quaternion newRotation = Quaternion.Lerp(startRotation, originalRotation, smoothProgress);

            cam.transform.position = newPosition;
            cam.transform.rotation = newRotation;

            yield return null;
        }

        cam.transform.position = originalPosition;
        cam.transform.rotation = originalRotation;

        isZoomedIn = false;
        isZooming = false;
    }

    // Detect button presses continuously
    private void CheckButtonPress()
    {
        // Check if the clicked object is a UI Button
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponent<Button>())
            {
                // If a button is clicked, perform zoom out
                ZoomOut();
                break; // Exit loop after handling first button press
            }
        }
    }

    // Register Button Click listeners dynamically
    private void RegisterButtonListeners()
    {
        // Register all buttons in the scene to trigger the ZoomOut() when clicked
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(OnButtonPressed);
        }
    }

    // Method for button click handler
    public void OnButtonPressed()
    {
        // Zoom out when button is clicked
        ZoomOut();
    }

    public void EnableZoom()
    {
        zoomEnabled = true;
    }

    public void DisableZoom()
    {
        zoomEnabled = false;
    }
}
