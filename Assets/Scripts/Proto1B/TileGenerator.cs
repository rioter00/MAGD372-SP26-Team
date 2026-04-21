using UnityEngine;
using System.Collections.Generic;

public class TileGenerator : MonoBehaviour
{
    public GameObject[] straightTiles;
    public GameObject[] leftTiles;
    public GameObject[] rightTiles;

    public int maxTiles = 10;

    private int currentCount = 0;
    private Transform currentSpawnPoint;

    public void StartGenerating()
    {
        currentCount = 0;
        currentSpawnPoint = this.transform;
        SpawnNextTile();
    }

    void SpawnNextTile()
    {
        if (currentCount >= maxTiles)
        {
            Debug.Log("Finished generating tiles.");
            return;
        }

        GameObject nextTile = GetRandomTile();

        if (nextTile == null)
        {
            Debug.LogError("No tile available!");
            return;
        }

        GameObject spawnedTile = Instantiate(nextTile, currentSpawnPoint.position, currentSpawnPoint.rotation);
        Transform nextSpawn = spawnedTile.transform.Find("SpawnPoint");

        if (nextSpawn == null)
        {
            Debug.LogError("SpawnPoint not found");
            return;
        }

        currentSpawnPoint = nextSpawn;
        currentCount++;
        SpawnNextTile();
    }

    GameObject GetRandomTile()
    {
        List<GameObject[]> validTiles = new List<GameObject[]>();

        if (straightTiles.Length > 0) validTiles.Add(straightTiles);
        if (leftTiles.Length > 0) validTiles.Add(leftTiles);
        if (rightTiles.Length > 0) validTiles.Add(rightTiles);

        if (validTiles.Count == 0)
        {
            return null;
        }

        GameObject[] chosenArray = validTiles[Random.Range(0, validTiles.Count)];
        return chosenArray[Random.Range(0, chosenArray.Length)];
    }
}
