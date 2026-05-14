using System.Collections.Generic;
using UnityEngine;

public class LandscapeManager : MonoBehaviour
{
    [Header("Ground Tiles")]
    public GameObject[] groundTilePrefabs;

    [Header("Environment Prefabs")]
    public string environmentFolderPath = "EnvironmentPrefabs";
    private GameObject[] environmentPrefabs;

    [Header("References")]
    public Transform cleanupReference;

    [Header("Road")]
    public float roadHalfWidth = 12f;
    public float sideTerrainWidth = 80f;
    public float gapFromRoad = 0f;
    public float roadsideClearDistance = 10f;
    public float tileY = 0f;

    [Header("Spawn")]
    public float currentSpawnZ = 50f;
    public float segmentLength = 100f;
    public int segmentsAheadOnStart = 15;
    public float spawnTriggerDistance = 300f;

    [Header("Movement")]
    public float targetMoveSpeed = 80f;
    public float acceleration = 20f;
    private float currentMoveSpeed = 0f;

    [Header("Props")]
    public int minPropsPerSide = 12;
    public int maxPropsPerSide = 25;
    public float propEdgePadding = 2f;
    public float minPropScale = 4f;
    public float maxPropScale = 6f;

    [Header("Cleanup")]
    public float despawnBehindDistance = 80f;

    private readonly List<GameObject> spawnedObjects = new List<GameObject>();

    void Start()
    {
        environmentPrefabs = Resources.LoadAll<GameObject>(environmentFolderPath);

        if (groundTilePrefabs == null || groundTilePrefabs.Length == 0)
        {
            Debug.LogError("LandscapeManager: No ground tile prefabs assigned.");
            return;
        }

        for (int i = 0; i < segmentsAheadOnStart; i++)
        {
            SpawnNextSegment();
        }
    }

    void Update()
    {
        UpdateSpeed();
        MoveAllSpawnedObjects();
        SpawnMoreIfNeeded();
        CleanupOldObjects();
    }

    void UpdateSpeed()
    {
        currentMoveSpeed = Mathf.MoveTowards(currentMoveSpeed, targetMoveSpeed, acceleration * Time.deltaTime);
    }

    void SpawnMoreIfNeeded()
    {
        if (cleanupReference == null)
            return;

        float forwardTargetZ = cleanupReference.position.z + spawnTriggerDistance;

        while (currentSpawnZ < forwardTargetZ)
        {
            SpawnNextSegment();
        }
    }

    public void SpawnNextSegment()
    {
        SpawnSide(-1, currentSpawnZ);
        SpawnSide(1, currentSpawnZ);
        currentSpawnZ += segmentLength;
    }

    void SpawnSide(int side, float segmentZ)
    {
        GameObject tilePrefab = groundTilePrefabs[Random.Range(0, groundTilePrefabs.Length)];

        float centerOffset = roadHalfWidth + gapFromRoad + (sideTerrainWidth * 0.5f);
        float x = side * centerOffset;

        Vector3 tilePos = new Vector3(x, tileY, segmentZ + (segmentLength * 0.5f));
        GameObject tile = Instantiate(tilePrefab, tilePos, Quaternion.identity);

        tile.transform.localScale = new Vector3(sideTerrainWidth / 10f, 1f, segmentLength / 10f);

        spawnedObjects.Add(tile);

        SpawnPropsOnSide(side, segmentZ);
    }

    void SpawnPropsOnSide(int side, float segmentZ)
    {
        if (environmentPrefabs == null || environmentPrefabs.Length == 0)
            return;

        int propCount = Random.Range(minPropsPerSide, maxPropsPerSide + 1);

        float minX = roadHalfWidth + gapFromRoad + roadsideClearDistance;
        float maxX = roadHalfWidth + gapFromRoad + sideTerrainWidth - propEdgePadding;

        if (minX >= maxX)
            return;

        float minZ = segmentZ + propEdgePadding;
        float maxZ = segmentZ + segmentLength - propEdgePadding;

        for (int i = 0; i < propCount; i++)
        {
            GameObject prefab = environmentPrefabs[Random.Range(0, environmentPrefabs.Length)];

            float localX = Random.Range(minX, maxX);
            float x = side * localX;
            float z = Random.Range(minZ, maxZ);

            Vector3 pos = new Vector3(x, tileY, z);
            Quaternion rot = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            GameObject obj = Instantiate(prefab, pos, rot);

            float scale = Random.Range(minPropScale, maxPropScale);
            obj.transform.localScale *= scale;

            spawnedObjects.Add(obj);
        }
    }

    void MoveAllSpawnedObjects()
    {
        float moveAmount = currentMoveSpeed * Time.deltaTime;

        for (int i = 0; i < spawnedObjects.Count; i++)
        {
            if (spawnedObjects[i] != null)
            {
                spawnedObjects[i].transform.position += Vector3.back * moveAmount;
            }
        }

        currentSpawnZ -= moveAmount;
    }

    void CleanupOldObjects()
    {
        if (cleanupReference == null)
            return;

        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            if (spawnedObjects[i] == null)
            {
                spawnedObjects.RemoveAt(i);
                continue;
            }

            if (spawnedObjects[i].transform.position.z < cleanupReference.position.z - despawnBehindDistance)
            {
                Destroy(spawnedObjects[i]);
                spawnedObjects.RemoveAt(i);
            }
        }
    }
}