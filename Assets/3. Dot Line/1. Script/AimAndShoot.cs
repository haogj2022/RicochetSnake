using System.Collections.Generic;
using UnityEngine;

public class AimAndShoot : MonoBehaviour
{
    #region SetUpVisuals
    private void Start()
    {
        GameManager.Instance.OnMoveCompleted += OnMoveCompleted;

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

        BodyRadius = Mathf.Max(transform.localScale.x, transform.localScale.y) / 2;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnMoveCompleted -= OnMoveCompleted;
    }

    private void OnMoveCompleted()
    {
        if (GameManager.Instance.GetAmmoCount() > 0)
        {
            CanShoot = true;
        }
        else
        {
            CanShoot = false;
        }
    }
    #endregion SetUpVisuals

    #region HandleMouseEvent
    [SerializeField] private float MinAngle = 10f;
    [SerializeField] private float MaxAngle = 170f;
    private float CurrentAngle;
    private float BodyRadius;
    private bool CanAim;
    private bool CanShoot;
    private Vector2 StartPoint;
    private Vector2 EndPoint;
    private Vector2 AimDirection;

    private void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (Vector3.Distance(mousePos, transform.position) < BodyRadius)
            {
                CanAim = true;
            }
            else
            {
                CanAim = false;
            }
        }

        if (Input.GetMouseButton(0) && CanAim)
        {
            StartAiming();

            if (CurrentAngle > MinAngle && CurrentAngle < MaxAngle)
            {
                ShowDotLine();
                CanShoot = true;
            }
        }

        if (Input.GetMouseButtonUp(0) && CanAim && CanShoot)
        {
            DotLineContainer.SetActive(false);
            CanShoot = false;
            GameManager.Instance.DecreaseAmmoCount();
            GameManager.Instance.OnSnakeShot();
        }
    }

    private void StartAiming()
    {
        StartPoint = transform.position;
        EndPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        AimDirection = StartPoint - EndPoint;
        CurrentAngle = Mathf.Atan2(AimDirection.y, AimDirection.x) * Mathf.Rad2Deg;

        if (CurrentAngle > MinAngle && CurrentAngle < MaxAngle)
        {
            transform.rotation = Quaternion.Euler(0, 0, CurrentAngle);
        }
    }
    #endregion HandleMouseEvent

    #region VisualizeDotLine
    [SerializeField] private LineRenderer DotLinePrefab;
    [SerializeField] private GameObject DotLineContainer;
    [SerializeField] private int DotLineCount = 2;
    [SerializeField] private GameObject SnakeHeadVisualPrefab;
    private List<LineRenderer> DotLines = new();
    private List<Transform> Visuals = new();

    private void ShowDotLine()
    {
        DotLineContainer.SetActive(true);

        for (int i = 0; i < DotLines.Count; i++)
        {
            Vector2 newPos = StartPoint;
            Vector2 newDir = AimDirection.normalized;
            RaycastHit2D hit = Physics2D.CircleCast(newPos, BodyRadius, newDir);
            DotLines[i].SetPosition(0, newPos);
            DotLines[i].SetPosition(1, hit.centroid);

            if (hit.collider.gameObject.CompareTag("Finish"))
            {
                float angle = Mathf.Atan2(AimDirection.y, AimDirection.x) * Mathf.Rad2Deg;

                if (Visuals.Count < DotLines.Count)
                {
                    SpawnSnakeHeadVisual();
                }

                Visuals[i].SetPositionAndRotation(hit.centroid, Quaternion.Euler(0, 0, angle));
                continue;
            }

            if (hit.collider.gameObject.CompareTag("Obstacle"))
            {
                StartPoint = hit.centroid - AimDirection * 0.01f;
                AimDirection = Vector2.Reflect(newDir, hit.normal);
                float angle = Mathf.Atan2(AimDirection.y, AimDirection.x) * Mathf.Rad2Deg;

                if (Visuals.Count < DotLines.Count)
                {
                    SpawnSnakeHeadVisual();
                }

                Visuals[i].SetPositionAndRotation(StartPoint, Quaternion.Euler(0, 0, angle));
            }
        }
    }

    private void SpawnSnakeHeadVisual()
    {
        Transform newVisual = PoolingSystem.Spawn<Transform>(
            SnakeHeadVisualPrefab,
            DotLineContainer.transform,
            SnakeHeadVisualPrefab.transform.localScale,
            Vector3.one,
            Quaternion.identity);
        Visuals.Add(newVisual);
    }
    #endregion VisualizeDotLine
}
