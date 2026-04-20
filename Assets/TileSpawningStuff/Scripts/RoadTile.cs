using System.Collections.Generic;
using UnityEngine;

public class RoadTile : MonoBehaviour
{
    [Header("Core")]
    public Transform exitPoint;
    public TileEndTrigger endTrigger;

    [Header("Lane Centers (0 = far left, 3 = far right)")]
    public Transform[] laneCenters = new Transform[4];

    [Header("Optional Side Scenery")]
    public bool spawnSideScenery = false;
    public Transform[] leftSceneryPoints;
    public Transform[] rightSceneryPoints;
    public GameObject[] sideSceneryPrefabs;
    [Range(0f, 1f)] public float scenerySpawnChance = 0.45f;

    private readonly List<GameObject> spawnedScenery = new();

    public void Initialize(EndlessTileSpawner spawner)
    {
        if (endTrigger != null)
            endTrigger.Initialize(spawner);

        // Easy to disable later:
        // uncheck Spawn Side Scenery in the Inspector
        // or comment out the next two lines.
        if (spawnSideScenery)
            SpawnSideScenery();
    }

    public Vector3 GetLaneWorldPosition(int laneIndex)
    {
        if (laneCenters == null || laneCenters.Length == 0)
            return transform.position;

        laneIndex = Mathf.Clamp(laneIndex, 0, laneCenters.Length - 1);

        Transform lane = laneCenters[laneIndex];
        return lane != null ? lane.position : transform.position;
    }

    private void SpawnSideScenery()
    {
        ClearSideScenery();
        SpawnFromPoints(leftSceneryPoints);
        SpawnFromPoints(rightSceneryPoints);
    }

    private void SpawnFromPoints(Transform[] points)
    {
        if (points == null || points.Length == 0) return;
        if (sideSceneryPrefabs == null || sideSceneryPrefabs.Length == 0) return;

        foreach (Transform point in points)
        {
            if (point == null) continue;
            if (Random.value > scenerySpawnChance) continue;

            int index = Random.Range(0, sideSceneryPrefabs.Length);
            GameObject spawned = Instantiate(sideSceneryPrefabs[index], point.position, point.rotation, transform);
            spawnedScenery.Add(spawned);
        }
    }

    private void ClearSideScenery()
    {
        for (int i = 0; i < spawnedScenery.Count; i++)
        {
            if (spawnedScenery[i] != null)
                Destroy(spawnedScenery[i]);
        }

        spawnedScenery.Clear();
    }
}