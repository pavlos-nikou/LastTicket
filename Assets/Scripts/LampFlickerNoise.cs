using UnityEngine;

public class LampFlickerNoise : MonoBehaviour
{
    [Header("Intensity")]
    public float minIntensity = 0.8f;
    public float maxIntensity = 1.5f;

    [Header("Speed")]
    public float noiseSpeed = 1.5f;

    private Light _light;
    private float _seed;

    void Start()
    {
        _light = GetComponent<Light>();
        if (_light == null) Debug.LogWarning("LampFlickerNoise: No Light component found!", this);

        // Random seed so multiple lamps don't flicker in sync
        _seed = Random.Range(0f, 100f);
    }

    void Update()
    {
        // Sample Perlin noise using time as input
        float noise = Mathf.PerlinNoise(_seed + Time.time * noiseSpeed, 0f);

        // Map the 0�1 noise value to your intensity range
        _light.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
    }
}