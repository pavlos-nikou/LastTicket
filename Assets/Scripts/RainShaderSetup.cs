using UnityEngine;

public class RainShaderSetup : MonoBehaviour
{
    public Material rainMaterial;

    void Start()
    {
        rainMaterial.SetFloat("_RainIntensity", 2f);
        rainMaterial.SetFloat("_RainSize", 0.5f);
        rainMaterial.SetFloat("_Distortion", 1f);
        rainMaterial.SetFloat("_Speed", 1f);
    }
}