using UnityEngine;
using UnityEngine.UI;

public class WheelRotate : MonoBehaviour
{

    public RectTransform wheelTransform; // Reference to the UI Image component of the wheel
    public float rotationSpeed = 100f; // Speed of rotation in degrees per second 
    LaneMover_PlayerOnly laneMover; 

    void Start(){
        laneMover = GameObject.FindObjectOfType<LaneMover_PlayerOnly>();
    }

    void Update(){
        RotateWheel(laneMover.Rotation);
    }

    public void RotateWheel(float wheelRotation)
    {
        // rotate wheel image based on range -1 to 1
        var rotationVal = wheelRotation;
        // switch (rotationVal)
        // {
        //     case rotationVal == 0:
        //     wheelTransform.rotation = Quaternion.Euler(0,0, 0);
        //     break;
        //     case (rotationVal > 0):
        //     break;
        //     default:
        // }
        wheelTransform.rotation = Quaternion.Euler(0,0, wheelRotation);
    }
}
