using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject interactText;
    public GameObject signInCanvas;
    public GameObject desktopCanvas;

    [Header("Popups")]
    public RectTransform notificationPopup; // sliding popup
    public GameObject openedPopup;          // instant popup (Raw Image)

    private Vector2 hiddenPos;
    private Vector2 shownPos;

    private bool playerNear = true;
    private bool isOpen = true;
    private static bool isLoggedIn = false;

    private bool popupVisible = false;

    void Start()
    {
        interactText.SetActive(false);
        signInCanvas.SetActive(true);
        desktopCanvas.SetActive(false);

        shownPos = new Vector2(495, -260);
        hiddenPos = new Vector2(750, -260);

        notificationPopup.anchoredPosition = hiddenPos;
        notificationPopup.gameObject.SetActive(false);

        openedPopup.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
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
        yield return new WaitForSecondsRealtime(15f);

        popupVisible = true;
        notificationPopup.gameObject.SetActive(true);
        notificationPopup.anchoredPosition = hiddenPos;

        float t = 0f;
        float duration = 0.35f;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float k = t / duration;

            notificationPopup.anchoredPosition =
                Vector2.Lerp(hiddenPos, shownPos, k);

            yield return null;
        }

        notificationPopup.anchoredPosition = shownPos;
    }

    void Update()
    {
        if (popupVisible && Input.GetMouseButtonDown(0))
        {
            StartCoroutine(SwitchToSecondPopup());
        }
    }

    private IEnumerator SwitchToSecondPopup()
    {
        popupVisible = false;

        // slide OUT first popup
        float t = 0f;
        float duration = 0.25f;

        Vector2 start = notificationPopup.anchoredPosition;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float k = t / duration;

            notificationPopup.anchoredPosition =
                Vector2.Lerp(start, hiddenPos, k);

            yield return null;
        }

        notificationPopup.gameObject.SetActive(false);

        // INSTANT second popup (NO animation)
        openedPopup.SetActive(true);
    }

}