using System.Collections;
using UnityEngine;

public class GameTracker : MonoBehaviour
{
    public static GameTracker Instance;

    public GameObject printerNote;
    public float flickerDuration = 15f;
    public AudioSource printerSound;
    public GameObject printerSpotlight;



    //github is the worst
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
        ;
        switch (message)
        {
            case "VictimCompLogin":
                StartCoroutine(PrintNote());
                StartCoroutine(slamDeskDrawers());
                break;

            case "PaperClueFound":
                StartCoroutine(SpotlightStopFlicker());
                StartCoroutine(slamDeskDrawersStop());
                GameObject[] KillerComputer = GameObject.FindGameObjectsWithTag("Computer_K");
                KillerComputer[0].GetComponent<BoxCollider>().enabled = true;
                break;
            case "KillerCompInteractExit":
                GameObject MicrowaveDoorTriggerZone = GameObject.Find("TriggerZone");
                MicrowaveDoorTriggerZone.GetComponent<BoxCollider>().enabled = true;
                GameObject Phone = GameObject.Find("VictimPhone");
                Phone.GetComponent<BoxCollider>().enabled = true;
                GameObject[] Microwave = GameObject.FindGameObjectsWithTag("Microwave");
                Microwave[0].GetComponent<AudioSource>().Play();
                GameObject[] ItLamp = GameObject.FindGameObjectsWithTag("ItLamp");
                GameObject[] MicrowaveLightTrigger = GameObject.FindGameObjectsWithTag("MicrowaveLightTrigger");
                MicrowaveLightTrigger[0].GetComponent<BoxCollider>().enabled = true;
                ItLamp[0].GetComponent<Light>().enabled = false;
                break;
            case "PhoneInteract":
                GameObject[] k_drawer = GameObject.FindGameObjectsWithTag("KillerDrawer");
                k_drawer[0].GetComponent<BoxCollider>().enabled = true;
                break;
            case "LockpickSuccess":
                GameObject[] k_drawer2 = GameObject.FindGameObjectsWithTag("KillerDrawer");
                k_drawer2[0].GetComponent<BoxCollider>().enabled = false;
                k_drawer2[0].GetComponent<DrawerInteract>().enabled = false;
                GameObject[] ram = GameObject.FindGameObjectsWithTag("Weapon");
                ram[0].GetComponent<BoxCollider>().enabled = true;
                break;
            case "WeaponPickup":
                StartCoroutine(SpawnBody());
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
    private IEnumerator slamDeskDrawers()
    {
        GameObject[] scarySound = GameObject.FindGameObjectsWithTag("ScarySound");
        scarySound[0].GetComponent<AudioSource>().Play();
        GameObject[] Drawers = GameObject.FindGameObjectsWithTag("drawer");
        foreach (GameObject Drawer in Drawers)
        {
            Debug.Log("Starting slam for drawer: " + Drawer.name);
            Drawer.GetComponent<DrawerSlam>().StartSlamming();
        }
        yield return null;
    }
    private IEnumerator slamDeskDrawersStop()
    {
        GameObject[] Drawers = GameObject.FindGameObjectsWithTag("drawer");
        foreach (GameObject Drawer in Drawers)
        {
            Drawer.GetComponent<DrawerSlam>().StopSlamming();
        }
        yield return null;
    }

    private IEnumerator SpawnBody()
    {
        GameObject body = GameObject.Find("Body");
        body.GetComponent<BoxCollider>().enabled = true;
        body.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(2.8f);
        body.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(1f);
        GameObject deadBodyLight = GameObject.Find("deadBodyLight");
        deadBodyLight.GetComponent<Light>().enabled = true;
        deadBodyLight.GetComponent<LampFlickerNoise>().enabled = true;
    }
}

