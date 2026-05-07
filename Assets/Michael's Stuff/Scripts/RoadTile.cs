using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RoadTile : MonoBehaviour
{
    [Header("Core")]
    public Transform exitPoint;
    public float fallbackLength = 240f;

    [Header("Lane Centers")]
    public Transform[] laneCenters = new Transform[4];

    private Rigidbody rb;

    public float EndZ
    {
        get
        {
            CacheRigidbody();

            if (exitPoint != null)
                return exitPoint.position.z;

            return rb.position.z + fallbackLength;
        }
    }

    private void Awake()
    {
        CacheRigidbody();
    }

    public void PrepareForRuntime()
    {
        CacheRigidbody();
    }

    public void MoveBackward(float distance)
    {
        CacheRigidbody();
        rb.MovePosition(rb.position + Vector3.back * distance);
    }

    private void CacheRigidbody()
    {
        if (rb != null)
            return;

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }
}