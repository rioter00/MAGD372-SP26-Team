using UnityEngine;

public class RoadTile : MonoBehaviour
{
    public Transform exitPoint;
    public TileEndTrigger endTrigger;

    public void Initialize(EndlessTileSpawner spawner)
    {
        if (endTrigger != null)
        {
            endTrigger.Initialize(spawner);
        }
    }
}