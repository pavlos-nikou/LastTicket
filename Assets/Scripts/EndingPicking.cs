using System.Collections;
using UnityEngine;

public class EndingCanvasSelector : MonoBehaviour
{
    [Header("Current selection canvas")]
    public GameObject currentCanvas;

    [Header("Ending canvases")]
    public GameObject option1Canvas;
    public GameObject option2Canvas;
    public GameObject option3Canvas;

    [Header("Timer")]
    public GameObject targetButton;
    public float shrinkTime = 1f;

    private Vector3 originalScale;

    void Start()
    {

        originalScale = targetButton.transform.localScale;
        StartCoroutine(ShrinkCoroutine());
        option1Canvas.SetActive(false);
        option2Canvas.SetActive(false);
        option3Canvas.SetActive(false);
    }

    public void ShowOption1()
    {
        currentCanvas.SetActive(false);
        option1Canvas.SetActive(true);
    }

    public void ShowOption2()
    {
        currentCanvas.SetActive(false);
        option2Canvas.SetActive(true);
    }

    public void ShowOption3()
    {
        currentCanvas.SetActive(false);
        option3Canvas.SetActive(true);
    }

    public void ShrinkAndDisappear()
    {
        StartCoroutine(ShrinkCoroutine());
    }

    IEnumerator ShrinkCoroutine()
    {
        float timer = 0f;

        while (timer < shrinkTime)
        {
            timer += Time.deltaTime;

            float t = timer / shrinkTime;

            float xScale = Mathf.Lerp(originalScale.x, 0f, t);

            targetButton.transform.localScale = new Vector3(
                xScale,
                originalScale.y,
                originalScale.z
            );

            yield return null;
        }

        targetButton.SetActive(false);
    }
}
