using UnityEngine;

public class MicrowaveDoorOpen : MonoBehaviour
{
    [Header("Door Settings")]
    public Transform door;

    [Tooltip("Negative opens opposite direction")]
    public float openAngle = 0f;

    public float speed = 2f;

    private bool isOpen = false;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        // Save the closed rotation
        closedRotation = door.localRotation;

        // Create the open rotation
        // Change axis here if needed
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
    }

    void Update()
    {
        // TEST KEY
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleDoor();
        }

        // Smoothly rotate door
        Quaternion targetRotation = isOpen ? openRotation : closedRotation;

        door.localRotation = Quaternion.Lerp(
            door.localRotation,
            targetRotation,
            Time.deltaTime * speed
        );
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
    }
}