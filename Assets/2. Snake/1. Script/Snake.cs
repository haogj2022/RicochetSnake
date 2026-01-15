using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private Transform BodyPart;
    [SerializeField] private float MoveSpeed = 10f;
    [SerializeField] private float SnakeLength = 2f;
    private Vector2 MoveDirection;
    private Obstacle[] Obstacles;
    private float BodyRadius;
    private bool CanMove = true;

    private List<Transform> BodyParts = new();
    private List<Vector2> HeadPositions = new();
    private Vector2 LastHeadPosition;

    private void Start()
    {
        GameManager.Instance.OnGameOver += OnGameOver;
        Obstacles = FindObjectsOfType<Obstacle>();
        BodyRadius = Mathf.Max(transform.localScale.x, transform.localScale.y) / 2;
        Transform newBodyPart = PoolingSystem.Spawn<Transform>(
            BodyPart.gameObject,
            transform.parent,
            transform.localScale,
            transform.position,
            Quaternion.identity);
        BodyParts.Add(newBodyPart);

        LastHeadPosition = transform.position;
        HeadPositions.Add(LastHeadPosition);
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

        if (Vector2.Distance(transform.position, LastHeadPosition) > SnakeLength)
        {
            LastHeadPosition = transform.position;
            HeadPositions.Add(LastHeadPosition);
        }

        if (HeadPositions.Count > 0)
        {
            Vector2 targetPos = HeadPositions[0];

            BodyParts[0].position = Vector2.MoveTowards(
                BodyParts[0].position, targetPos, MoveSpeed * Time.deltaTime);

            if (Vector2.Distance(BodyParts[0].position, targetPos) < 0.01f)
            {
                HeadPositions.RemoveAt(0);
            }
        }

        if (BodyParts.Count > 1)
        {
            if (Vector2.Distance(BodyParts[0].position, BodyParts[1].position) < 0.01f)
            {
                PoolingSystem.Despawn(BodyPart.gameObject, BodyParts[1].gameObject);
                BodyParts.RemoveAt(1);
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

                Transform newBodyPart = PoolingSystem.Spawn<Transform>(
                    BodyPart.gameObject,
                    transform.parent,
                    transform.localScale,
                    transform.position,
                    Quaternion.identity);
                BodyParts.Add(newBodyPart);

                LastHeadPosition = transform.position;
                HeadPositions.Add(LastHeadPosition);

                MoveDirection = Obstacles[i].GetReflectDirection(transform.position, MoveDirection);

                float angle = Mathf.Atan2(MoveDirection.y, MoveDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
    }
}
