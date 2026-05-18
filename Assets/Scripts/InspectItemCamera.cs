using System.Collections;
using UnityEngine;

public class InspectItemCamera : MonoBehaviour
{
    [Header("Spawn")]
    public GameObject itemPrefab;
    public float spawnDistance = 2f;

    [Header("Rotation")]
    public float rotationSpeed = 5f;

    [Header("References")]
    public MonoBehaviour cameraLookScript; // Drag your FPS look script here

    private GameObject spawnedItem;

    private Vector3 lockedCameraPosition;
    private Quaternion lockedCameraRotation;

    public bool isInspecting = false;

    void LateUpdate()
    {
        if (!isInspecting) return;

        // Keep camera locked
        transform.position = lockedCameraPosition;
        transform.rotation = lockedCameraRotation;

        // Rotate item with mouse
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        if (spawnedItem != null)
        {
            spawnedItem.transform.Rotate(Vector3.up, -mouseX * rotationSpeed, Space.World);
            spawnedItem.transform.Rotate(Vector3.right, mouseY * rotationSpeed, Space.World);
        }
    }
    public void StartInspection()
    {
        PauseMenu.IsInteracting = true;


        if (isInspecting) return;

        isInspecting = true;

        // Save camera position/rotation
        lockedCameraPosition = transform.position;
        lockedCameraRotation = transform.rotation;

        // Disable camera movement script
        if (cameraLookScript != null)
            cameraLookScript.enabled = false;

        // Spawn item
        Vector3 spawnPos = transform.position + transform.forward * spawnDistance;

        spawnedItem = Instantiate(itemPrefab, spawnPos, Quaternion.identity);

        spawnedItem.transform.LookAt(transform);
    }

    public void StopInspection()
    {
        if (!isInspecting) return;

        isInspecting = false;

        // Re-enable camera movement
        if (cameraLookScript != null)
            cameraLookScript.enabled = true;

        // Remove item
        if (spawnedItem != null)
        {
            Destroy(spawnedItem);
        }
        GameTracker.Instance.SendMessageToStory("WeaponPickup");
        StartCoroutine(ResetInteracting());
    }

    IEnumerator ResetInteracting()
    {
        yield return null; // wait one frame � pause menu misses it
        PauseMenu.IsInteracting = false;
    }
}