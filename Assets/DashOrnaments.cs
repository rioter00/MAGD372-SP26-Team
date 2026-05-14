using UnityEngine;

public class DashOrnaments : MonoBehaviour
{
    public GameObject[] spots;
    public int Ornament = 1;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Ornament++;
            if (Ornament > 4)
            {
                Ornament = 1;
            }
            UpdateSpots();
        }
    }

    void UpdateSpots()
    {
        for (int i = 0; i < spots.Length; i++)
        {
            spots[i].SetActive((Ornament != 1) && (Ornament - 2 == i));
        }
    }
}
