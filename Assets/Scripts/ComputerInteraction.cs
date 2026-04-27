using UnityEngine;
using TMPro;


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

    void Start()
    {
        interactText.SetActive(false);
        signInCanvas.SetActive(false);
        desktopCanvas.SetActive(false);
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
            isLoggedIn = true;
            signInCanvas.SetActive(false);
            desktopCanvas.SetActive(true);
        }
        else
        {
            Debug.Log("Wrong password");
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
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            interactText.SetActive(false);

            if (isOpen)
                CloseUI();
        }
    }
}