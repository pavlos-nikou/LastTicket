using UnityEngine;
using TMPro;
using System.Collections;


public class ComputerInteraction : MonoBehaviour
{
    public GameObject interactText;
    public GameObject signInCanvas;
    public GameObject desktopCanvas;

    private bool playerNear = false;
    private bool isOpen = false;
    private static bool isLoggedIn = false;

    public TMP_InputField passwordInput;
    public string correctPassword;
    public TMP_Text wrongPasswordText;
    [Header("Act 3")]
    public LampFlickerNoise[] lamps;
    public Light printerSpotlight;
    public AudioSource printerSound;
    public GameObject printerNote;        // the note GameObject that appears
    public float flickerDuration;


    void Start()
    {
        interactText.SetActive(false);
        signInCanvas.SetActive(false);
        desktopCanvas.SetActive(false);
        wrongPasswordText.gameObject.SetActive(false);

    }

    void Update()
    {
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
        GameTracker.Instance.TrackInteraction("Computer");

        isOpen = true;
        interactText.SetActive(false);

        if (isLoggedIn)
        {
            signInCanvas.SetActive(false);
            desktopCanvas.SetActive(true);
        }
        else
        {
            signInCanvas.SetActive(true);
            desktopCanvas.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void CloseUI()
    {
        isOpen = false;

        signInCanvas.SetActive(false);
        desktopCanvas.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;

    }

    public bool IsPasswordCorrect()
    {
        return passwordInput.text == correctPassword;
    }

    // show desktop after sign in
    public void Login()
    {
        if (IsPasswordCorrect())
        {
            GameTracker.Instance.TrackPasswordAttempt(true);

            isLoggedIn = true;
            wrongPasswordText.gameObject.SetActive(false);
            signInCanvas.SetActive(false);
            desktopCanvas.SetActive(true);
           
        }
        else
        {
            GameTracker.Instance.TrackPasswordAttempt(false);

            Debug.Log("Wrong password");
            passwordInput.text = "";
            wrongPasswordText.gameObject.SetActive(true);
            passwordInput.ActivateInputField();
        }
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
        if (isLoggedIn)
        {
            GameTracker.Instance.SendMessageToStory("VictimCompExit");
        }
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            interactText.SetActive(false);

            if (isOpen)
                CloseUI();
        }
    }

  
}