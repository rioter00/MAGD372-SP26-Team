using UnityEngine;
using System.Collections;

public class LaneMover_PlayerOnly : MonoBehaviour
{
    [Header("Lane Setup")]
    public Transform[] LanePositions;
    public float LaneChangeSpeed = 0.25f;
    public float rotationMultiplier = 1f;
    public bool MultiLaneChange = false;

    [Header("Lane Availability")]
    public bool Lane1Open = true;
    public bool Lane2Open = true;
    public bool Lane3Open = true;
    public bool Lane4Open = true;

    [Header("Debug")]
    public float Rotation;

    private int lane;
    private bool changing;
    private float turn;
    private float targetTurn;
    private Coroutine laneRoutine;

    void Start()
    {
        lane = Mathf.Clamp(0, 0, LanePositions.Length - 1);

        if (LanePositions != null && LanePositions.Length > 0 && LanePositions[lane] != null)
        {
            Vector3 startPos = transform.position;
            startPos.x = LanePositions[lane].position.x;
            transform.position = startPos;
        }
    }

    void Update()
    {
        HandleLaneInput();

        Rotation = transform.rotation.eulerAngles.y;
        float auto = 1f / LaneChangeSpeed;
        turn = Mathf.MoveTowards(turn, targetTurn, Time.deltaTime * auto * 20f);
        transform.rotation = Quaternion.Euler(0, turn, 0);
    }

    void HandleLaneInput()
    {
        if (!changing)
        {
            if (Input.GetKeyDown(KeyCode.A) && lane > 0 && LaneIsOpen(lane - 1))
            {
                lane -= 1;
                StartLaneShift(-1);
            }

            if (Input.GetKeyDown(KeyCode.D) && lane < LanePositions.Length - 1 && LaneIsOpen(lane + 1))
            {
                lane += 1;
                StartLaneShift(1);
            }
        }
        else if (MultiLaneChange)
        {
            if (Input.GetKeyDown(KeyCode.A) && lane > 0 && LaneIsOpen(lane - 1))
            {
                lane -= 1;
                StartLaneShift(-1);
            }

            if (Input.GetKeyDown(KeyCode.D) && lane < LanePositions.Length - 1 && LaneIsOpen(lane + 1))
            {
                lane += 1;
                StartLaneShift(1);
            }
        }
    }

    bool LaneIsOpen(int index)
    {
        if (index == 0) return Lane1Open;
        if (index == 1) return Lane2Open;
        if (index == 2) return Lane3Open;
        if (index == 3) return Lane4Open;
        return true;
    }

    void StartLaneShift(int changeLane)
    {
        if (laneRoutine != null)
            StopCoroutine(laneRoutine);

        laneRoutine = StartCoroutine(ChangeLane(changeLane));
    }

    IEnumerator ChangeLane(int changeLane)
    {
        changing = true;

        Vector3 start = transform.position;
        Vector3 end = LanePositions[lane].position;

        // Keep the player's forward position fixed
        end.z = transform.position.z;
        end.y = transform.position.y;

        float time = 0f;
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

        targetTurn = 0f;
        changing = false;
    }
}