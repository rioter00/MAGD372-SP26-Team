using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FourLaneMover : MonoBehaviour
{
    [Header("Forward Movement")]
    public float forwardSpeed = 30f;

    [Header("Lane Change")]
    public float laneChangeSpeed = 50f;

    [Header("Lane X Positions")]
    public float[] laneXPositions = { -22.5f, -7.5f, 7.5f, 22.5f };

    [Header("Use scene position on start")]
    public bool useClosestLaneFromScenePosition = true;

    [Header("Fallback starting lane")]
    [Range(0, 3)]
    public int startingLane = 1;

    private Rigidbody rb;
    private int currentLane;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;

        if (useClosestLaneFromScenePosition)
            currentLane = GetClosestLaneIndex(transform.position.x);
        else
            currentLane = Mathf.Clamp(startingLane, 0, laneXPositions.Length - 1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            currentLane = Mathf.Max(0, currentLane - 1);

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            currentLane = Mathf.Min(laneXPositions.Length - 1, currentLane + 1);
    }

    private void FixedUpdate()
    {
        float targetX = laneXPositions[currentLane];

        Vector3 nextPosition = rb.position;
        nextPosition.x = Mathf.MoveTowards(rb.position.x, targetX, laneChangeSpeed * Time.fixedDeltaTime);
        nextPosition.z += forwardSpeed * Time.fixedDeltaTime;

        rb.MovePosition(nextPosition);
    }

    private int GetClosestLaneIndex(float currentX)
    {
        int closestIndex = 0;
        float closestDistance = Mathf.Abs(currentX - laneXPositions[0]);

        for (int i = 1; i < laneXPositions.Length; i++)
        {
            float distance = Mathf.Abs(currentX - laneXPositions[i]);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }
}