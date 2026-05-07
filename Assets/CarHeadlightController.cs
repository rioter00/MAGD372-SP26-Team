using UnityEngine;

public class CarHeadlightController : MonoBehaviour
{
    [Header("Day/Night Reference")]
    public Light sun;

    [Header("Headlights")]
    public Light leftHeadlight;
    public Light rightHeadlight;

    [Header("When lights turn on")]
    public float sunsetThreshold = -0.03f;

    [Header("Fog Material")]
    public Material fogMaterial;

    [Header("Beam Settings")]
    public float beamLength = 30f;
    [Range(0f, 1f)] public float fogClearStrength = 1f;
    [Range(0.01f, 1f)] public float beamRadius = 0.18f;

    void Update()
    {
        if (sun == null) return;

        float sunDot = Vector3.Dot(sun.transform.forward, Vector3.down);
        bool headlightsOn = sunDot < sunsetThreshold;

        if (leftHeadlight != null) leftHeadlight.enabled = headlightsOn;
        if (rightHeadlight != null) rightHeadlight.enabled = headlightsOn;

        if (fogMaterial == null) return;

        if (leftHeadlight != null)
        {
            fogMaterial.SetVector("_HeadlightPosA", leftHeadlight.transform.position);
            fogMaterial.SetVector("_HeadlightDirA", leftHeadlight.transform.forward);
        }

        if (rightHeadlight != null)
        {
            fogMaterial.SetVector("_HeadlightPosB", rightHeadlight.transform.position);
            fogMaterial.SetVector("_HeadlightDirB", rightHeadlight.transform.forward);
        }

        fogMaterial.SetFloat("_BeamLength", beamLength);
        fogMaterial.SetFloat("_FogClearStrength", headlightsOn ? fogClearStrength : 0f);
        fogMaterial.SetFloat("_BeamRadius", beamRadius);
    }
}