using UnityEngine;

public class pointText2 : MonoBehaviour
{

    public LaneMover_PlayerOnly pointManager;
    public TMPro.TextMeshProUGUI text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Points: " + Mathf.CeilToInt(pointManager.points / 10f).ToString(); ;
    }
}
