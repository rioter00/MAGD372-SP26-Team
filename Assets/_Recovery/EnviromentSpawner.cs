using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [Header("Prefab Loading")]
    public string folderPath = "EnvironmentPrefabs";
    private GameObject[] environmentPrefabs;

    [Header("Spawn Settings")]
    public float currentSpawnZ = 20f;
    public float segmentLength = 6f;
    public int segmentsAheadOnStart = 12;
    public float spawnTriggerDistance = 80f;

    [Header("Road Settings")]
    public float roadHalfWidth = 3f;
    public float roadsideClearDistance = 6f;

    [Header("Forest Bands")]
    public float nearBandMin = 1f;
    public float nearBandMax = 4f;
    public int nearBandCount = 1;

    public float midBandMin = 4f;
    public float midBandMax = 8f;
    public int midBandCount = 3;

    public float farBandMin = 8f;
    public float farBandMax = 14f;
    public int farBandCount = 4;

    [Header("Scaling")]
    public float minScale = 4.5f;
    public float maxScale = 5.5f;

    [Header("Movement")]
    public float targetMoveSpeed = 80f;
    public float acceleration = 20f;
    private float currentMoveSpeed = 0f;

    [Header("Cleanup")]
    public Transform cleanupReference;
    public float despawnBehindDistance = 25f;

    [Header("Vertical Placement")]
    public float spawnY = 0f;

    private readonly List<GameObject> spawnedObjects = new List<GameObject>();

    void Start()
    {
        environmentPrefabs = Resources.LoadAll<GameObject>(folderPath);

        if (environmentPrefabs.Length == 0)
        {
            Debug.LogError("No prefabs found in Resources/" + folderPath);
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
        MoveObjects();
        SpawnMoreIfNeeded();
        CleanupOldObjects();
    }

    void UpdateSpeed()
    {
        currentMoveSpeed = Mathf.MoveTowards(
            currentMoveSpeed,
            targetMoveSpeed,
            acceleration * Time.deltaTime
        );
    }

    void SpawnMoreIfNeeded()
    {
        if (cleanupReference == null)
            return;

        while (currentSpawnZ < cleanupReference.position.z + spawnTriggerDistance)
        {
            SpawnNextSegment();
        }
    }

    public void SpawnNextSegment()
    {
        SpawnSegment(currentSpawnZ);
        currentSpawnZ += segmentLength;
    }

    void SpawnSegment(float segmentZ)
    {
        SpawnBand(segmentZ, nearBandMin, nearBandMax, nearBandCount);
        SpawnBand(segmentZ, midBandMin, midBandMax, midBandCount);
        SpawnBand(segmentZ, farBandMin, farBandMax, farBandCount);
    }

    void SpawnBand(float segmentZ, float bandMin, float bandMax, int countPerSide)
    {
        for (int side = -1; side <= 1; side += 2)
        {
            for (int i = 0; i < countPerSide; i++)
            {
                float adjustedMin = Mathf.Max(bandMin, roadsideClearDistance);

                if (adjustedMin >= bandMax)
                    continue;

                GameObject prefab = environmentPrefabs[Random.Range(0, environmentPrefabs.Length)];

                float offsetFromRoad = Random.Range(adjustedMin, bandMax);
                float x = side * (roadHalfWidth + offsetFromRoad);
                float z = segmentZ + Random.Range(0f, segmentLength);

                Vector3 spawnPos = new Vector3(x, spawnY, z);
                Quaternion rot = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

                GameObject obj = Instantiate(prefab, spawnPos, rot);

                float scale = Random.Range(minScale, maxScale);
                obj.transform.localScale *= scale;

                spawnedObjects.Add(obj);
            }
        }
    }

    void MoveObjects()
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