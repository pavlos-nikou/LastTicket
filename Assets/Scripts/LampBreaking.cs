using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class LampBreaking : MonoBehaviour
{
    public Light targetLight;

    public float startIntensity = 0.2f;
    public float maxIntensity = 8f;
    public float rampDuration = 5f;



    private void Start()
    {
        targetLight.intensity = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(RampAndShutOff());

        }
    }

    IEnumerator RampAndShutOff()
    {
        float timer = 0f;

        targetLight.GetComponent<AudioSource>().Play();
        while (timer < rampDuration)
        {
            timer += Time.deltaTime;

            float progress = timer / rampDuration;

            // Slowly increases light intensity
            float intensity = Mathf.Lerp(startIntensity, maxIntensity, progress);

            // Adds flickering
            intensity += Random.Range(-1f, 1.5f);

            targetLight.intensity = Mathf.Max(0f, intensity);

            yield return null;


        }



        // Sudden shut off
        targetLight.intensity = 0f;
        targetLight.enabled = false;
        yield return new WaitForSeconds(1.5f); // brief pause before breaking
        GameObject[] ItLamp = GameObject.FindGameObjectsWithTag("ItLamp");
        ItLamp[0].GetComponent<Light>().enabled = true;
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }
}
