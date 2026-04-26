using System.Collections;
using UnityEngine;

public class RunnerGameFlow : MonoBehaviour
{
    [Header("Objects To Hide / Show")]
    public GameObject[] hideWhenStartPressed;
    public GameObject[] showWhenStartPressed;

    [Header("Speed")]
    public RunnerSpeedSource speedSource;

    [Header("Timing")]
    public float startDelaySeconds = 3f;
    public bool autoStartForTesting = false;

    private Coroutine startRoutine;

    private void Start()
    {
        if (speedSource != null)
            speedSource.ResetToMenuState();

        SetObjectsActive(showWhenStartPressed, false);

        if (autoStartForTesting)
            OnStartPressed();
    }

    public void OnStartPressed()
    {
        if (startRoutine != null)
            StopCoroutine(startRoutine);

        SetObjectsActive(hideWhenStartPressed, false);
        SetObjectsActive(showWhenStartPressed, true);

        if (speedSource != null)
            speedSource.ResetToMenuState();

        startRoutine = StartCoroutine(BeginRunAfterDelay());
    }

    public void ReturnToMenuState()
    {
        if (startRoutine != null)
        {
            StopCoroutine(startRoutine);
            startRoutine = null;
        }

        if (speedSource != null)
            speedSource.ResetToMenuState();

        SetObjectsActive(hideWhenStartPressed, true);
        SetObjectsActive(showWhenStartPressed, false);
    }

    private IEnumerator BeginRunAfterDelay()
    {
        yield return new WaitForSecondsRealtime(startDelaySeconds);

        if (speedSource != null)
            speedSource.BeginRun();

        startRoutine = null;
    }

    private void SetObjectsActive(GameObject[] objects, bool activeState)
    {
        if (objects == null)
            return;

        foreach (GameObject obj in objects)
        {
            if (obj != null)
                obj.SetActive(activeState);
        }
    }
}