using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ClickableCamera : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public GameObject cameraIcon;
    public float iconOffset = 1f; // Adjust this to change how far above the object the icon appears

    private void Start()
    {
        if (cameraIcon != null)
        {
            // Position the icon above the clickable object
            cameraIcon.transform.position = transform.position + Vector3.up * iconOffset;

            // Make the icon face the camera
            cameraIcon.transform.LookAt(2 * cameraIcon.transform.position - Camera.main.transform.position);
        }
        else
        {
            Debug.LogWarning("Camera icon not assigned to ClickableCamera on " + gameObject.name);
        }
    }

    public void ActivateCamera()
    {
        if (virtualCamera != null)
        {
            virtualCamera.Priority = 20; // Set a higher priority to make this camera active

            // Set all other virtual cameras to a lower priority
            CinemachineVirtualCamera[] allCameras = FindObjectsOfType<CinemachineVirtualCamera>();
            foreach (var cam in allCameras)
            {
                if (cam != virtualCamera)
                {
                    cam.Priority = 10;
                }
            }

            // Hide all camera icons and show only the active one
            ClickableCamera[] allClickableCameras = FindObjectsOfType<ClickableCamera>();
            foreach (var clickableCam in allClickableCameras)
            {
                if (clickableCam.cameraIcon != null)
                {
                    clickableCam.cameraIcon.SetActive(clickableCam == this);
                }
            }
        }
        else
        {
            Debug.LogError("Virtual Camera not assigned to ClickableCamera on " + gameObject.name);
        }
    }
}