using UnityEngine;

public class TileEndTrigger : MonoBehaviour
{
    private EndlessTileSpawner spawner;
    private bool hasTriggered;

    public void Initialize(EndlessTileSpawner spawnerRef)
    {
        spawner = spawnerRef;
        hasTriggered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;

        hasTriggered = true;
        spawner.SpawnNextTile();
    }
}