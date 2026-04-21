using UnityEngine;
using System.Collections.Generic;

public enum TileType
{
    Straight,
    Left,
    Right
}

public class TileGenerator : MonoBehaviour
{
    public GameObject[] StraightTiles;
    public GameObject[] LeftTiles;
    public GameObject[] RightTiles;

    public int maxTiles = 10;

    private int currentCount = 0;
    private Transform currentSpawnPoint;

    private TileType lastTileType = TileType.Straight;

    public void StartGenerating()
    {
        currentCount = 0;
        currentSpawnPoint = transform;
        lastTileType = TileType.Straight;
        SpawnNextTile();
    }

    void SpawnNextTile()
    {
        if (currentCount >= maxTiles)
            return;

        GameObject nextTile = GetNextTile();

        if (nextTile == null)
        {
            Debug.LogError("No valid tile");
            return;
        }

        GameObject spawnedTile = Instantiate(nextTile, currentSpawnPoint.position, currentSpawnPoint.rotation);
        Transform nextSpawn = spawnedTile.transform.Find("SpawnPoint");

        if (nextSpawn == null)
        {
            Debug.LogError("SpawnPoint missing");
            return;
        }

        currentSpawnPoint = nextSpawn;
        currentCount++;
        SpawnNextTile();
    }

    GameObject GetNextTile()
    {
        List<(GameObject[] tiles, TileType type)> validOptions = new List<(GameObject[], TileType)>();

        switch (lastTileType)
        {
            case TileType.Left:
                if (StraightTiles.Length > 0) validOptions.Add((StraightTiles, TileType.Straight));
                if (RightTiles.Length > 0) validOptions.Add((RightTiles, TileType.Right));
                break;

            case TileType.Right:
                if (StraightTiles.Length > 0) validOptions.Add((StraightTiles, TileType.Straight));
                if (LeftTiles.Length > 0) validOptions.Add((LeftTiles, TileType.Left));
                break;

            case TileType.Straight:
                if (StraightTiles.Length > 0) validOptions.Add((StraightTiles, TileType.Straight));
                if (LeftTiles.Length > 0) validOptions.Add((LeftTiles, TileType.Left));
                if (RightTiles.Length > 0) validOptions.Add((RightTiles, TileType.Right));
                break;
        }

        if (validOptions.Count == 0)
            return null;

        var choice = validOptions[Random.Range(0, validOptions.Count)];
        lastTileType = choice.type;

        return choice.tiles[Random.Range(0, choice.tiles.Length)];
    }
}
