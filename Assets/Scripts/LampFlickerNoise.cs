using UnityEngine;
using System.Collections;

public class LampFlickerNoise : MonoBehaviour
{
    [Header("Intensity")]
    public float minIntensity = 0.8f;
    public float maxIntensity = 1.5f;

    [Header("Speed")]
    public float noiseSpeed = 1.5f;

    private Light _light;
    private float _seed;
    private bool _forcing = false;

    void Start()
    {
        _light = GetComponent<Light>();
        if (_light == null) Debug.LogWarning("LampFlickerNoise: No Light found!", this);
        _seed = Random.Range(0f, 100f);
    }

    void Update()
    {
        if (_forcing) return;
        float noise = Mathf.PerlinNoise(_seed + Time.time * noiseSpeed, 0f);
        _light.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
    }

    public void ForceFlicker(float duration) => StartCoroutine(FlickerRoutine(duration));

    private IEnumerator FlickerRoutine(float duration)
    {
        _forcing = true;
        float t = 0f;
        while (t < duration)
        {
            _light.intensity = Random.value > 0.5f ? maxIntensity : 0f;
            t += 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
        _forcing = false;
    }
}