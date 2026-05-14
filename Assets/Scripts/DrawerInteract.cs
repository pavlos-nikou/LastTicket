using System.Collections;
using UnityEngine;

public class DrawerInteract : MonoBehaviour
{
    public GameObject interactText;
    public GameObject Puzzle;

    public GameObject playermovement;
    public GameObject playerCamera;

    public Animator drawerAnimator;
    public GameObject ram;
    public AudioSource drawerOpenSound;

    private bool unlocked = false;

    private bool playerNear = false;
    private bool isOpen = false;

    void Start()
    {
        interactText.SetActive(false);
        Puzzle.SetActive(false);

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

        // GameTracker.Instance.SendMessageToStory("PaperClueFound");

        isOpen = true;
        interactText.SetActive(false);
        Puzzle.SetActive(true);
        // PauseGame();

        // Cursor.lockState = CursorLockMode.None;
        // Cursor.visible = true;
        // Time.timeScale = 0f;


        playermovement.GetComponent<PlayerMovement>().enabled = false;
        playerCamera.GetComponent<PlayerCam>().enabled = false;
    }

    public void CloseUI()
    {

        isOpen = false;

        interactText.SetActive(false);
        Puzzle.SetActive(false);

        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
        // Time.timeScale = 1f;

        playermovement.GetComponent<PlayerMovement>().enabled = true;
        playerCamera.GetComponent<PlayerCam>().enabled = true;

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

    public void UnlockDrawer()
    {
        if (unlocked) return;

        unlocked = true;

        // open animation
        if (drawerAnimator != null)
            drawerAnimator.SetTrigger("Open");

        // reveal item
        if (ram != null)
            ram.SetActive(true);

        if (drawerOpenSound != null)
            drawerOpenSound.Play();

        CloseUI();
    }
}
