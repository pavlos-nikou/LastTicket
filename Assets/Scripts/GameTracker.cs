using UnityEngine;

public class GameTracker : MonoBehaviour
{
    public static GameTracker Instance;

    // public BoxCollider guardCollider;
    // public BoxCollider keyCollider;
    // public BoxCollider doorCollider;
    public GameObject Paper;

    private void Awake()
    {
        Instance = this;
    }

    public void SendMessageToStory(string message)
    {
        Debug.Log("Story Message: " + message);

        switch (message)
        {
            case "VictimCompExit":
                StartCoroutine(Act3Sequence());
                break;
        }

    }
    private IEnumerator Act3Sequence()
    {

        Light printerSpotlight = GameObject.Find("PrinterSpotlight").GetComponent<Light>();
        LampFlickerNoise[] lamps = GameObject.FindObjectsOfType<LampFlickerNoise>();
        AudioSource printerSound = GameObject.Find("PrinterSound").GetComponent<AudioSource>();

        if (printerSound != null)
            printerSound.Play();

        yield return new WaitForSeconds(flickerDuration);

        if (printerSpotlight != null)
            printerSpotlight.enabled = true;

        if (printerNote != null)
            printerNote.SetActive(true);

        GameTracker.Instance.CompleteEvent("Act3_PrinterTriggered");
    }
}