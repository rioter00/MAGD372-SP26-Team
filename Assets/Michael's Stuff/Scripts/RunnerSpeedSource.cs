using UnityEngine;

public abstract class RunnerSpeedSource : MonoBehaviour
{
    public abstract float CurrentSpeed { get; }

    public virtual void ResetToMenuState() { }
    public virtual void BeginRun() { }
    public virtual void StopRun() { }
}