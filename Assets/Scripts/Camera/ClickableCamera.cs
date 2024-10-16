using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ClickableCamera : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;

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
        }
        else
        {
            Debug.LogError("Virtual Camera not assigned to ClickableCamera on " + gameObject.name);
        }
    }
}