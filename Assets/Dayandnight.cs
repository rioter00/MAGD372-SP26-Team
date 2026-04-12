using UnityEngine;

public class DayNight : MonoBehaviour
{
    public float speed = 20f;
    public Light sun;
    public Light moon;
    public float moonIntensity = 0.3f;

    private float angle = 50f;

    void Update()
    {
        angle += speed * Time.deltaTime;
        if (angle >= 360f)
            angle -= 360f;

        transform.rotation = Quaternion.Euler(angle, -30f, 0f);

        if (moon != null)
            moon.transform.rotation = Quaternion.Euler(angle + 180f, -30f, 0f);

        bool isDay = angle < 180f;

        if (sun != null)
            sun.intensity = isDay ? 1f : 0f;

        if (moon != null)
            moon.intensity = isDay ? 0f : moonIntensity;
    }
}