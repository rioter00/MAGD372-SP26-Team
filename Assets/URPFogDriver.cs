using UnityEngine;

public class URPFogDriver : MonoBehaviour
{
    public Material fogMaterial;
    public float fogStart = 25f;
    public float fogEnd = 120f;
    [Range(0f, 1f)] public float fogStrength = 1f;

    void LateUpdate()
    {
        // Turn off Unity's built-in fog so it does not affect the skybox
        RenderSettings.fog = false;

        if (fogMaterial == null) return;

        // Use the fog color from your DayNightCycle script
        fogMaterial.SetColor("_FogColor", RenderSettings.fogColor);
        fogMaterial.SetFloat("_FogStart", fogStart);
        fogMaterial.SetFloat("_FogEnd", fogEnd);
        fogMaterial.SetFloat("_FogStrength", fogStrength);
    }
}