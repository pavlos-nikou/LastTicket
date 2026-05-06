using System.Collections;
using UnityEngine;

public class PaperInteract : MonoBehaviour
{
    public GameObject interactText;
    public GameObject PaperText;

    private bool playerNear = false;
    private bool isOpen = false;

    void Start()
    {
        interactText.SetActive(false);
        PaperText.SetActive(false);

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
        GameTracker.Instance.TrackInteraction(gameObject.name); // uses the GameObject's name
        GameTracker.Instance.DiscoverClue(gameObject.name);     // counts as finding a clue

        GameTracker.Instance.SendMessageToStory("PaperClueFound");

        isOpen = true;
        interactText.SetActive(false);
        PaperText.SetActive(true);
        // PauseGame();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void CloseUI()
    {

        isOpen = false;

        interactText.SetActive(false);
        PaperText.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;

        StartCoroutine(ResetInteracting());
    }

    IEnumerator ResetInteracting()
    {
        yield return null; // wait one frame — pause menu misses it
        PauseMenu.IsInteracting = false;
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
