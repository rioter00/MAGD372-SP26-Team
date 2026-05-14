using TMPro.EditorUtilities;
using UnityEngine;
using TMPro;
public class PointText : MonoBehaviour
{

    public TextMeshProUGUI text;
    public LaneMover_PlayerOnly laneMover;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = Mathf.CeilToInt(laneMover.points / 10f).ToString();
    }
}
