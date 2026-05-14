using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.CompilerServices;


public class SimpleLockPick : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform pick;
    public Slider progressBar;

    [Header("Audio")]
    public AudioSource moveSound;
    public AudioSource sweetSpotSound;

    [Header("Pivot Settings (0 to 1)")]
    [Range(0f, 1f)] public float pivotX = 0.5f;
    [Range(0f, 1f)] public float pivotY = 0f;

    [Header("Rotation Settings")]
    public float rotationSpeed = 200f;

    [Header("Lock Settings")]
    public float targetAngle = 90f;
    public float tolerance = 10f;

    [Header("Progress Settings")]
    public float progress = 0f;
    public float fillSpeed = 0.5f;
    public float drainSpeed = 0.3f;

    void Start()
    {
        ApplyPivot();
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (pick != null)
            ApplyPivot();
    }
#endif

    void ApplyPivot()
    {
        pick.pivot = new Vector2(pivotX, pivotY);
    }

    void Update()
    {
        // INPUT
        float input = Input.GetAxis("Horizontal");

        // ?? MOVING SOUND
        if (Mathf.Abs(input) > 0.01f)
        {
            if (moveSound != null && !moveSound.isPlaying)
                moveSound.Play();
        }
        else
        {
            if (moveSound != null)
                moveSound.Stop();
        }

        // ROTATE PICK
        pick.Rotate(0, 0, -input * rotationSpeed * Time.deltaTime);

        // ANGLE CHECK
        float currentAngle = NormalizeAngle(pick.eulerAngles.z);
        float distance = Mathf.Abs(Mathf.DeltaAngle(currentAngle, targetAngle));

        bool inSweetSpot = distance < tolerance;

        // ?? SWEET SPOT SOUND
        if (inSweetSpot)
        {
            if (sweetSpotSound != null && !sweetSpotSound.isPlaying)
                sweetSpotSound.Play();
        }
        else
        {
            if (sweetSpotSound != null)
                sweetSpotSound.Stop();
        }

        // PROGRESS LOGIC
        if (inSweetSpot)
            progress += fillSpeed * Time.deltaTime;
        else
            progress -= drainSpeed * Time.deltaTime;

        progress = Mathf.Clamp01(progress);
        progressBar.value = progress;

        if (progress >= 1f)
        {
            Debug.Log("LOCK PICKED!");

            StartCoroutine(WaitOnExit());

        }
    }

    float NormalizeAngle(float angle)
    {
        return (angle > 180f) ? angle - 360f : angle;
    }

    private IEnumerator WaitOnExit()
    {
        yield return new WaitForSeconds(1.5f);
        GameObject[] closeUiScript = GameObject.FindGameObjectsWithTag("KillerDrawer");
        closeUiScript[0].GetComponent<DrawerInteract>().UnlockDrawer();
    }
}