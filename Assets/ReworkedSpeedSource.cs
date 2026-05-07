using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

public class ReworkedSpeedSource : RunnerSpeedSource
{
    [Header("Startup")]
    public float startupCruiseSpeed = 40f;
    public float startupAcceleration = 15f;
    public float speedIncreaser = 0.5f;
    public float speedMultiplier = 1f;
    public MaterialTouching mt;

    public float ICE_Multiplier = 1.5f;
    public float SNOW_Multiplier = 0.75f;
    public float GRAVEL_Multiplier = 0.9f;
    public float DIRT_Multiplier = 1f;

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

        currentSpeed += speedIncreaser * Time.deltaTime;
        if (mt.TouchedMaterial == "Ice_15")
        {
            currentSpeed *= ICE_Multiplier;
        }
        else if (mt.TouchedMaterial == "Snowy_Concrete_Pavement_3")
        {
            currentSpeed *= SNOW_Multiplier;
        }
        else if (mt.TouchedMaterial == "Gravel_11")
        {
            currentSpeed *= GRAVEL_Multiplier;
        }
        else if (mt.TouchedMaterial == "Beach_Sand_1")
        {
            currentSpeed *= DIRT_Multiplier;
        }
    }
}