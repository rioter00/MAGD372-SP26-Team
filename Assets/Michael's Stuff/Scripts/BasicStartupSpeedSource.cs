using UnityEngine;

public class BasicStartupSpeedSource : RunnerSpeedSource
{
    [Header("Startup")]
    public float startupCruiseSpeed = 40f;
    public float startupAcceleration = 15f;

    protected float currentSpeed;
    protected bool runStarted;

    public override float CurrentSpeed => currentSpeed;

    private void Awake()
    {
        ResetToMenuState();
    }

    public override void ResetToMenuState()
    {
        runStarted = false;
        currentSpeed = 0f;
    }

    public override void BeginRun()
    {
        runStarted = true;
    }

    public override void StopRun()
    {
        runStarted = false;
    }

    protected virtual float GetTargetSpeed()
    {
        return runStarted ? startupCruiseSpeed : 0f;
    }

    protected virtual void Update()
    {
        currentSpeed = Mathf.MoveTowards(
            currentSpeed,
            GetTargetSpeed(),
            startupAcceleration * Time.deltaTime
        );
    }
}