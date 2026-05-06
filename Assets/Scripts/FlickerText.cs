using System.Collections;
using UnityEngine;
using TMPro;

public class FlickerText : MonoBehaviour
{
    private TextMeshProUGUI text;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3f, 7f));
            text.alpha = 0.1f;
            yield return new WaitForSeconds(0.05f);
            text.alpha = 1f;
            yield return new WaitForSeconds(0.05f);
            text.alpha = 0.3f;
            yield return new WaitForSeconds(0.05f);
            text.alpha = 1f;
        }
    }
}