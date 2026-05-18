using System.Collections;
using TMPro;
using UnityEngine;

public class EndingSelection : MonoBehaviour
{
    public GameObject interactText;
    public GameObject EndingSelectionGameObject;
    private bool playerNear = false;
    private bool isOpen = false;


    private Vector3 lockedCameraPosition;
    private Quaternion lockedCameraRotation;

    void Start()
    {
        interactText.SetActive(false);
    }

    void Update()
    {
        if (PauseMenu.IsPaused) return; // ignore all input while paused

        if (playerNear && Input.GetKeyDown(KeyCode.E))
        {
            if (!isOpen)
                OpenUI();
        }
        if (playerNear && Input.GetKeyDown(KeyCode.Escape))
        {
            if (isOpen)
                CloseUI();
        }
    }

    void OpenUI()
    {

        PauseMenu.IsInteracting = true;
        isOpen = true;
        interactText.SetActive(false);
        EndingSelectionGameObject.SetActive(true);


        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;

        if (!isOpen) return;

        // Keep camera locked
        transform.position = lockedCameraPosition;
        transform.rotation = lockedCameraRotation;
    }
    public void CloseUI()
    {

        isOpen = false;

        interactText.SetActive(false);
        EndingSelectionGameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;

        StartCoroutine(ResetInteracting()); // delayed by one frame
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;

            if (!isOpen)
                interactText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            interactText.SetActive(false);

            if (isOpen)
                CloseUI();
        }
    }

    IEnumerator ResetInteracting()
    {
        yield return null; // wait one frame — pause menu misses it
        PauseMenu.IsInteracting = false;
    }
}
