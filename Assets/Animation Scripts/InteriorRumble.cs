using UnityEngine;

public class InteriorRumble : MonoBehaviour
{
    public float rumbleStrengthX = .8f;
    public float rumbleStrengthY = 1.5f;
    public float turningRumbleMultiplier = 1.0f; // Multiplier for rumble strength based on turning intensity
    public RectTransform interiorTransform; // Reference to the UI Image component of the interior

    public bool turning = false;
    // Update is called once per frame
    void Update()
    {
        // microadjustment to simulate rumble effect
        float rumbleX = Random.Range(-rumbleStrengthX, rumbleStrengthX);
        float rumbleY = Random.Range(-rumbleStrengthX, rumbleStrengthX);
        if (turning)
        {
            rumbleX *= turningRumbleMultiplier;
            rumbleY *= turningRumbleMultiplier;
        }
        interiorTransform.anchoredPosition = new Vector2(rumbleX, rumbleY);
    }
}
