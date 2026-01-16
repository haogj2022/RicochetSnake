using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private Transform SnakeTailPrefab;
    [SerializeField] private SnakeBody BodyPartPrefab;
    [SerializeField] private float MoveSpeed = 10f;
    [SerializeField] private float SnakeLength = 1f;
    private Vector2 MoveDirection;
    private Obstacle[] Obstacles;
    private float BodyRadius;
    private bool CanMove = true;

    private List<Transform> SnakeTails = new();
    private List<SnakeBody> BodyParts = new();
    private List<Vector2> HeadPositions = new();
    private Vector2 LastHeadPosition;

    private void Start()
    {
        GameManager.Instance.OnGameOver += OnGameOver;
        Obstacles = FindObjectsOfType<Obstacle>();
        BodyRadius = Mathf.Max(transform.localScale.x, transform.localScale.y) / 2;

        SpawnNewPart();
        LastHeadPosition = transform.position;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameOver -= OnGameOver;
    }

    private void OnGameOver()
    {
        CanMove = false;
    }

    public void MoveTowardsDirection(Vector2 direction)
    {
        MoveDirection = direction.normalized;
        CanMove = true;
    }

    private void Update()
    {
        CheckCollision();

        if (CanMove)
        {
            transform.position += MoveSpeed * Time.deltaTime * (Vector3)MoveDirection;
        }

        UpdateHeadPositions();
        UpdateSnakeTails();
        UpdateBodyParts();
    }

    private void UpdateHeadPositions()
    {
        if (Vector2.Distance(transform.position, LastHeadPosition) > SnakeLength)
        {
            LastHeadPosition = transform.position;
            HeadPositions.Add(LastHeadPosition);
        }

        if (HeadPositions.Count > 0)
        {
            SnakeTails[0].position = Vector2.MoveTowards(
                SnakeTails[0].position, HeadPositions[0], MoveSpeed * 1.5f * Time.deltaTime);

            if (SnakeTails[0].position == (Vector3)HeadPositions[0])
            {
                HeadPositions.RemoveAt(0);
            }
        }
    }

    private void UpdateSnakeTails()
    {
        if (SnakeTails.Count > 1)
        {
            if (SnakeTails[0].position == SnakeTails[1].position)
            {
                PoolingSystem.Despawn(SnakeTailPrefab.gameObject, SnakeTails[1].gameObject);
                SnakeTails.RemoveAt(1);

                PoolingSystem.Despawn(BodyPartPrefab.gameObject, BodyParts[1].gameObject);
                BodyParts.RemoveAt(1);
            }
        }
    }

    private void UpdateBodyParts()
    {
        if (BodyParts.Count < 2)
        {
            BodyParts[0].SetPosition(SnakeTails[0].position, transform.position);
        }

        for (int i = 1; i < BodyParts.Count; i++)
        {
            BodyParts[i - 1].SetPosition(SnakeTails[i - 1].position, SnakeTails[i].position);

            if (i == BodyParts.Count - 1)
            {
                BodyParts[i].SetPosition(SnakeTails[i].position, transform.position);
            }
        }
    }

    private void CheckCollision()
    {
        for (int i = 0; i < Obstacles.Length; i++)
        {
            if (Obstacles[i].IsOverlappedWith(transform.position, BodyRadius, MoveDirection))
            {
                transform.position = Obstacles[i].GetCircleCenter();
                SpawnNewPart();

                LastHeadPosition = transform.position;
                HeadPositions.Add(LastHeadPosition);

                MoveDirection = Obstacles[i].GetReflectDirection(transform.position, MoveDirection);
                float angle = Mathf.Atan2(MoveDirection.y, MoveDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
    }

    private void SpawnNewPart()
    {
        Transform newTail = PoolingSystem.Spawn<Transform>(
            SnakeTailPrefab.gameObject,
            transform.parent,
            transform.localScale,
            transform.position,
            Quaternion.identity);
        SnakeTails.Add(newTail);

        SnakeBody newBodyPart = PoolingSystem.Spawn<SnakeBody>(
            BodyPartPrefab.gameObject,
            transform.parent,
            transform.localScale,
            transform.position,
            Quaternion.identity);
        BodyParts.Add(newBodyPart);
    }
}
