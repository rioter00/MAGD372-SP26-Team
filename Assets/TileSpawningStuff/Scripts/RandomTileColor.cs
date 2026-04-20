using UnityEngine;

public class RandomTileColor : MonoBehaviour
{
    [Header("Leave empty to auto-find all renderers on this tile")]
    [SerializeField] private Renderer[] targetRenderers;

    [Header("Demo terrain colors")]
    [SerializeField] private Color roadColor = new Color(0.25f, 0.25f, 0.25f);
    [SerializeField] private Color iceColor = new Color(0.70f, 0.88f, 1.00f);
    [SerializeField] private Color dirtColor = new Color(0.45f, 0.29f, 0.16f);
    [SerializeField] private Color sandColor = new Color(0.88f, 0.77f, 0.52f);

    private void Start()
    {
        // Comment out the next line to disable terrain-color randomization.
        ApplyTerrainColor();
    }

    [ContextMenu("Apply Terrain Color")]
    public void ApplyTerrainColor()
    {
        if (targetRenderers == null || targetRenderers.Length == 0)
        {
            targetRenderers = GetComponentsInChildren<Renderer>();
        }

        Color[] terrainColors = { roadColor, iceColor, dirtColor, sandColor };
        Color chosenColor = terrainColors[Random.Range(0, terrainColors.Length)];

        foreach (Renderer rend in targetRenderers)
        {
            if (rend == null || !rend.enabled)
                continue;

            rend.material.color = chosenColor;
        }
    }
}