using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EndlessTileSpawner : MonoBehaviour
{
    public RoadTile startingTilePrefab;
    public RoadTile[] randomTilePrefabs;
    public int tilesOnScreen = 6;
    public Vector3 startPosition = Vector3.zero;

    private Queue<RoadTile> activeTiles = new Queue<RoadTile>();
    private Vector3 nextSpawnPosition;

    private void Start()
    {
        nextSpawnPosition = startPosition;

        if (startingTilePrefab != null)
        {
            SpawnTile(startingTilePrefab);
        }

        if (randomTilePrefabs == null || randomTilePrefabs.Length == 0)
        {
            Debug.LogError("No random tile prefabs assigned.");
            return;
        }

        while (activeTiles.Count < tilesOnScreen)
        {
            SpawnRandomTile();
        }
    }

    public void SpawnNextTile()
    {
        SpawnRandomTile();

        if (activeTiles.Count > tilesOnScreen)
        {
            RoadTile oldestTile = activeTiles.Dequeue();
            if (oldestTile != null)
            {
                Destroy(oldestTile.gameObject, 1f);
            }
        }
    }

    private void SpawnRandomTile()
    {
        int index = Random.Range(0, randomTilePrefabs.Length);
        SpawnTile(randomTilePrefabs[index]);
    }

    private void SpawnTile(RoadTile tilePrefab)
    {
        RoadTile newTile = Instantiate(tilePrefab, nextSpawnPosition, Quaternion.identity, transform);
        newTile.Initialize(this);

        activeTiles.Enqueue(newTile);

        if (newTile.exitPoint != null)
        {
            nextSpawnPosition = newTile.exitPoint.position;
        }
        else
        {
            nextSpawnPosition += Vector3.forward * 30f;
        }
    }
}