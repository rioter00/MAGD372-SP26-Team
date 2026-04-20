using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FourLaneTestMover : MonoBehaviour
{
    public float forwardSpeed = 12f;
    public float laneSpacing = 3f;
    public float laneChangeSpeed = 16f;
    public int startingLane = 1;

    private Rigidbody rb;
    private int currentLane;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;

        currentLane = Mathf.Clamp(startingLane, 0, 3);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            currentLane = Mathf.Max(0, currentLane - 1);

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            currentLane = Mathf.Min(3, currentLane + 1);
    }

    private void FixedUpdate()
    {
        float targetX = (-1.5f + currentLane) * laneSpacing;

        Vector3 nextPosition = rb.position;
        nextPosition.x = Mathf.MoveTowards(rb.position.x, targetX, laneChangeSpeed * Time.fixedDeltaTime);
        nextPosition.z += forwardSpeed * Time.fixedDeltaTime;

        rb.MovePosition(nextPosition);
    }
}