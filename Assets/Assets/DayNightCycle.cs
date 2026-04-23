using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light sun;
    public Light moon;
    public float rotationSpeed = 10f;

    void Update()
    {
        sun.transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
        moon.transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);

        float sunDot = Vector3.Dot(sun.transform.forward, Vector3.down);
        float moonDot = Vector3.Dot(moon.transform.forward, Vector3.down);

        sun.intensity = Mathf.Clamp01(sunDot);
        moon.intensity = Mathf.Clamp01(moonDot) * 0.5f;

        RenderSettings.sun = sun.intensity > moon.intensity ? sun : moon;
    }
}