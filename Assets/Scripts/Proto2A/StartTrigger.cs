using UnityEngine;

public class StartTrigger : MonoBehaviour
{
    public TileGenerator generator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            generator.StartGenerating();
        }
    }
}
