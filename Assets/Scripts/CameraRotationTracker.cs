using UnityEngine;

public class CameraRotationTracker : MonoBehaviour
{
    private Transform cameraToTrack; // Reference to the camera's transform

    // Update is called once per frame
    void Update()
    {
        // Check if cameraToTrack is assigned
        if (cameraToTrack != null)
        {
            // Set the rotation of the object to match the rotation of the camera
            transform.rotation = cameraToTrack.rotation;
        }
        else
        {
            // Attempt to find the camera in the scene
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                // Assign the camera's transform to cameraToTrack
                cameraToTrack = mainCamera.transform;
            }
            else
            {
                // Log a warning if the camera is not found
                Debug.LogWarning("Main camera not found!");
            }
        }
    }
}

