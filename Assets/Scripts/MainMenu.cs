using System.Collections;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject interactText;
    public GameObject signInCanvas;
    public GameObject desktopCanvas;

    [Header("Windows Popup")]
    public RectTransform windowsPopup;
    public GameObject itAppCanvas;

    [Header("Player")]
    public PlayerCam playerCam; // drag the PlayerCam GameObject here

    private Vector2 hiddenPos;
    private Vector2 shownPos;

    private bool playerNear = true;
    private bool isOpen = true;
    private static bool isLoggedIn = false;
    private static bool itAppWasOpen = false; // persists when player walks away

    private bool popupVisible = false;
    private bool isSwitching = false;

    void Start()
    {
        interactText.SetActive(false);
        signInCanvas.SetActive(false);
        desktopCanvas.SetActive(false);
        itAppCanvas.SetActive(false);
        OpenUI(); // open the menu immediately for testing

        shownPos = windowsPopup.anchoredPosition;
        hiddenPos = new Vector2(shownPos.x + 600f, shownPos.y);

        windowsPopup.anchoredPosition = hiddenPos;
        windowsPopup.gameObject.SetActive(false);

        if (playerCam != null) playerCam.enabled = false;
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

        if (popupVisible && !isSwitching && Input.GetMouseButtonDown(0))
        {
            isSwitching = true;
            StartCoroutine(OpenItApp());
        }
    }

    void OpenUI()
    {
        isOpen = true;
        interactText.SetActive(false);

        if (playerCam != null) playerCam.enabled = false; // fix issue 1

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;

        if (isLoggedIn)
        {
            signInCanvas.SetActive(false);
            desktopCanvas.SetActive(true);

            // fix issue 3: restore ItApp if it was open before
            if (itAppWasOpen)
            {
                itAppCanvas.SetActive(true);
                windowsPopup.gameObject.SetActive(true);
                windowsPopup.anchoredPosition = shownPos;
                popupVisible = true;
            }
        }
        else
        {
            signInCanvas.SetActive(true);
            desktopCanvas.SetActive(false);
        }
    }

    public void CloseUI()
    {
        isOpen = false;

        signInCanvas.SetActive(false);
        desktopCanvas.SetActive(false);

        // don't hide itAppCanvas or popup — just hide the whole computer UI
        // state is preserved in itAppWasOpen for when they return

        if (playerCam != null) playerCam.enabled = true; // re-enable camera

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    public void Login()
    {
        isLoggedIn = true;
        signInCanvas.SetActive(false);
        desktopCanvas.SetActive(true);

        StartCoroutine(ShowPopupAfterDelay());
    }

    private IEnumerator ShowPopupAfterDelay()
    {
        yield return new WaitForSecondsRealtime(3f);

        windowsPopup.gameObject.SetActive(true);
        windowsPopup.anchoredPosition = hiddenPos;

        yield return SlidePopup(hiddenPos, shownPos, 0.4f);

        popupVisible = true;
    }

    private IEnumerator OpenItApp()
    {
        GameTracker.Instance.TrackInteraction("ItApp");
        GameTracker.Instance.CompleteEvent("ItAppOpened");

        popupVisible = false;
        itAppCanvas.SetActive(true);
        itAppWasOpen = true;
        isSwitching = false;

        yield return null; // kept as coroutine in case you want to add entrance anim later
    }

    private IEnumerator SlidePopup(Vector2 from, Vector2 to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float k = 1f - Mathf.Pow(1f - Mathf.Clamp01(t / duration), 3f);
            windowsPopup.anchoredPosition = Vector2.Lerp(from, to, k);
            yield return null;
        }
        windowsPopup.anchoredPosition = to;
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

    void OnDestroy()
    {
        isLoggedIn = false;
        itAppWasOpen = false;
    }
}