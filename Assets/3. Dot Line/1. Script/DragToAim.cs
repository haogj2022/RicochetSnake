using System.Collections.Generic;
using UnityEngine;

public class DragToAim : MonoBehaviour
{
    [SerializeField] private LineRenderer DotLinePrefab;
    [SerializeField] private Transform DotLineContainer;
    [SerializeField] private int DotLineCount = 1;
    private Vector2 StartPoint;
    private Vector2 EndPoint;
    private Vector2 Direction;
    private float MaxDistance = 100f;
    private List<LineRenderer> DotLines = new();

    private void Start()
    {
        for (int i = 0; i < DotLineCount; i++)
        {
            LineRenderer newDotLine = PoolingSystem.Spawn<LineRenderer>(
                DotLinePrefab.gameObject,
                DotLineContainer,
                DotLinePrefab.transform.localScale,
                Vector2.zero,
                Quaternion.identity);
            DotLines.Add(newDotLine);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            StartAiming();
            ShowDotLine();
        }
    }

    private void StartAiming()
    {
        StartPoint = transform.position;
        EndPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Direction = StartPoint - EndPoint;
        float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void ShowDotLine()
    {
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
