using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraToggle : MonoBehaviour
{
    public Transform cameraTransform;
    public float panSpeed = 2.0f;
    public float zoomSpeed = 5.0f; // New parameter for zoom speed
    public float minZoomDistance = 2.0f; // Minimum zoom distance from the original position
    public float maxZoomDistance = 10.0f; // Maximum zoom distance from the original position

    private Vector3 originalCameraPosition;
    private bool isPanning = false;
    private Vector3 panOrigin;

    private void Start()
    {
        if (cameraTransform == null)
        {
            Debug.LogError("Camera transform is not assigned. Please assign a camera transform in the inspector.");
            enabled = false;
        }

        // Initialize the original camera position
        originalCameraPosition = cameraTransform.position;
    }

    private void Update()
    {
        // Check for mouse input to initiate camera panning
        if (Input.GetMouseButtonDown(0))
        {
            isPanning = true;
            panOrigin = Input.mousePosition;
        }

        // End camera panning when the right mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            isPanning = false;
        }

        // Pan the camera based on mouse movement
        if (isPanning)
        {
            Vector3 mouseDelta = (Input.mousePosition - panOrigin) * panSpeed * Time.deltaTime;
            Vector3 newPosition = cameraTransform.position - new Vector3(mouseDelta.x, 0, mouseDelta.y);
            cameraTransform.position = newPosition;
            panOrigin = Input.mousePosition;
        }

        // Zoom the camera using the mouse scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 zoomVector = cameraTransform.forward * scroll * zoomSpeed * Time.deltaTime;
        Vector3 newZoomPosition = cameraTransform.position + zoomVector;

        // Clamp the zoom distance to the specified range
        newZoomPosition.y = Mathf.Clamp(newZoomPosition.y, minZoomDistance, maxZoomDistance);

        // Update the camera position after zooming
        cameraTransform.position = newZoomPosition;
    }

    // Reset camera position to original value
    public void ResetCamera()
    {
        cameraTransform.position = originalCameraPosition;
    }
}

