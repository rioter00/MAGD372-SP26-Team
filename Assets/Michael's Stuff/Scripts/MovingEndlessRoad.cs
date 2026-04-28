using System.Collections.Generic;
using UnityEngine;

public class MovingEndlessRoad : MonoBehaviour
{
    [Header("Tile Prefabs")]
    public RoadTile startingTilePrefab;
    public RoadTile[] randomTilePrefabs;

    [Header("Track Setup")]
    public int minimumTilesOnScreen = 4;
    public Vector3 firstTilePosition = Vector3.zero;
    public float fallbackTileLength = 240f;

    [Header("Player / Camera Anchor")]
    public Transform playerAnchor;
    public float fallbackAnchorZ = 10f;

    [Header("Spawn / Recycle Distances")]
    public float spawnAheadDistance = 700f;
    public float recycleWhenEndZIsBelow = -40f;

    [Header("Movement")]
    public RunnerSpeedSource speedSource;

    private readonly Queue<RoadTile> activeTiles = new Queue<RoadTile>();
    private RoadTile frontMostTile;

    private void Start()
    {
        minimumTilesOnScreen = Mathf.Max(1, minimumTilesOnScreen);

        if (startingTilePrefab == null && (randomTilePrefabs == null || randomTilePrefabs.Length == 0))
        {
            Debug.LogError("MovingEndlessRoad needs at least one tile prefab.");
            return;
        }

        if (startingTilePrefab != null)
            SpawnTileAt(startingTilePrefab, firstTilePosition);
        else
            SpawnRandomTileAtFront();

        EnsureEnoughRoadAhead();
    }

    private void FixedUpdate()
    {
        float speed = speedSource != null ? speedSource.CurrentSpeed : 0f;

        if (speed > 0f)
        {
            float distance = speed * Time.fixedDeltaTime;

            foreach (RoadTile tile in activeTiles)
            {
                if (tile != null)
                    tile.MoveBackward(distance);
            }
        }

        RecyclePassedTiles();
        EnsureEnoughRoadAhead();
    }

    private void RecyclePassedTiles()
    {
        while (activeTiles.Count > 0)
        {
            RoadTile oldestTile = activeTiles.Peek();

            if (oldestTile == null)
            {
                activeTiles.Dequeue();
                continue;
            }

            if (oldestTile.EndZ >= recycleWhenEndZIsBelow)
                break;

            activeTiles.Dequeue();
            Destroy(oldestTile.gameObject);
        }
    }

    private void EnsureEnoughRoadAhead()
    {
        float anchorZ = playerAnchor != null ? playerAnchor.position.z : fallbackAnchorZ;

        while (activeTiles.Count < minimumTilesOnScreen || FrontEndZ() < anchorZ + spawnAheadDistance)
        {
            SpawnRandomTileAtFront();
        }
    }

    private float FrontEndZ()
    {
        if (frontMostTile == null)
            return firstTilePosition.z;

        return frontMostTile.EndZ;
    }

    private void SpawnRandomTileAtFront()
    {
        if (randomTilePrefabs == null || randomTilePrefabs.Length == 0)
        {
            Debug.LogError("No random tile prefabs assigned.");
            return;
        }

        int randomIndex = Random.Range(0, randomTilePrefabs.Length);
        Vector3 spawnPosition = GetNextSpawnPosition();

        SpawnTileAt(randomTilePrefabs[randomIndex], spawnPosition);
    }

    private Vector3 GetNextSpawnPosition()
    {
        if (frontMostTile == null)
            return firstTilePosition;

        if (frontMostTile.exitPoint != null)
            return frontMostTile.exitPoint.position;

        return frontMostTile.transform.position + Vector3.forward * fallbackTileLength;
    }

    private void SpawnTileAt(RoadTile prefab, Vector3 position)
    {
        RoadTile newTile = Instantiate(prefab, position, Quaternion.identity, transform);
        newTile.PrepareForRuntime();

        activeTiles.Enqueue(newTile);
        frontMostTile = newTile;
    }
}