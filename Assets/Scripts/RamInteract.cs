using UnityEngine; 

public class RamInteract : MonoBehaviour
{
    public GameObject interactText;
    private bool playerNear = false;
    public InspectItemCamera inspectScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactText.SetActive(false);
    }

    void Update()
    {
        if (PauseMenu.IsPaused) return; // ignore all input while paused

        if (playerNear && Input.GetKeyDown(KeyCode.E))
        {
            if (!inspectScript.isInspecting)
            {
                interactText.SetActive(false);
                inspectScript.StartInspection();
            }
        }
        if (playerNear && Input.GetKeyDown(KeyCode.Escape))
        {
            if (inspectScript.isInspecting)
            {
                interactText.SetActive(true);
                inspectScript.StopInspection();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;

            if (!inspectScript.isInspecting)
                interactText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            interactText.SetActive(false);

            if (inspectScript.isInspecting)
               inspectScript.StopInspection();
        }
    }
}
