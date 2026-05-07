using System;
using UnityEngine;

public class MaterialTouching : MonoBehaviour
{
    public string TouchedMaterial = "";
    public LaneMover_PlayerOnly car;
    private BoxCollider box;

    private readonly string[] validMaterials =
    {
        "Ice_15",
        "Dry_Ground_10",
        "Snowy_Concrete_Pavement_3",
        "Gravel_11"
    };

    private void Awake()
    {
        box = GetComponent<BoxCollider>();
    }


            private void Update()
            {
                this.transform.position = new Vector3(car.transform.position.x, car.transform.position.y, car.transform.position.z+40);
                Vector3 center = transform.TransformPoint(box.center);
                Vector3 halfExtents = Vector3.Scale(box.size * 0.45f, transform.lossyScale);


                Collider[] hits = Physics.OverlapBox(center, halfExtents, transform.rotation);

                TouchedMaterial = "";

                foreach (Collider hit in hits)
                {

                    Renderer r = hit.GetComponent<Renderer>();
                    if (r != null && r.sharedMaterial != null)
                    {
                        string matName = r.sharedMaterial.name;
                        foreach (string valid in validMaterials)
                        {
                            if (matName == valid)
                            {
                                TouchedMaterial = matName;
                            }
                        }
                    }
                }
            }
}
