using System.Collections;
using UnityEngine;

public class GameTracker : MonoBehaviour
{
    public static GameTracker Instance;

    public GameObject printerNote;
    public float flickerDuration = 2f;

    private void Awake()
    {
        Instance = this;
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
                StartCoroutine(Act3Sequence());
                break;
        }
    }

    private IEnumerator Act3Sequence()
    {
        Light printerSpotlight = GameObject.Find("PrinterSpotlight").GetComponent<Light>();
        LampFlickerNoise[] lamps = FindObjectsByType<LampFlickerNoise>(FindObjectsSortMode.None);
        AudioSource printerSound = GameObject.Find("PrinterSound").GetComponent<AudioSource>();

        foreach (var lamp in lamps)
            lamp.ForceFlicker(flickerDuration);

        if (printerSound != null)
            printerSound.Play();

        yield return new WaitForSeconds(flickerDuration);

        if (printerSpotlight != null)
            printerSpotlight.enabled = true;

        if (printerNote != null)
            printerNote.SetActive(true);

        CompleteEvent("Act3_PrinterTriggered");
    }
}