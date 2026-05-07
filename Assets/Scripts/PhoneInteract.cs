using UnityEngine;
using TMPro;
using System.Collections;


public class PhoneInteract : MonoBehaviour
{
    public GameObject interactText;
    public GameObject PhoneScreen;
    public GameObject HealthApp;

    private bool playerNear = false;
    private bool isOpen = false;

    void Start()
    {
        interactText.SetActive(false);
        PhoneScreen.SetActive(false);
        HealthApp.SetActive(false);
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

        PhoneScreen.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void CloseUI()
    {

        isOpen = false;

        PhoneScreen.SetActive(false);
        HealthApp.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;

        StartCoroutine(ResetInteracting()); // delayed by one frame
    }

    IEnumerator ResetInteracting()
    {
        yield return null; // wait one frame — pause menu misses it
        PauseMenu.IsInteracting = false;
    }

    public void OpenApp()
    {

        PhoneScreen.SetActive(false);
        HealthApp.SetActive(true);

    }

    // walk up to the computer text apears
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
}
