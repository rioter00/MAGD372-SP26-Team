using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuUI : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject controlsPanel;

    public RectTransform playButton;
    public RectTransform controlsButton;
    public RectTransform exitButton;

    public string gameSceneName = "RandomTileSpawning - MH";

    public float flyTime = 0.6f;
    public float flyDistance = 1500f;

    public void PlayGame()
    {
        StartCoroutine(PlayTransition());
    }

    private IEnumerator PlayTransition()
    {
        Vector2 playStart = playButton.anchoredPosition;
        Vector2 controlStart = controlsButton.anchoredPosition;
        Vector2 exitStart = exitButton.anchoredPosition;

        float timer = 0f;

        while(timer < flyTime)
        {
            timer += Time.deltaTime;
            float t = timer / flyTime;

            playButton.anchoredPosition = Vector2.Lerp(playStart, playStart + Vector2.up * flyDistance, t);
            controlsButton.anchoredPosition = Vector2.Lerp(controlStart, controlStart + Vector2.up * flyDistance, t);
            exitButton.anchoredPosition = Vector2.Lerp(exitStart, exitStart + Vector2.up * flyDistance, t);

            yield return null;
        }

        SceneManager.LoadScene(gameSceneName);
    }

    public void ShowControls()
    {
        mainMenuPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }

    public void ShowMainMenu()
    {
        controlsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
