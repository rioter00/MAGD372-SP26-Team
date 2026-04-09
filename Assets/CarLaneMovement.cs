using UnityEngine;
using System.Collections;

public class LaneMover : MonoBehaviour
{
    public Transform[] LanePositions;
    public float LaneChangeSpeed = 0.25f;
    public float rotationMultiplier = 1f;
    public bool MultiLaneChange = false;

    int lane;
    bool changing;
    float turn;
    float targetTurn;
    Coroutine laneRoutine;

    void Start()
    {
        lane = 0;
        transform.position = LanePositions[lane].position;
    }

    void Update()
    {
        if (!changing)
        {
            if (Input.GetKeyDown(KeyCode.A) && lane > 0)
            {
                lane -= 1;
                StartNewLaneRoutine(-1);
            }
            if (Input.GetKeyDown(KeyCode.D) && lane < LanePositions.Length - 1)
            {
                lane += 1;
                StartNewLaneRoutine(1);
            }
        }
        else if (MultiLaneChange)
        {
            if (Input.GetKeyDown(KeyCode.A) && lane > 0)
            {
                lane -= 1;
                StartNewLaneRoutine(-1);
            }
            if (Input.GetKeyDown(KeyCode.D) && lane < LanePositions.Length - 1)
            {
                lane += 1;
                StartNewLaneRoutine(1);
            }
        }

        float auto = 1f / LaneChangeSpeed;
        turn = Mathf.MoveTowards(turn, targetTurn, Time.deltaTime * auto * 20f);
        transform.rotation = Quaternion.Euler(0, turn, 0);
    }

    void StartNewLaneRoutine(int changeLane)
    {
        if (laneRoutine != null)
        {
            StopCoroutine(laneRoutine);

        }
        laneRoutine = StartCoroutine(ChangeLane(changeLane));
    }

    IEnumerator ChangeLane(int changeLane)
    {
        changing = true;
        Vector3 start = transform.position;
        Vector3 end = LanePositions[lane].position;
        float time = 0;

        float maxTurn = (10f / LaneChangeSpeed) * rotationMultiplier;

        while (time < 1f)
        {
            time += Time.deltaTime / LaneChangeSpeed;
            transform.position = Vector3.Lerp(start, end, time);

            float smoothMoveStart = Mathf.SmoothStep(0f, 1f, time * 0.6f);
            float hold = Mathf.Clamp01(smoothMoveStart);
            float smoothMoveEnd = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01((time - 0.85f) * 7f));
            float curve = hold * (1f - smoothMoveEnd);

            targetTurn = changeLane * maxTurn * curve;

            yield return null;
        }

        targetTurn = 0;
        changing = false;
    }
}
