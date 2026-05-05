using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject canvasObject;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCanvas();
        }
    }

    void ToggleCanvas()
    {
        if (canvasObject != null)
        {
            canvasObject.SetActive(!canvasObject.activeSelf);
        }
    }
}
