using UnityEngine;

public class DayNightPivot : MonoBehaviour
{
    public float degreesPerSecond = 6f;
    public Light sun;
    public Light moon;
    public float moonMaxIntensity = 0.2f;

    void Update()
    {
        transform.Rotate(Vector3.right * degreesPerSecond * Time.deltaTime, Space.Self);

        float angle = transform.localEulerAngles.x;

        bool isNight = angle > 180f;

        if (sun != null)
        {
            sun.enabled = !isNight;
            sun.intensity = isNight ? 0f : 1f;
        }

        if (moon != null)
        {
            moon.enabled = isNight;
            moon.intensity = isNight ? moonMaxIntensity : 0f;
        }
    }
}