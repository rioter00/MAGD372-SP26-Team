using UnityEngine;
using System.Collections.Generic;

public class TileTrigger1 : MonoBehaviour
{
    public Transform spawnPoint;

    public GameObject[] StraightTiles;
    public GameObject[] LeftTiles;
    public GameObject[] RightTiles;

    public float TileLife = 10f;

    private bool hasSpawned = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasSpawned)
        {
            hasSpawned = true;

            GameObject nextTile = GetRandomTile();

            if (nextTile == null)
            {
                Debug.LogError("No tile available to spawn!");
                return;
            }

            Instantiate(nextTile, spawnPoint.position, spawnPoint.rotation);

            Destroy(transform.parent.gameObject, TileLife);
        }
    }

    GameObject GetRandomTile()
    {
        List<GameObject[]> validTiles = new List<GameObject[]>();

        if (StraightTiles.Length > 0) validTiles.Add(StraightTiles);
        if (LeftTiles.Length > 0) validTiles.Add(LeftTiles);
        if (RightTiles.Length > 0) validTiles.Add(RightTiles);

        if (validTiles.Count == 0)
        {
            Debug.LogError("No tiles assigned in inspector!");
            return null;
        }

        GameObject[] chosenArray = validTiles[Random.Range(0, validTiles.Count)];
        return chosenArray[Random.Range(0, chosenArray.Length)];
    }
}
