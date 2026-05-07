using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameTracker : MonoBehaviour
{
    public static GameTracker Instance;

    public GameObject printerNote;
    public float flickerDuration = 15f;
    public AudioSource printerSound;
    public GameObject printerSpotlight;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }

    public void TrackInteraction(string name) => Debug.Log("Interaction: " + name);
    public void TrackPasswordAttempt(bool success) => Debug.Log("Password attempt: " + success);
    public void CompleteEvent(string name) => Debug.Log("Event: " + name);
    public void DiscoverClue(string name) => Debug.Log("Clue: " + name);

    public void SendMessageToStory(string message)
    {
        Debug.Log("Story Message: " + message);
        switch (message)
        {
            case "VictimCompLogin":
                StartCoroutine(PrintNote());
                // StartCoroutine(slamDeskDrawers());
                break;

            case "PaperClueFound":
                StartCoroutine(SpotlightStopFlicker());
                GameObject[] KillerComputer = GameObject.FindGameObjectsWithTag("Computer_K");
                KillerComputer[0].GetComponent<BoxCollider>().enabled = true;
                break;
            case "KillerCompInteractExit":
                break;
        }

    }

    private IEnumerator PrintNote()
    {
        printerSound.Play();
        printerSpotlight.SetActive(true);
        yield return new WaitForSeconds(6f); // wait a frame for sound to start
        printerNote.SetActive(true);
    }

    private IEnumerator SpotlightStopFlicker()
    {
        printerSpotlight.SetActive(false);
        yield return null;
    }
    //     private IEnumerator slamDeskDrawers()
    //     {
    //         GameObject[] drawers = GameObject.FindGameObjectsWithTag("drawer");

    //         drawers[0].GetComponent<DrawerSlam>().Slam();
    //         yield return null;
    //     }
}

