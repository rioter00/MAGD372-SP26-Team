using UnityEngine;

public class MaterialTouching : MonoBehaviour
{
    public string TouchedMaterial = "";

    private BoxCollider box;

    private void Awake()
    {
        box = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        Vector3 center = transform.TransformPoint(box.center);
        Vector3 halfExtents = Vector3.Scale(box.size * 0.5f, transform.lossyScale);

        Collider[] hits = Physics.OverlapBox(center, halfExtents, transform.rotation);

        TouchedMaterial = "";

        foreach (Collider hit in hits)
        {
            if (hit.gameObject == this.gameObject)
                continue;

            Renderer renderer = hit.GetComponent<Renderer>();
          
            
                TouchedMaterial = renderer.sharedMaterial.name;
               
            
        }
    }
}
