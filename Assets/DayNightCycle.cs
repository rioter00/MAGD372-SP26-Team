using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("References")]
    public Light sun;
    public Light moon;
    public Material daySky;   // Procedural Skybox
    public Material nightSky; // Night sky material

    [Header("Cycle")]
    public float rotationSpeed = 10f;

    [Header("Sky Switch")]
    public float nightSwitchSunDot = -0.03f;

    [Header("Light Intensity")]
    public float maxSunIntensity = 1.2f;
    public float maxMoonIntensity = 0.16f;

    [Header("Sun Colors")]
    public Color noonSunColor = Color.white;
    public Color sunsetSunColor = new Color(1f, 0.42f, 0.12f);
    public Color sunriseSunColor = new Color(1f, 0.55f, 0.2f);

    [Header("Ambient Colors")]
    public Color dayAmbient = new Color(0.75f, 0.78f, 0.82f);
    public Color sunsetAmbient = new Color(0.85f, 0.32f, 0.16f);
    public Color nightAmbient = new Color(0.06f, 0.07f, 0.14f);

    [Header("Fog Colors")]
    public bool controlFog = true;
    public Color dayFog = new Color(0.72f, 0.82f, 0.95f);
    public Color sunsetFog = new Color(0.95f, 0.38f, 0.18f);
    public Color twilightFog = new Color(0.22f, 0.18f, 0.35f);
    public Color nightFog = new Color(0.03f, 0.04f, 0.08f);

    [Header("Procedural Day Sky")]
    public Color daySkyTint = new Color(0.5f, 0.7f, 1f);
    public Color sunsetSkyTint = new Color(1f, 0.38f, 0.18f);
    public Color sunriseSkyTint = new Color(1f, 0.5f, 0.22f);

    public Color dayGroundColor = new Color(0.37f, 0.35f, 0.32f);
    public Color sunsetGroundColor = new Color(0.28f, 0.16f, 0.12f);

    public float dayExposure = 1.25f;
    public float sunsetExposure = 0.95f;
    public float duskDayExposure = 0.8f;

    public float dayAtmosphereThickness = 1.0f;
    public float sunsetAtmosphereThickness = 1.7f;

    public float daySunSize = 0.03f;
    public float sunsetSunSize = 0.06f;

    public float daySunConvergence = 6f;
    public float sunsetSunConvergence = 3f;

    [Header("Night Sky")]
    public float nightSkyExposure = 2.0f;
    public float duskNightSkyExposure = 0.5f;
    public Color nightSkyTint = new Color(0.6f, 0.8f, 1f);
    public Color duskNightSkyTint = new Color(0.25f, 0.4f, 0.5f);

    private bool isNight;

    void Start()
    {
        if (sun == null || moon == null || daySky == null || nightSky == null)
        {
            Debug.LogError("DayNightCycle: Assign sun, moon, daySky, and nightSky.");
            enabled = false;
            return;
        }

        float sunDot = Vector3.Dot(sun.transform.forward, Vector3.down);
        isNight = sunDot < nightSwitchSunDot;

        RenderSettings.skybox = isNight ? nightSky : daySky;
        DynamicGI.UpdateEnvironment();
    }

    void Update()
    {
        sun.transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
        moon.transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);

        float sunDot = Vector3.Dot(sun.transform.forward, Vector3.down);
        float moonDot = Vector3.Dot(moon.transform.forward, Vector3.down);

        float sunAmount = Mathf.Clamp01(sunDot);
        float moonAmount = Mathf.Clamp01(moonDot);

        float horizonPop = 1f - Mathf.Clamp01(Mathf.Abs(sunDot) / 0.35f);
        horizonPop = Mathf.Pow(horizonPop, 0.55f);

        bool isSunsetSide = sun.transform.forward.x > 0f;

        Color targetWarmSun = isSunsetSide ? sunsetSunColor : sunriseSunColor;
        Color targetWarmSky = isSunsetSide ? sunsetSkyTint : sunriseSkyTint;

        sun.intensity = Mathf.Lerp(0.08f, maxSunIntensity, sunAmount) * (1f - horizonPop * 0.2f);
        if (sunDot <= -0.08f) sun.intensity = 0f;

        float moonFade = Mathf.InverseLerp(-0.05f, 0.25f, moonDot);
        moon.intensity = Mathf.Lerp(0.02f, maxMoonIntensity, moonFade);

        sun.color = Color.Lerp(noonSunColor, targetWarmSun, horizonPop);

        Color baseAmbient = Color.Lerp(nightAmbient, dayAmbient, sunAmount);
        RenderSettings.ambientLight = Color.Lerp(baseAmbient, sunsetAmbient, horizonPop);

        if (controlFog)
        {
            Color fog;

            if (sunDot > 0.15f)
            {
                // Day -> Sunset
                float t = Mathf.InverseLerp(0.15f, 1f, sunDot);
                fog = Color.Lerp(sunsetFog, dayFog, t);
            }
            else if (sunDot > -0.08f)
            {
                // Sunset -> Twilight
                float t = Mathf.InverseLerp(-0.08f, 0.15f, sunDot);
                fog = Color.Lerp(twilightFog, sunsetFog, t);
            }
            else
            {
                // Twilight -> Night
                float t = Mathf.InverseLerp(-0.2f, -0.08f, sunDot);
                fog = Color.Lerp(nightFog, twilightFog, t);
            }

            RenderSettings.fogColor = fog;
        }

        RenderSettings.sun = sun.intensity > moon.intensity ? sun : moon;

        UpdateProceduralDaySky(sunDot, horizonPop, targetWarmSky);
        UpdateNightSkyMaterial(sunDot);

        bool shouldBeNight = sunDot < nightSwitchSunDot;

        if (shouldBeNight != isNight)
        {
            isNight = shouldBeNight;
            RenderSettings.skybox = isNight ? nightSky : daySky;
            DynamicGI.UpdateEnvironment();
        }
    }

    void UpdateProceduralDaySky(float sunDot, float horizonPop, Color targetWarmSky)
    {
        if (daySky == null) return;

        if (daySky.HasProperty("_SkyTint"))
        {
            daySky.SetColor("_SkyTint", Color.Lerp(daySkyTint, targetWarmSky, horizonPop));
        }

        if (daySky.HasProperty("_GroundColor"))
        {
            daySky.SetColor("_GroundColor", Color.Lerp(dayGroundColor, sunsetGroundColor, horizonPop));
        }

        if (daySky.HasProperty("_Exposure"))
        {
            float duskDarken = Mathf.InverseLerp(0.15f, -0.15f, sunDot);
            float targetExposure = Mathf.Lerp(dayExposure, sunsetExposure, horizonPop);
            float finalExposure = Mathf.Lerp(targetExposure, duskDayExposure, duskDarken);
            daySky.SetFloat("_Exposure", finalExposure);
        }

        if (daySky.HasProperty("_AtmosphereThickness"))
        {
            daySky.SetFloat("_AtmosphereThickness",
                Mathf.Lerp(dayAtmosphereThickness, sunsetAtmosphereThickness, horizonPop));
        }

        if (daySky.HasProperty("_SunSize"))
        {
            daySky.SetFloat("_SunSize", Mathf.Lerp(daySunSize, sunsetSunSize, horizonPop));
        }

        if (daySky.HasProperty("_SunSizeConvergence"))
        {
            daySky.SetFloat("_SunSizeConvergence",
                Mathf.Lerp(daySunConvergence, sunsetSunConvergence, horizonPop));
        }
    }

    void UpdateNightSkyMaterial(float sunDot)
    {
        if (nightSky == null) return;

        float nightAmount = Mathf.InverseLerp(0.25f, -0.15f, sunDot);

        if (nightSky.HasProperty("_Exposure"))
        {
            float exposure = Mathf.Lerp(duskNightSkyExposure, nightSkyExposure, nightAmount);
            nightSky.SetFloat("_Exposure", exposure);
        }

        if (nightSky.HasProperty("_Tint"))
        {
            Color tint = Color.Lerp(duskNightSkyTint, nightSkyTint, nightAmount);
            nightSky.SetColor("_Tint", tint);
        }
        else if (nightSky.HasProperty("_Color"))
        {
            Color tint = Color.Lerp(duskNightSkyTint, nightSkyTint, nightAmount);
            nightSky.SetColor("_Color", tint);
        }
    }
}