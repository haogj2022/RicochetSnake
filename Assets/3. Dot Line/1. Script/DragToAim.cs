using System.Collections.Generic;
using UnityEngine;

public class DragToAim : MonoBehaviour
{
    [SerializeField] private LineRenderer DotLinePrefab;
    [SerializeField] private GameObject DotLineContainer;
    [SerializeField] private int DotLineCount = 2;
    [SerializeField] private float MinAngle = 10f;
    [SerializeField] private float MaxAngle = 170f;
    private float CurrentAngle;
    private Vector2 StartPoint;
    private Vector2 EndPoint;
    private Vector2 Direction;
    private float MaxDistance = 100f;
    private List<LineRenderer> DotLines = new();
    private bool CanDrag = true;

    private void Start()
    {
        GameManager.Instance.OnMoveComplete += OnMoveComplete;
        GameManager.Instance.OnGameOver += OnGameOver;

        for (int i = 0; i < DotLineCount; i++)
        {
            LineRenderer newDotLine = PoolingSystem.Spawn<LineRenderer>(
                DotLinePrefab.gameObject,
                DotLineContainer.transform,
                DotLinePrefab.transform.localScale,
                Vector2.zero,
                Quaternion.identity);
            DotLines.Add(newDotLine);
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnMoveComplete -= OnMoveComplete;
        GameManager.Instance.OnGameOver -= OnGameOver;
    }

    private void OnMoveComplete()
    {
        CanDrag = true;
    }

    private void OnGameOver()
    {
        CanDrag = false;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && CanDrag)
        {
            StartAiming();

            if (CurrentAngle > MinAngle && CurrentAngle < MaxAngle ||
                MinAngle == 0 && MaxAngle == 0)
            {
                ShowDotLine();
            }
        }

        if (Input.GetMouseButtonUp(0) && CanDrag)
        {
            DotLineContainer.SetActive(false);
            CanDrag = false;
        }
    }

    private void StartAiming()
    {
        StartPoint = transform.position;
        EndPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Direction = StartPoint - EndPoint;
        CurrentAngle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;

        if (CurrentAngle > MinAngle && CurrentAngle < MaxAngle ||
            MinAngle == 0 && MaxAngle == 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, CurrentAngle);
        }
    }

    private void ShowDotLine()
    {
        DotLineContainer.SetActive(true);

        for (int i = 0; i < DotLines.Count; i++)
        {
            Vector2 newPos = StartPoint;
            Vector2 newDir = Direction.normalized;
            RaycastHit2D hit = Physics2D.Raycast(newPos, newDir, MaxDistance);

            if (hit)
            {
                DotLines[i].SetPosition(0, newPos);
                DotLines[i].SetPosition(1, hit.point);
                StartPoint = hit.point - Direction * 0.01f;
                Direction = Vector2.Reflect(newDir, hit.normal);
            }
            else
            {
                DotLines[i].SetPosition(0, newPos);
                DotLines[i].SetPosition(1, newPos + newDir * MaxDistance);
                break;
            }
        }
    }
}
