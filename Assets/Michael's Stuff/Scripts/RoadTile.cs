using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RoadTile : MonoBehaviour
{
    [Header("Core")]
    public Transform exitPoint;
    public float fallbackLength = 240f;

    [Header("Lane Centers")]
    public Transform[] laneCenters = new Transform[4];

    [Header("Optional Lane Surface Renderers")]
    public Renderer[] laneRenderers = new Renderer[4];

    [Header("Optional Side Scenery Points")]
    public Transform[] leftSceneryPoints;
    public Transform[] rightSceneryPoints;

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

    public Vector3 GetLaneWorldPosition(int laneIndex)
    {
        if (laneCenters == null || laneCenters.Length == 0)
            return transform.position;

        laneIndex = Mathf.Clamp(laneIndex, 0, laneCenters.Length - 1);
        Transform lane = laneCenters[laneIndex];

        return lane != null ? lane.position : transform.position;
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