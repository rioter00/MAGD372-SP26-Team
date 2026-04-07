using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AutoForwardMover : MonoBehaviour
{
    public float forwardSpeed = 12f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }

    private void FixedUpdate()
    {
        Vector3 nextPosition = rb.position + Vector3.forward * forwardSpeed * Time.fixedDeltaTime;
        rb.MovePosition(nextPosition);
    }
}