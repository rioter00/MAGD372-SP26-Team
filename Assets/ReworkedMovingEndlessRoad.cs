using System.Collections.Generic;
using UnityEngine;

public class MovingEndlessRoad2 : MonoBehaviour
{
    public RoadTile startingTilePrefab;
    public RoadTile[] randomTilePrefabs;

    public int minimumTilesOnScreen = 4;
    public Vector3 firstTilePosition = Vector3.zero;
    public float fallbackTileLength = 240f;

    public Transform playerAnchor;
    public float fallbackAnchorZ = 10f;

    public float spawnAheadDistance = 700f;
    public float recycleWhenEndZIsBelow = -40f;

    public float startupCruiseSpeed = 40f;
    public float startupAcceleration = 15f;
    public float speedIncreaser = 0.5f;
    public float speedMultiplier = 1f;

    public MaterialTouching mt;
    public float ICE_Multiplier = 1.5f;
    public float SNOW_Multiplier = 0.75f;
    public float GRAVEL_Multiplier = 0.9f;
    public float DIRT_Multiplier = 1f;

    public float currentSpeed;
    public bool runStarted = true;

    public float terrainMultiplier = 1f;

    public readonly Queue<RoadTile> activeTiles = new Queue<RoadTile>();
    public RoadTile frontMostTile;

    private void Start()
    {
        minimumTilesOnScreen = Mathf.Max(1, minimumTilesOnScreen);

        if (startingTilePrefab == null && (randomTilePrefabs == null || randomTilePrefabs.Length == 0))
            return;

        if (startingTilePrefab != null)
            SpawnTileAt(startingTilePrefab, firstTilePosition);
        else
            SpawnRandomTileAtFront();

        EnsureEnoughRoadAhead();
    }

    

    private void UpdateTerrainMultiplier()
    {
        terrainMultiplier = 1f;

        if (mt == null)
            return;

        if (mt.TouchedMaterial == "Ice_15")
            terrainMultiplier = ICE_Multiplier;
        else if (mt.TouchedMaterial == "Snowy_Concrete_Pavement_3")
            terrainMultiplier = SNOW_Multiplier;
        else if (mt.TouchedMaterial == "Gravel_11")
            terrainMultiplier = GRAVEL_Multiplier;
        else if (mt.TouchedMaterial == "Beach_Sand_1")
            terrainMultiplier = DIRT_Multiplier;
    }

    private void FixedUpdate()
    {
        UpdateSpeed();

        float finalSpeed = currentSpeed * terrainMultiplier * speedMultiplier;
        float distance = finalSpeed * Time.fixedDeltaTime;

        if (distance > 0f)
        {
            foreach (RoadTile tile in activeTiles)
                if (tile != null)
                    tile.MoveBackward(distance);
        }

        RecyclePassedTiles();
        EnsureEnoughRoadAhead();
    }



    private void UpdateSpeed()
    {
        UpdateTerrainMultiplier();

        if (!runStarted)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, startupAcceleration * Time.deltaTime);
            return;
        }

        if (currentSpeed < startupCruiseSpeed)
        {
            currentSpeed = Mathf.MoveTowards(
                currentSpeed,
                startupCruiseSpeed,
                startupAcceleration * Time.deltaTime
            );
        }
        else
        {
            currentSpeed += speedIncreaser * Time.deltaTime;
        }
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
            SpawnRandomTileAtFront();
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
            return;

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

        return frontMostTile.transform.position + Vector3.forward * fallbackTileLength * terrainMultiplier;
    }

    private void SpawnTileAt(RoadTile prefab, Vector3 position)
    {
        RoadTile newTile = Instantiate(prefab, position, Quaternion.identity, transform);
        newTile.PrepareForRuntime();

        activeTiles.Enqueue(newTile);
        frontMostTile = newTile;
    }
}
