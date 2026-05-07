using UnityEngine;

public class MicrowaveDoorOpen : MonoBehaviour
{
    public Transform door;

    public float openAngle = 90f;
    public float speed = 2f;

    private bool isOpen = false;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = door.localRotation;

        openRotation =
            closedRotation * Quaternion.Euler(0, openAngle, 0);
    }

    void Update()
    {
        Quaternion targetRotation =
            isOpen ? openRotation : closedRotation;

        door.localRotation = Quaternion.Lerp(
            door.localRotation,
            targetRotation,
            Time.deltaTime * speed
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOpen = true;
        }
    }
}