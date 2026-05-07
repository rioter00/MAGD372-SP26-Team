using UnityEngine;

public class Hazards : MonoBehaviour
{
    public LaneMover_PlayerOnly car;
    public float distanceOut = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit: " + collision.collider.name);

        if (collision.collider.CompareTag("Hazard"))
        {
            Debug.Log("HAZARD HIT");
            car.playerDead = true;
        }
        else if (collision.collider.CompareTag("TrafficCone"))
        {
            Debug.Log("TRAFFIC CONE HIT");
            car.TrafficConeHit();
            car.oilOnWindshield = 7;
        }
        else if (collision.collider.CompareTag("PotHole"))
        {
            Debug.Log("POT HOLE HIT");
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(car.transform.position.x, car.transform.position.y+5, car.transform.position.z + distanceOut);
    }
}
