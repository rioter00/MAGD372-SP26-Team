using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject menuCanvas;
    //public GameObject hudCanvas;

    private bool isPaused = false;

    void Start()
    {
        menuCanvas.SetActive(false);
        //hudCanvas.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    void TogglePauseMenu()
    {
        isPaused = !isPaused;

        menuCanvas.SetActive(isPaused);
        //hudCanvas.SetActive(!isPaused);

        Time.timeScale = isPaused ? 0f : 1f;
    }
}
